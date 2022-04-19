namespace Messaging.Infrastructure.Messaging
{
	using System;
    using Kernel.Messaging.Messaging;

    [Serializable]
    public class Command : Message
	{
        public Command(Guid id, Guid correlationId) : base(id, correlationId)
        {
        }
	}
}