using System;
using System.Linq;

namespace Anroo.Common.Extensions
{
    public static class ByteArrayExtensions
    {
        public static byte[] GetRange(this byte[] arr, uint from)
        {
            return GetRange(arr, from, (uint)arr.Length - from);
        }
        public static byte[] GetRange(this byte[] arr, uint from, uint count)
        {
            if (@from > arr.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(@from));
            }

            byte[] newArr = new byte[count];
            for (uint srcIdx = @from, newIdx = 0; newIdx < count; srcIdx++, newIdx++)
            {
                newArr[newIdx] = arr[srcIdx];
            }
            return newArr;
        }

        public static string ToCSharpString(this byte[] arr)
        {
            return "{" + arr.Aggregate("", (current, b) => current + "0x" + b.ToString("X") + ", ").RemoveEnd(2) + "}";
        }

        public static string ToMacAddressString(this byte[] arr)
        {
            return arr.Aggregate("", (current, b) => current + b.ToString("X") + "-").RemoveEnd(1);
        }
    }
}