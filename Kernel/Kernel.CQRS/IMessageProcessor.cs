namespace Kernel.Messaging
{
	using System.Threading.Tasks;
    using Kernel.Messaging.Messaging;

    /// <summary>
    /// Exposes method to process serialized command
    /// </summary>
    public interface IMessageProcessor
    {
		Task ProcessMessage(string command);
		Task ProcessMessage(Message command);
	}
}