namespace Kernel.Initialisation
{
    using System;
    using System.Threading.Tasks;
    using Kernel.DependancyResolver;

    /// <summary>
    /// Initialises the server
    /// </summary>
    public interface IInitialiser
    {
        byte Order { get; }
        Type Type { get; }
        bool AutoDiscoverable { get; }
        Task Initialise(IDependencyResolver dependencyResolver);
    }
}