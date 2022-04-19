using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.CQRS.Transport;
using Kernel.Initialisation;
using Kernel.Logging;
using ServerInitialisation;
using UnityResolver;

namespace Message.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageDispatched = 0;
            var resolver = new UnityDependencyResolver();
            using (new InformationLogEventWriter())
            {
                ApplicationConfiguration.RegisterDependencyResolver(() => resolver);
                ApplicationConfiguration.RegisterServerInitialiserFactory(() => new ServerInitialiser());

                Program.InitializeServer();
            }
            var dispatcher = resolver.Resolve<ITranspontDispatcher>();
            var internalListener = new MessageListener();
            
            dispatcher.TransportManager.RegisterListener(internalListener);

            Console.ReadLine();
            //Task.WaitAll(tasks.ToArray());
        }

        private static void InitializeServer()
        {
            using (new InformationLogEventWriter())
            {
                var container = ApplicationConfiguration.Instance.DependencyResolver;

                var initialiser = ApplicationConfiguration.Instance.ServerInitialiserFactory();

                var task = initialiser.Initialise(container)
                      .ContinueWith(t =>
                      {
                          throw t.Exception;
                      }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}
