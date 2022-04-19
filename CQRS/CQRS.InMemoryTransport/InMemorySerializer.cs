using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Messaging.Infrastructure.Messaging;
using Kernel.Compression;

namespace Messaging.InMemoryTransport.Serializers
{
    public class InMemorySerializer : IMessageSerialiser
    {
        public InMemorySerializer(ICompression encoder)
        {
            this.Encoder = encoder;
        }

        public ICompression Encoder { get; }

        public object[] Deserialize(Stream stream, IList<Type> messageTypes)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string data)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(string data)
        {
            throw new NotImplementedException();
        }

        public async Task<object> Deserialize(Stream stream)
        {
            var formatter = new BinaryFormatter();
            try
            {
                using (var ms = new MemoryStream())
                {
                    stream.Position = 0;
                    await this.Encoder.Decompress(stream, ms);
                    ms.Position = 0;
                    var message = formatter.Deserialize(ms);
                    return message;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SerialiseAsync(Stream stream, object message)
        {
            this.Serialize(stream, new[] { message });
            using (var ms = new MemoryStream())
            {
                stream.Position = 0;
                await this.Encoder.Compress(stream, ms);
                ms.Position = 0;
                stream.SetLength(0);
                await ms.CopyToAsync(stream);
            }
        }

        public void Serialize(Stream stream, object[] o)
        {
            var formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, o[0]);
            }
            catch (SerializationException)
            {
                throw;
            }
        }

        public string Serialize(object o)
        {
            throw new NotImplementedException();
        }
    }
}