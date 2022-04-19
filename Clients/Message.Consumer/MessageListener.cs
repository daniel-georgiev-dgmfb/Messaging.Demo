using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.CQRS.Transport;

namespace Message.Consumer
{
    internal class MessageListener : IMessageListener
    {
        public Task<bool> AttachTo(ITransportManager transportManager)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveMessage(byte[] message)
        {
            Console.WriteLine("Message recieved.");
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
