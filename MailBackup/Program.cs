using System;
using System.Collections.Generic;
using System.Linq;

namespace MailBackup
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var options = Options.Parse(args);
                if (options == null)
                    return;
                Log.LogVerbose = options.Verbose;
                var program = new Operation(options);
                program.Run();
            }
            finally
            {
#if DEBUG
                Console.WriteLine("Press any key...");
                Console.ReadKey(true);
#endif
            }
        }
    }
}