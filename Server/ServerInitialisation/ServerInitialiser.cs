using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DeflateCompression.Initialisation;
using Kernel.DependancyResolver;
using Kernel.Initialisation;
using Kernel.Logging;
using Kernel.Reflection;
using Logging.Initialisation;
using MemoryCacheProvider.Initialisation;
using MessageDistpacher.Initialisation;
using MessageHandling.Initialisation;
using Messaging.InMemoryTransport.Initialisation;
using Serialisation.JSON.Initialisation;
using Serialisation.Xml.Initialisation;
using Shared.Initialisation;

namespace ServerInitialisation
{
	public class ServerInitialiser : IServerInitialiser
	{
        private Action _onInitialised = () => { };
        private IEnumerable<Assembly> AssemblyToAdd
        {
            get
            {
                yield return typeof(XmlSerializerInitialiser).Assembly;
                yield return typeof(CacheProviderInitialiser).Assembly;
                yield return typeof(DeflateCompressorInitialiser).Assembly;
                yield return typeof(JsonSerializerInitialiser).Assembly;
                yield return typeof(InMemoryTransportInitialiser).Assembly;
                yield return typeof(MessageDispatchertInitialiser).Assembly;
                yield return typeof(MessageHandlingInitialiser).Assembly;
                yield return typeof(WindowsEventLogLoggerInitialiser).Assembly;
            }
        }

        public ServerInitialiser()
        {
            this.InitialiserTypes = new List<string>();
            this._onInitialised = () => { };
        }

        public ICollection<string> InitialiserTypes { get; }

        public Action OnInitialised
        {
            get
            {
                return this._onInitialised;
            }
            set
            {
                this._onInitialised = value;
            }
        }

        public async Task Initialise(IDependencyResolver dependencyResolver)
		{
			await this.Initialise(dependencyResolver, i => true);
		}

		public async Task Initialise(IDependencyResolver dependencyResolver, Func<IInitialiser, bool> condition)
		{
			this.DiscoverAndRegisterTypes(dependencyResolver);
			var initialisers = this.GetInitialisers();
			await this.Initialise(initialisers, dependencyResolver, condition);
		}
        
        /// <summary>
        /// Runs all initialisers in parallel
        /// </summary>
        /// <param name="initialisers"></param>
        public async Task Initialise(IEnumerable<IInitialiser> initialisers, IDependencyResolver dependencyResolver, Func<IInitialiser, bool> condition)
		{
			var exceptions = new ConcurrentQueue<Exception>();
			//Aggregate all exeptions and throw 
			foreach (var x in initialisers)
			{
				if (!this.InitialiserTypes.Contains(x.Type.AssemblyQualifiedName) && !condition(x))
					continue;

				using (new InformationLogEventWriter(String.Format("Initialiser {0}", x.GetType().Name)))
				{
					try
					{
						await x.Initialise(dependencyResolver);
					}
					catch (Exception ex)
					{
						LoggerManager.WriteExceptionToEventLog(ex);
						exceptions.Enqueue(ex);
					}
				}
			};

			if (exceptions.Count > 0)
				throw new AggregateException(exceptions);
            this._onInitialised();
		}

		/// <summary>
		/// Discovers and instansiates all initialisers
		/// </summary>
		/// <returns></returns>
		private IEnumerable<Initialiser> GetInitialisers()
		{
			return
				ReflectionHelper
				.GetAllTypes
				(
					x =>
					(
						!x.IsAbstract &&
						typeof(Initialiser).IsAssignableFrom(x)
					)
				)
                .Select(x => Activator.CreateInstance(x) as Initialiser)
                .Where(x => x.AutoDiscoverable)
                .Union(this.InitialiserTypes.Select(x => 
                {
                    try
                    {
                        return Type.GetType(x, (an) =>
                        {
                            if (String.IsNullOrWhiteSpace(x))
                                return null;
                            var assembly = AssemblyScanner.ScannableAssemblies.Where(a => a.FullName == an.FullName)
                            .FirstOrDefault();
                            if (assembly == null)
                                throw new InvalidOperationException(String.Format("Assembly name: {0} can't be resolved.", an));
                            return assembly;
                        }, (a, s, b) =>
                        {
                            return a.GetType(s, b);
                        });
                    }
                    catch(Exception e)
                    {
                        throw;
                    }
                })
                .Select(x => Activator.CreateInstance(x) as Initialiser))
				.OrderBy(x => x.Order);
		}

		/// <summary>
		/// Discovers and register types.
		/// </summary>
        private void DiscoverAndRegisterTypes(IDependencyResolver dependencyResolver)
        {
            var scannableAssemblies = AssemblyScanner.ScannableAssemblies
                 .Union(this.AssemblyToAdd);

            var typeToRegister = ReflectionHelper.GetAllTypes(scannableAssemblies, t => !t.IsAbstract && !t.IsInterface && typeof(IAutoRegister).IsAssignableFrom(t));

            //get all transient type and register them
            var transientTypes = typeToRegister.Where(t => typeof(IAutoRegisterAsTransient).IsAssignableFrom(t));
            foreach (var t in transientTypes)
            {
                dependencyResolver.RegisterType(t, Lifetime.Transient);
            }

            //get all transient type and register them
            var singletonTypes = typeToRegister.Where(t => typeof(IAutoRegisterAsSingleton).IsAssignableFrom(t));
            foreach (var t in singletonTypes)
            {
                dependencyResolver.RegisterType(t, Lifetime.Singleton);
            }
        }
    }
}