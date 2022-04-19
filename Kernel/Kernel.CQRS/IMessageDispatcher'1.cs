using System.Threading.Tasks;
using Kernel.Messaging.Messaging;

namespace Kernel.Messaging.Dispatching
{
    public interface IMessageDispatcher<TMessage> : IMessageDispatcher where TMessage : Message
    {
        Task SendMessage(TMessage message);
    }
}