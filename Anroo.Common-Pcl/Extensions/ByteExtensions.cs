namespace Anroo.Common.Extensions
{
    public static class ByteExtensions
    {
        public static bool Between(this byte val, byte minValue, byte maxValue) 
        {
            return val > minValue && val < maxValue;
        }
    }
}