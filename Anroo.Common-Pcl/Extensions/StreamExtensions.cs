using System.IO;
using System.Threading.Tasks;

namespace Anroo.Common
{
    public static class StreamExtensions
    {
        public static Task WriteAsync(this Stream stream, byte[] buffer)
        {
            return stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}