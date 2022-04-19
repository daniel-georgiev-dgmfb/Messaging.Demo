using System.Threading.Tasks;
using Kernel.Messaging.Messaging;

namespace Kernel.Messaging.Dispatching
{
	public interface IMessageDispatcher
    {
        Task SendMessage(Message message);
    }
}