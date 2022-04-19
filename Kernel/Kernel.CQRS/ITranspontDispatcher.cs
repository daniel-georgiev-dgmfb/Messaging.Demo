using System.Threading.Tasks;
using Kernel.Messaging.Messaging;

namespace Kernel.Messaging.Transport
{
    public interface ITranspontDispatcher
    {
        ITransportManager TransportManager { get; }
        Task SendMessage<TMessage>(TMessage message) where TMessage : Message;
    }
}
