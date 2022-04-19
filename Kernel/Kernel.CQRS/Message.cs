using System;

namespace Kernel.Messaging.Messaging
{
    [Serializable]
    public abstract class Message
    {
        public Guid Id { get; }
        public Guid CorrelationId { get; }

        protected Message(Guid id, Guid correlationId)
        {
            this.Id = id;
            this.CorrelationId = correlationId;
        }
    }
}