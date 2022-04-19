using System;
using System.Threading.Tasks;
using CQRS.InMemoryTransportTests.MockData;
using Kernel.Messaging.Transport;
using Messaging.Infrastructure.Transport;
using Messaging.InMemoryTransport;
using NUnit.Framework;

namespace CQRS.InMemoryTransportTests.InMemoryTransportTests
{
	[TestFixture]
    internal class ManagerTests
    {
        [Test]
        public async Task EnqueueMessageTest()
        {
            //ARRANGE
            var logger = new LogProviderMock();
            Func<ITransportConfiguration> configuration = () => new TransportConfiguration();
            var transport = new InMemoryQueueTransport(logger, configuration);
            var manager = new TransportManager(transport, logger);
            var message = new byte[] { 0, 1, 2 };
            byte[] dequeuedMessage;
            //ACT
            await transport.Start();
            await manager.EnqueueMessage(message);
            var result = transport.TryDequeue(out dequeuedMessage);
            //ASSERT
            Assert.IsTrue(result);
            Assert.AreEqual(message, dequeuedMessage);
        }
    }
}