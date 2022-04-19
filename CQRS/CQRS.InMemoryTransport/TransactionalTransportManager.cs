using System;
using System.Threading.Tasks;
using Kernel.Messaging.Transaction;
using Kernel.Logging;

namespace Messaging.InMemoryTransport
{
    internal class TransactionalTransportManager : TransportManager
    {
        private readonly ITransaction _transaction;
        
        public TransactionalTransportManager(InMemoryQueueTransport transport, ILogProvider logProvider, ITransaction transaction):base(transport, logProvider)
        {
           
            this._transaction = transaction;
        }

        public override async Task<bool> EnqueueMessage(byte[] message)
        {
            try
            {
                if (!base.Transport.IsTransactional)
                    throw new InvalidOperationException("Non trasactional transport.");
                this._transaction.Begin();
                var result = await this.Transport.Send(message);
                this._transaction.Commit();
                return result;
            }
            catch(Exception e)
            {
                this._transaction.Abort();
                Exception inner;
                this.LogProvider.TryLogException(e, out inner);
                throw;
            }
        }
    }
}