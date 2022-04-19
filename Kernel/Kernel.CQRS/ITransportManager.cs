using System.Threading.Tasks;

namespace Kernel.Messaging.Transport
{
	public interface ITransportManager
    {
        Task Initialise();
        Task Start();
        Task Stop();
        Task<bool> EnqueueMessage(byte[] message);
        Task RegisterListener(IMessageListener listener);
    }
}