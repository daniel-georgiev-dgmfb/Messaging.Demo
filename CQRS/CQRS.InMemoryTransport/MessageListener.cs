using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Messaging.Infrastructure.Messaging;
using Kernel.Messaging.MessageHandling;
using Kernel.Messaging.Transport;

namespace Messaging.InMemoryTransport
{
    internal class MessageListener : IMessageListener
    {
        private readonly Func<IHandlerResolver> _handlerResolver;
        private readonly Func<IHandlerInvoker> _handlerInvoker;
        private readonly IMessageSerialiser _serialiser;
        private bool _isListening;
        public MessageListener(Func<IHandlerResolver> handlerResolver, Func<IHandlerInvoker> handlerInvoker, IMessageSerialiser serialiser)
        {
            this._handlerResolver = handlerResolver;
            this._handlerInvoker = handlerInvoker;
            this._serialiser = serialiser;
        }
        public Task<bool> AttachTo(ITransportManager transportManager)
        {
            this._isListening = true;
            transportManager.RegisterListener(this);
            return Task.FromResult(true);
        }

        public async Task ReceiveMessage(byte[] message)
        {
            if (!this._isListening)
                return;
            var formatter = new BinaryFormatter();
            try
            {
                using (var stream = new MemoryStream(message))
                {
                    var deserialised = await this._serialiser.Deserialize(stream);

                    var handlerResolver = this._handlerResolver();
                    var handlers = handlerResolver.ResolveAllHandlersFor(deserialised.GetType())
                        .Distinct();
                    var invoker = this._handlerInvoker();
                    await invoker.InvokeHandlers(handlers, deserialised);
                }
                return;
            }
            catch (Exception)
            {
                throw;
            }
    }

        public Task<bool> Start()
        {
            this._isListening = true;
            return Task.FromResult(true);
        }

        public Task<bool> Stop()
        {
            this._isListening = false;
            return Task.FromResult(true);
        }
    }
}