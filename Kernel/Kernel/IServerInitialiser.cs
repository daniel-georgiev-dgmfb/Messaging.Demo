namespace Kernel.Initialisation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Kernel.DependancyResolver;

    /// <summary>
    /// Initialises the server
    /// </summary>
    public interface IServerInitialiser
	{
        Action OnInitialised { get; set; }
        ICollection<string> InitialiserTypes { get; }
		Task Initialise(IDependencyResolver dependencyResolver);
        Task Initialise(IDependencyResolver dependencyResolver, Func<IInitialiser, bool> condition);
        Task Initialise(IEnumerable<IInitialiser> initialisers, IDependencyResolver dependencyResolver, Func<IInitialiser, bool> condition);
    }
}