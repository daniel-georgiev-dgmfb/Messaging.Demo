using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kernel.Messaging.MessageHandling;

namespace Message.InMemory.Messaging.Client.Handlers
{
	internal class HandlerFactorySettings : IHandlerResolverSettings
    {
        private ICollection<Assembly> _limitAssembliesTo = new List<Assembly>
        {

        };
        public IEnumerable<Assembly> LimitAssembliesTo
        {
            get
            {
                return this._limitAssembliesTo;
            }
        }

        public bool HasCustomAssemlyList
        {
            get
            {
                return this.LimitAssembliesTo != null && this.LimitAssembliesTo.Count() > 0;
            }
        }

        internal void AddAssembly(Assembly assembly)
        {
            this._limitAssembliesTo.Add(assembly);
        }

        internal void ClearList()
        {
            this._limitAssembliesTo.Clear();
        }
    }
}