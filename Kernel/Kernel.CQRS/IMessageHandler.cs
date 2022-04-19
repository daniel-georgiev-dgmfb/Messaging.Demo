namespace Kernel.Messaging.MessageHandling
{
    using System.Threading.Tasks;
    using Kernel.Messaging.Messaging;

    /// <summary>
    /// Provides methods to handle commands
    /// </summary>
    /// <typeparam name="TMessage">The type of the command.</typeparam>
    public interface IMessageHandler<TMessage> where TMessage : Message
	{
		/// <summary>
		/// Handles the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		Task Handle(TMessage command);
	}
}