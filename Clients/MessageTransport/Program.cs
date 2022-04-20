using CQRS.Infrastructure.Commands.Logging;
using DeflateCompression;
using Message.InMemory.Messaging.Client.Logging;
using Messaging.Infrastructure.Transport;
using Messaging.InMemoryTransport;
using Messaging.InMemoryTransport.Serializers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Message.InMemory.Messaging.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var messageDispatched = 0;
            //var resolver = new UnityDependencyResolver();
            //using (new InformationLogEventWriter())
            //{
            //    ApplicationConfiguration.RegisterDependencyResolver(() => resolver);
            //    ApplicationConfiguration.RegisterServerInitialiserFactory(() => new ServerInitialiser());

            //    Program.InitializeServer();
            //}
            var compressor = new DeflateCompressor();
            var serialaser = new InMemorySerializer(compressor);
            var logger = new MockLogProvider();
            var transport = new InMemoryQueueTransport(logger, () => new TransportConfiguration());
            await transport.Start();
            var transportManager = new TransportManager(transport, null);
            var dispatcher = new TransportDispatcher(transportManager ,serialaser);//resolver.Resolve<ITranspontDispatcher>();


            var internalListener = new MessageListener();
            await dispatcher.TransportManager.RegisterListener(internalListener);
            var tasks = new List<Task>();
            for (var i = 0; i < 1; i++)
            {
                var batch = i;
                var task = Task.Factory.StartNew(async () =>
                {
                    for (var j = 0; j < 100; j++)
                    {
                        var command = new LogErrorCommand(Guid.NewGuid(), Guid.NewGuid());
                        Interlocked.Increment(ref messageDispatched);
                        Console.WriteLine(String.Format("Dispatched message No: {0}, batch: {1}. Total number messages: {2}", j, batch, messageDispatched));
                        //Thread.Sleep(10);
                        await dispatcher.SendMessage(command);
                    }
                });

                tasks.Add(task);
            }

            Console.ReadLine();
        }

        //private static void InitializeServer()
        //{
        //    using (new InformationLogEventWriter())
        //    {
        //        var container = ApplicationConfiguration.Instance.DependencyResolver;
        //        DIRegistration.Register(container);
        //        var initialiser = ApplicationConfiguration.Instance.ServerInitialiserFactory();
                
        //        var task = initialiser.Initialise(container)
        //              .ContinueWith(t =>
        //              {
        //                  throw t.Exception;
        //              }, TaskContinuationOptions.OnlyOnFaulted);
        //    }
        //}
    }
}