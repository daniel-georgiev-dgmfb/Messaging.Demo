using System;
using System.Collections.Generic;
using Kernel.Messaging.Transport;

namespace Messaging.Infrastructure.Transport
{
    public class TransportConfiguration : ITransportConfiguration
    {
        public TransportConfiguration()
        {
            this.ConsumerPeriod = TimeSpan.FromMilliseconds(100);
            this.MaxDegreeOfParallelism = Environment.ProcessorCount;
            this.Listeners = new List<IMessageListener>();
        }
        public virtual int MaxDegreeOfParallelism { get; set; }
        public virtual TimeSpan ConsumerPeriod { get; set; }
        public ICollection<IMessageListener> Listeners { get; }
    }
}