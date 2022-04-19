using System.Collections.Generic;
using System.Reflection;

namespace Kernel.Messaging.MessageHandling
{
    public interface IHandlerResolverSettings
    {
        IEnumerable<Assembly> LimitAssembliesTo { get; }
        bool HasCustomAssemlyList { get; }
    }
}