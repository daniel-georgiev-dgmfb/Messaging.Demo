using System;
using System.Threading.Tasks;
using Kernel.Messaging.Dispatching;
using Kernel.Messaging.Messaging;
using Kernel.Messaging.Transport;
using Messaging.Infrastructure.Messaging;

namespace Messaging.MessageDistpacher
{
	public class CommandDispatcher : IMessageDispatcher<Command>
    {
        private readonly ITranspontDispatcher _transpontHandler;

        public CommandDispatcher(ITranspontDispatcher transpontHandler)
        {
            this._transpontHandler = transpontHandler;
        }
        Task IMessageDispatcher.SendMessage(Message message)
        {
            return Task.Factory.StartNew(async () =>
            {
                try
                {
                    await _transpontHandler.SendMessage(message);
                }
                catch (Exception e)
                {
                    HandleError(message, e);
                }
            });
        }

        public Task SendMessage(Command message)
        {
            return ((IMessageDispatcher)this).SendMessage(message);
        }

        private void HandleError(Message message, Exception e)
        {
            throw new NotImplementedException();
        }
    }
}