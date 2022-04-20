using Kernel.DependancyResolver;
using Shared.Initialisation;
using System.Threading.Tasks;

namespace Serialisation.Xml.Initialisation
{
    public class XmlSerializerInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 0; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterFactory<IXmlSerialiser>(() => new XMLSerialiser(), Lifetime.Transient);
           
            return Task.CompletedTask;
        }
    }
}