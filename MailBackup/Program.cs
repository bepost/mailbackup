using System;
using System.Collections.Generic;
using System.Linq;
using S22.Imap;

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
            if (Options.ImapPort == 0)
                Options.ImapPort = Options.ImapSsl ? 993 : 143;

            if (Options.Password==null)
            {
                Console.WriteLine("Enter password:");
                Options.Password=Console.ReadLine();
            }
            
            // The default port for IMAP over SSL is 993.
            using (ImapClient client = new ImapClient(Options.ImapServer, Options.ImapPort, Options.UserName, Options.Password, Options.AuthMethod, Options.ImapSsl))
            {
                Console.WriteLine("We are connected!");
            }
        }
    }
}