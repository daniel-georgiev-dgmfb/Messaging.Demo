using System;
using Messaging.Infrastructure.Messaging;

namespace MessageHandling.Test.MockData.MessageHandling
{
	internal class TestCommand : Command
    {
        public TestCommand(Guid id, Guid correlationId) : base(id, correlationId)
        {
        }
    }
}
