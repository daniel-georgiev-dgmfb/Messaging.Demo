using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CQRS.InMemoryTransportTests.MockData.Commands;
using DeflateCompression;
using Messaging.InMemoryTransport.Serializers;
using NUnit.Framework;

namespace CQRS.InMemoryTransportTests.Serialisers
{
	[TestFixture]
    internal class InMemorySerializerTests
    {
        [Test]
        public async Task Serialised_with_cmpression_test()
        {
            //ARRANGE
            var compression = new DeflateCompressor();
            var serialiser = new InMemorySerializer(compression);
            var command = new TestCommand(Guid.NewGuid(), Guid.NewGuid());
            TestCommand commandDeserialised = null;
            for (var i = 0; i < 1000; i++)
                command.Data.Add(i);
            //ACT
            using (var ms = new MemoryStream())
            {
                await serialiser.SerialiseAsync(ms, command);
                commandDeserialised = await serialiser.Deserialize(ms) as TestCommand;
            }
            //ASSERT
            Assert.AreEqual(command.Id, commandDeserialised.Id);
            Assert.AreEqual(command.CorrelationId, commandDeserialised.CorrelationId);
            Assert.True(command.Data.SequenceEqual(commandDeserialised.Data));
        }
    }
}
