using System;
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
    internal class ListenerTests
    {
        [Test]
        public async Task ListnerTest_one_listener()
        {
            //ARRANGE
            var logger = new LogProviderMock();
            Func<ITransportConfiguration> configuration = () => new TransportConfiguration();
            var transport = new InMemoryQueueTransport(logger, configuration);
            var manager = new TransportManager(transport, logger);
            var message = new byte[] { 0, 1, 2 };
            
            byte[] messageReceived = null;
            
            var listener = new MessageListener1(m => messageReceived = m);
            //ACT
            await listener.AttachTo(manager);
            await transport.Start();
            await manager.EnqueueMessage(message);
            Thread.Sleep(500);
            //ASSERT
            
            Assert.AreEqual(message, messageReceived);
        }

        [Test]
        public async Task ListnerTest_2_listeners()
        {
            //ARRANGE
            var logger = new LogProviderMock();
            Func<ITransportConfiguration> configuration = () => new TransportConfiguration();
            var transport = new InMemoryQueueTransport(logger, configuration);
            var manager = new TransportManager(transport, logger);
            var message = new byte[] { 0, 1, 2 };

            byte[] messageReceived1 = null;
            byte[] messageReceived2 = null;

            var listener1 = new MessageListener1(m => messageReceived1= m);
            var listener2 = new MessageListener2(m => messageReceived2 = m);
            //ACT
            await listener1.AttachTo(manager);
            await listener2.AttachTo(manager);
            await transport.Start();
            await manager.EnqueueMessage(message);
            Thread.Sleep(500);
            //ASSERT

            Assert.AreEqual(message, messageReceived1);
            Assert.AreEqual(message, messageReceived2);
        }
    }
}