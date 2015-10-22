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
#if !DEBUG
            catch (Exception ex)
            {
                Log.LogVerbose = true;

                Log.Error($"FATAL: {ex.Message}");
                Exception inner = ex;
                while ((inner = inner.InnerException) != null)
                    Log.Error($"INNER: {ex.Message}");

                Log.Verbose($"{ex.StackTrace}");
            }
#else
            finally
            {
                Console.WriteLine("Press any key...");
                Console.ReadKey(true);
            }
#endif
        }
    }
}