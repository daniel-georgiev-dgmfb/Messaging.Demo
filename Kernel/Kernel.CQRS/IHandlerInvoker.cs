using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kernel.Messaging.MessageHandling
{
    public interface IHandlerInvoker
    {
        Task InvokeHandlers(IEnumerable<object> handlers, object message);
    }
}