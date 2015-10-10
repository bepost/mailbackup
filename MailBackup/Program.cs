using System;
using System.Collections.Generic;
using System.Linq;

namespace MailBackup
{
    public class Program
    {
        private readonly Options _options;

        private Program(Options options)
        {
            this._options = options;
        }

        public Options Options
        {
            get { return this._options; }
        }

        private static void Main(string[] args)
        {
            try
            {
                var options = Options.Parse(args);
                if (options == null)
                    return;

                var program = new Program(options);
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

        private void Run()
        {
            throw new NotImplementedException();
        }
    }
}