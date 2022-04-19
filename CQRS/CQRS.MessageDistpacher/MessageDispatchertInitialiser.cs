using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Messaging.MessageDistpacher;
using Shared.Initialisation;

namespace MessageDistpacher.Initialisation
{
	public class MessageDispatchertInitialiser : Initialiser
    {
        public override byte Order { get { return 0; } }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<CommandDispatcher>(Lifetime.Singleton);
            return Task.CompletedTask;
        }
    }
}