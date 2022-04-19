namespace CQRS.Infrastructure.Messaging
{
	using System;
    using Kernel.Messaging.Messaging;

    public class Event : Message
	{
        public Event(Guid id, Guid correlationId) : base(id, correlationId)
        {
        }
	}
}