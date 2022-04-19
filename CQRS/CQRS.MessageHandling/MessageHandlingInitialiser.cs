using System;
using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Kernel.Messaging.MessageHandling;
using MessageHandling.Factories;
using MessageHandling.Invocation;
using Shared.Initialisation;

namespace MessageHandling.Initialisation
{
	public class MessageHandlingInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<HandlerResolver>(Lifetime.Transient);
            dependencyResolver.RegisterType<HandlerInvoker>(Lifetime.Transient);
            return Task.CompletedTask;
        }

        internal static Func<Type, IHandlerResolver> RegisterHandlerFactories(IDependencyResolver dependencyResolver)
        {
            return t =>
            {
                throw new InvalidOperationException($"Unknown type: {t.Name}. The type must inherit Command or Event");
            };
        }
    }
}