using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Kernel.Messaging.MessageHandling;
using Kernel.Messaging.Transport;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Transport;
using Messaging.InMemoryTransport.Serializers;
using Shared.Initialisation;

namespace Messaging.InMemoryTransport.Initialisation
{
	public class InMemoryTransportInitialiser : Initialiser
    {
        public override byte Order { get { return 0; } }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<InMemorySerializer>(Lifetime.Transient);
            dependencyResolver.RegisterFactory<ITransportConfiguration>(() => new TransportConfiguration(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<ITranspontDispatcher>(() =>
            {
                var transport = dependencyResolver.Resolve<InMemoryQueueTransport>();
                var manager = dependencyResolver.Resolve<TransportManager>();
                var serializer = dependencyResolver.Resolve<IMessageSerialiser>();
                var listener = new MessageListener(() => dependencyResolver.Resolve<IHandlerResolver>(), () => dependencyResolver.Resolve<IHandlerInvoker>(), serializer);
                listener.AttachTo(manager);
                manager.Initialise();
                manager.Start();
                return new TransportDispatcher(manager, serializer);
            }, Lifetime.Singleton);
            return Task.CompletedTask;
        }
    }
}