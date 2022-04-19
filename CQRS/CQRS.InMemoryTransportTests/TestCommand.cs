using System;
using System.Collections.Generic;
using Messaging.Infrastructure.Messaging;

namespace CQRS.InMemoryTransportTests.MockData.Commands
{
	[Serializable]
    public class TestCommand : Command
    {
        public TestCommand(Guid id, Guid correlationId) : base(id, correlationId)
        {
            this.Data = new List<int>();
        }

        public ICollection<int> Data { get; }
    }
}
