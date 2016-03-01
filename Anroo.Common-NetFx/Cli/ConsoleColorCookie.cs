using System;

namespace Anroo.Common
{
    public class ConsoleColorCookie : IDisposable
    {
        private readonly ConsoleColor _prevColor;

        public ConsoleColorCookie(ConsoleColor newColor)
        {
            _prevColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
        }

        public void Dispose()
        {
            Console.ForegroundColor = _prevColor;
        }
    }
}