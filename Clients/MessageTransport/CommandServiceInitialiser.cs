using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Message.InMemory.Messaging.Client.Handlers;

namespace Message.InMemory.Messaging.Client
{
    public class DIRegistration
    {
        internal static Task Register(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<LogErrorCommandHandler>(Lifetime.Transient);
            dependencyResolver.RegisterType<HandlerFactorySettings>(Lifetime.Transient);
           
            return Task.CompletedTask;
        }
    }
}