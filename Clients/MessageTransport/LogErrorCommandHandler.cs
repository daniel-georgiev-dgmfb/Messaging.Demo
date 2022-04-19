using System;
using System.Threading.Tasks;
using CQRS.Infrastructure.Commands.Logging;

namespace Message.InMemory.Messaging.Client.Handlers
{
    internal class LogErrorCommandHandler : HandlerBase<LogErrorCommand>
    {
        public LogErrorCommandHandler()
        {
            
        }
        protected override Task HandleInternal(LogErrorCommand command)
        {
            //Console.WriteLine(String.Format("Command with correlationId recieved: {0}", command.CorrelationId));
            return Task.CompletedTask;
        }
    }
}