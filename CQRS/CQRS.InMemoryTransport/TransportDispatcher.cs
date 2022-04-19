using System.IO;
using System.Threading.Tasks;
using Kernel.Messaging.Messaging;
using Kernel.Messaging.Transport;
using Messaging.Infrastructure.Messaging;

namespace Messaging.InMemoryTransport
{
	public class TransportDispatcher : ITranspontDispatcher
    {
        private readonly IMessageSerialiser _serialiser;
        private readonly ITransportManager _transport;
        public TransportDispatcher(ITransportManager transport, IMessageSerialiser serialiser)
        {
            this._serialiser = serialiser;
            this._transport = transport;
        }

        public ITransportManager TransportManager
        {
            get
            {
                return this._transport;
            }
        }

        public async Task SendMessage<TMessage>(TMessage message) where TMessage : Message
        {
            using (var ms = new MemoryStream())
            {
                await this._serialiser.SerialiseAsync(ms, message);
                var serialsed = ms.ToArray();
                await this._transport.EnqueueMessage(serialsed);
            }
        }
    }
}