using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRS.InMemoryTransportTests.MockData;
using CQRS.InMemoryTransportTests.MockData.Listeners;
using Kernel.Messaging.Transport;
using Messaging.Infrastructure.Transport;
using Messaging.InMemoryTransport;
using NUnit.Framework;

namespace CQRS.InMemoryTransportTests.InMemoryTransportTests
{
	[TestFixture]
    internal class TransportTests
    {
        [Test]
        public void EnqueueMessageTest()
        {
            //ARRANGE
            var logger = new LogProviderMock();
            Func<ITransportConfiguration> configuration = () => new TransportConfiguration();
            var transport = new InMemoryQueueTransport(logger, configuration);
            var message = new byte[] { 0, 1, 2 };
            byte[] dequeuedMessage;
            //ACT
            transport.Start();
            transport.Enque(message);
            var result = transport.TryDequeue(out dequeuedMessage);
            //ASSERT
            Assert.IsTrue(result);
            Assert.AreEqual(message, dequeuedMessage);
        }


        [Test]
        public async Task ConsumeMessagesTest_many_messages_one_worker()
        {
            //ARRANGE
            var messageToEnque = 100000;
            var consumed = 0;
            var logger = new LogProviderMock();
            var listener = new MessageListener1(m => Interlocked.Increment(ref consumed));
            Func<ITransportConfiguration> configuration = () =>
            {
                var config = new TransportConfiguration
                {
                    ConsumerPeriod = Timeout.InfiniteTimeSpan
                };
                config.Listeners.Add(listener);
                return config;
            };
            var transport = new InMemoryQueueTransport(logger, configuration);
            
            var message = new byte[] { 0, 1, 2 };
            await transport.Start();
            for (var i = 0; i < messageToEnque; i++)
            {
                transport.Enque(message);
            }

            //ACT

            var task = await Task.Factory.StartNew(async () =>
            {
                transport.Consume(configuration());
            });
            
            await Task.WhenAll(task);

            //ASSERT
            Assert.AreEqual(messageToEnque, consumed);
            Assert.AreEqual(0, transport.PendingMessages);
        }

        [Test]
        public async Task ConsumeMessagesTest_many_messages_two_workers()
        {
            //ARRANGE
            var messageToEnque = 100000;
            var consumed = 0;
            var logger = new LogProviderMock();
            var listener = new MessageListener1(m => Interlocked.Increment(ref consumed));
            Func<ITransportConfiguration> configuration = () =>
            {
                var config = new TransportConfiguration
                {
                    ConsumerPeriod = Timeout.InfiniteTimeSpan
                };
                config.Listeners.Add(listener);
                return config;
            };
            var transport = new InMemoryQueueTransport(logger, configuration);

            var message = new byte[] { 0, 1, 2 };
            await transport.Start();
            for (var i = 0; i < messageToEnque; i++)
            {
                transport.Enque(message);
            }

            //ACT

            var task = await Task.Factory.StartNew(async () =>
            {
                transport.Consume(configuration());
            });

            var task1 = await Task.Factory.StartNew(async () =>
            {
                transport.Consume(configuration());
            });
            await Task.WhenAll(task, task1);

            //ASSERT
            Assert.AreEqual(messageToEnque, consumed);
            Assert.AreEqual(0, transport.PendingMessages);
        }

        [Test]
        public async Task ConsumeMessagesTest_many_messages_async_enque_auto_dequeue()
        {
            //ARRANGE
            var messageToEnque = 1000000;
            var consumed = 0;
            var logger = new LogProviderMock();
            var listener = new MessageListener1(m => Interlocked.Increment(ref consumed));
            Func<ITransportConfiguration> configuration = () =>
            {
                var config = new TransportConfiguration
                {
                    ConsumerPeriod = TimeSpan.FromMilliseconds(1)
                };
                config.Listeners.Add(listener);
                return config;
            };
            var transport = new InMemoryQueueTransport(logger, configuration);

            var message = new byte[] { 0, 1, 2 };
            await transport.Start();
            var enqueTask = Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < messageToEnque; i++)
                {
                    transport.Enque(message);
                }
            });

            var enqueTask1 = Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < messageToEnque; i++)
                {
                    transport.Enque(message);
                }
            });
            //ACT

            Task.WaitAll(enqueTask, enqueTask1);
            while (!transport.IsEmpty)
            {
                Thread.Sleep(1);
            }

            //ASSERT
            Assert.AreEqual(0, transport.PendingMessages);
            Assert.AreEqual(2 * messageToEnque, consumed);
        }

        [Test]
        public async Task GetAllMessagesTest_many_messages()
        {
            //ARRANGE
            var messageToEnque = 10000;
            var consumed = 0;
            var logger = new LogProviderMock();
            var listener = new MessageListener1(m => Interlocked.Increment(ref consumed));
            Func<ITransportConfiguration> configuration = () =>
            {
                var config = new TransportConfiguration
                {
                    ConsumerPeriod = Timeout.InfiniteTimeSpan
                };
                config.Listeners.Add(listener);
                return config;
            };
            var transport = new InMemoryQueueTransport(logger, configuration);

            var message = new byte[] { 0, 1, 2 };
            await transport.Start();
            for (var i = 0; i < messageToEnque; i++)
            {
                transport.Enque(message);
            }

            //ACT
            var messagesRead = await transport.ReadAllMessages();
            //ASSERT
            Assert.AreEqual(messageToEnque, messagesRead.Count());
            Assert.AreEqual(0, transport.PendingMessages);
        }

        [Test]
        public async Task CopyAllMessagesTest_many_messages()
        {
            //ARRANGE
            var messageToEnque = 10000;
            var consumed = 0;
            var logger = new LogProviderMock();
            var listener = new MessageListener1(m => Interlocked.Increment(ref consumed));
            Func<ITransportConfiguration> configuration = () =>
            {
                var config = new TransportConfiguration
                {
                    ConsumerPeriod = Timeout.InfiniteTimeSpan
                };
                config.Listeners.Add(listener);
                return config;
            };
            var transport = new InMemoryQueueTransport(logger, configuration);

            var message = new byte[] { 0, 1, 2 };
            await transport.Start();
            for (var i = 0; i < messageToEnque; i++)
            {
                transport.Enque(message);
            }
            var destination = new byte[transport.PendingMessages][];
            //ACT
            await transport.CopyMessages(destination);
            //ASSERT
            Assert.AreEqual(messageToEnque, destination.Count());
            Assert.AreEqual(messageToEnque, transport.PendingMessages);
        }
    }
}