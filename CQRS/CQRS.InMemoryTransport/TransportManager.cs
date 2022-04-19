using System;
using System.Threading.Tasks;
using Kernel.Logging;
using Kernel.Messaging.Transport;

namespace Messaging.InMemoryTransport
{
	public class TransportManager : ITransportManager
    {
        protected readonly ITransport Transport;
        protected readonly ILogProvider LogProvider;
        public TransportManager(InMemoryQueueTransport transport, ILogProvider logProvider)
        {
            this.Transport = transport;
            this.LogProvider = logProvider;
        }

        public virtual async Task<bool> EnqueueMessage(byte[] message)
        {
            try
            {
                var result = await this.Transport.Send(message);
                return result;
            }
            catch(Exception e)
            {
                Exception inner;
                this.LogProvider.TryLogException(e, out inner);
                throw;
            }
        }

        public Task Initialise()
        {
            return this.Transport.Initialise();
        }

        public Task RegisterListener(IMessageListener listener)
        {
            this.Transport.Configuration.Listeners.Add(listener);
            return Task.CompletedTask;
        }

        public Task Start()
        {
            return this.Transport.Start();
        }

        public Task Stop()
        {
            return this.Transport.Stop();
        }
    }
}