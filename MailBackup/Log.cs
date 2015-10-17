using System;
using System.Collections.Generic;
using System.Linq;

namespace MailBackup
{
    public static class Log
    {
        public static bool LogVerbose { get; set; }

        public static void Error(string message)
        {
            EmitMessage(ConsoleColor.Red, message);
        }

        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Verbose(string message)
        {
            if (LogVerbose)
                EmitMessage(ConsoleColor.DarkGray, message);
        }

        public static void Warn(string message)
        {
            EmitMessage(ConsoleColor.Yellow, message);
        }

        private static void EmitMessage(ConsoleColor color, string message)
        {
            if (!LogVerbose)
                return;
            var orgColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = orgColor;
        }
    }
}