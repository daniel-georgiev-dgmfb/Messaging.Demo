using System;
using System.Threading;
using System.Threading.Tasks;
using Kernel.Messaging.Transport;

namespace Message.InMemory.Messaging.Client
{
	internal class MessageListener : IMessageListener
    {
        private int _messagesReceived = 0;
        public Task<bool> AttachTo(ITransportManager transportManager)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveMessage(byte[] message)
        {
            Interlocked.Increment(ref this._messagesReceived);
            Console.WriteLine(String.Format("Messages recieved: {0}.", this._messagesReceived));
            return Task.CompletedTask;
        }

        public Task<bool> Start()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Stop()
        {
            throw new NotImplementedException();
        }
    }
}