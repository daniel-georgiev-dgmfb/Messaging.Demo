using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace SmtpNotification.Initialisation
{
    public class SmtpInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<SmtpSender>(Lifetime.Transient);
            dependencyResolver.RegisterType<SmtpNotifier>(Lifetime.Transient);
            dependencyResolver.RegisterType<SmtpDestinationTargetProvider>(Lifetime.Transient);
            return Task.CompletedTask;
        }
    }
}