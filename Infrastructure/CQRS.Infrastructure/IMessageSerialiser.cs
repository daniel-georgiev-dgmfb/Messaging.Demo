using System.IO;
using System.Threading.Tasks;
using Kernel.Compression;
using Kernel.Serialisation;

namespace Messaging.Infrastructure.Messaging
{
    public interface IMessageSerialiser : ISerializer
    {
        ICompression Encoder { get; }
        Task SerialiseAsync(Stream stream, object message);
        Task<object> Deserialize(Stream stream);
    }
}