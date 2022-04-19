using System;
using System.Threading.Tasks;
using MessageHandling.Invocation;
using MessageHandling.Test.MockData.MessageHandling;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MessageHandling.Test.Invocation
{
	[TestFixture]
    internal class HandlerInvocationTests
    {
        [Test]
        public async Task HandlerInvokerTest()
        {
            //ARRANGE
            var result = 0;
            var command = new TestCommand(Guid.NewGuid(), Guid.NewGuid());
            var handler = new TestCommandHandler(() => result = 10);
            var handlerInvoker = new HandlerInvoker();
            var serialised = JsonConvert.SerializeObject(command, command.GetType(), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
            var deserialised = JsonConvert.DeserializeObject(serialised, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
            //ACT
            await handlerInvoker.InvokeHandlers(new[] { handler }, (object)deserialised);

            //ASSERT
            Assert.AreEqual(10, result);
        }
    }
}