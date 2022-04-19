using System.Threading.Tasks;

namespace Kernel.Messaging.Transport
{
    public interface IMessageListener
    {
        Task<bool> Start();
        Task<bool> Stop();
        Task<bool> AttachTo(ITransportManager transportManager);
        Task ReceiveMessage(byte[] message);
    }
}