using System;
using System.Globalization;
using System.Linq;

namespace Anroo.Common
{
    public static class ByteArrayHelpers
    {
        #region Combine
        public static byte[] Combine(byte[] arr1, byte[] arr2)
        {
            byte[] combinedArr = new byte[arr1.Length + arr2.Length];
            Buffer.BlockCopy(arr1, 0, combinedArr, 0, arr1.Length);
            Buffer.BlockCopy(arr2, 0, combinedArr, arr1.Length, arr2.Length);
            return combinedArr;
        }

        public static byte[] Combine(byte[] arr1, byte[] arr2, byte[] arr3)
        {
            byte[] ret = new byte[arr1.Length + arr2.Length + arr3.Length];
            Buffer.BlockCopy(arr1, 0, ret, 0, arr1.Length);
            Buffer.BlockCopy(arr2, 0, ret, arr1.Length, arr2.Length);
            Buffer.BlockCopy(arr3, 0, ret, arr1.Length + arr2.Length,
                             arr3.Length);
            return ret;
        }

        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }

        #endregion

        public static byte[] FromString(string s)
        {
            long value = long.Parse(s, NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat);
            return BitConverter.GetBytes(value)
                .Reverse()
                .SkipWhile(b => b == 0)
                .ToArray();
        }
    }
}