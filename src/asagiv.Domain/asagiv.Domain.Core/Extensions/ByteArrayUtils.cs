using System.Text.Json;

namespace asagiv.Domain.Core.Extensions
{
    public static class ByteArrayUtils
    {
        public static byte[] ToByteArray(this object inputObject)
        {
            return JsonSerializer.SerializeToUtf8Bytes(inputObject);
        }
    }
}
