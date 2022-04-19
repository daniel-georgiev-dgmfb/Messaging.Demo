using Kernel.Messaging.MessageHandling;
using System;
using System.Threading.Tasks;

namespace Message.InMemory.Messaging.Client.Handlers
{
	public abstract class HandlerBase<TMessage> : IMessageHandler<TMessage>, IEquatable<IMessageHandler<TMessage>> where TMessage : Kernel.Messaging.Messaging.Message
    {
        public Task Handle(TMessage command)
        {
            return this.HandleInternal(command);
        }

        public bool Equals(IMessageHandler<TMessage> other)
        {
            if (other == null)
                return false;
            return this.GetType().Equals(other.GetType());
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var handler = obj as IMessageHandler<TMessage>;
            if (handler == null)
                return false;
            return this.Equals(handler);
        }
        protected abstract Task HandleInternal(TMessage command);

        public static bool operator ==(HandlerBase<TMessage> handler1, HandlerBase<TMessage> handler2)
        {
            if (((object)handler1) == null || ((object)handler2) == null)
                return Object.Equals(handler1, handler2);

            return handler1.Equals(handler2);
        }

        public static bool operator !=(HandlerBase<TMessage> handler1, HandlerBase<TMessage> handler2)
        {
            if (((object)handler1) == null || ((object)handler2) == null)
                return !Object.Equals(handler1, handler2);

            return !(handler1.Equals(handler2));
        }
    }
}