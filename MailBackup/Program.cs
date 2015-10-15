using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
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
                Log.LogVerbose = options.Verbose;
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

            Log.Info($"Opening imap connection to {Options.ImapServer}:{Options.ImapPort} using {Options.AuthMethod}.");
            // The default port for IMAP over SSL is 993.
            using (ImapClient client = new ImapClient(Options.ImapServer, Options.ImapPort, Options.UserName, Options.Password, Options.AuthMethod, Options.ImapSsl))
            {
                Log.Info($"Retrieving mailboxes.");
                var mailboxes = client.ListMailboxes();

               var targetDirectory= Directory.CreateDirectory(Options.Target);

                // delete obsolete folders
                foreach (
                    var dir in
                        targetDirectory.GetDirectories("*.*", SearchOption.AllDirectories).OrderByDescending(d=>d.FullName) // Descending to ensure child folders are first
                                       .Where(dir => !mailboxes.Any(m => IsSameDirectory(Path.Combine(targetDirectory.FullName, m), dir.FullName))))
                {
                    Log.Verbose($"Mailbox {dir} is depricated.");
                    dir.Delete(true);
                }



                foreach (var mailbox in mailboxes)
                {
                    this.SynchronizeMailbox(client, mailbox, Directory.CreateDirectory(Path.Combine(targetDirectory.FullName, mailbox)));
                }

                //IEnumerable<uint> uids = client.Search(SearchCondition.All());
                Log.Verbose("Disconnecting.");
            }
            Log.Info("Disconnected.");
        }

        private void SynchronizeMailbox(ImapClient client, string mailbox, DirectoryInfo target)
        {
            var info = client.GetMailboxInfo(mailbox);

            Log.Info($"Synchronizing mailbox {info.Name} ({mailbox})");

            var uids = client.Search(SearchCondition.All(), mailbox);
            Log.Verbose($"Found {uids.Count()} messages online");

        }

        private static bool IsSameDirectory(string pathA, string pathB) => Path.GetFullPath(pathA).TrimEnd('\\').Equals(Path.GetFullPath(pathB).TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase);
    }
    }
