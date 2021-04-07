using System;
using McMaster.Extensions.CommandLineUtils;

namespace Modeller
{
    internal static class ConsoleExtensions
    {
        public static void Write(this IConsole console, ConsoleColor color, string message)
        {
            console.ForegroundColor = color;
            console.Write(message);
            console.ResetColor();
        }

        public static void WriteLine(this IConsole console, ConsoleColor color, string message)
        {
            console.ForegroundColor = color;
            console.WriteLine(message);
            console.ResetColor();
        }
    }
}