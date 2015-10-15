using System;
using System.Collections.Generic;
using System.Linq;

namespace MailBackup
{
    public static class Log
    {
        public static  bool LogVerbose { get; set; }
        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Verbose(string message)
        {
            if (!LogVerbose)
                return;
            var orgColor = Console.ForegroundColor;
            Console.ForegroundColor=ConsoleColor.DarkGray;
            Console.WriteLine(message);
            Console.ForegroundColor = orgColor;
        }


    }
}