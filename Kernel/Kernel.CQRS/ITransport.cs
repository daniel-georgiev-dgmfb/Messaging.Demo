using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.Messaging.Transport
{
    public interface ITransport
    {
        ITransportConfiguration Configuration { get; }
        Task<bool> Send(byte[] message);
        bool IsTransactional { get; }
        int PendingMessages { get; }
        Task Initialise();
        Task Start();
        Task Stop();
        Task<IEnumerable<byte[]>> ReadAllMessages();
        Task CopyMessages(byte[][] destination);
    }
}