using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using S22.Imap;

namespace MailBackup
{
    public sealed class Operation
    {
        private MailMessageWriter _mailMessageWriter;

        public Operation(Options options)
        {
            this.Options = options;
        }

        private static char[] InvalidFileNameChars => Path.GetInvalidFileNameChars();
        private MailMessageWriter MailMessageWriter => this._mailMessageWriter ?? (this._mailMessageWriter = new MailMessageWriter());
        private Options Options { get; }

        private static string CleanSubject(string source)
        {
            // Replace any whitespace to single space
            source = Regex.Replace(source, @"\s+", " ");
            return new string(source.Where(c => !InvalidFileNameChars.Contains(c)).ToArray());
        }

        private static bool IsSameDirectory(string pathA, string pathB)
            => Path.GetFullPath(pathA).TrimEnd('\\').Equals(Path.GetFullPath(pathB).TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase);

        public void Run()
        {
            if (this.Options.ImapPort == 0)
                this.Options.ImapPort = this.Options.ImapSsl ? 993 : 143;

            if (this.Options.Password == null)
            {
                Console.WriteLine("Enter password:");
                this.Options.Password = Console.ReadLine();
            }

            Log.Info($"Opening imap connection to {this.Options.ImapServer}:{this.Options.ImapPort} using {this.Options.AuthMethod}.");
            // The default port for IMAP over SSL is 993.
            using (
                var client = new ImapClient(
                    this.Options.ImapServer,
                    this.Options.ImapPort,
                    this.Options.UserName,
                    this.Options.Password,
                    this.Options.AuthMethod,
                    this.Options.ImapSsl))
            {
                Log.Info($"Retrieving mailboxes.");
                var mailboxes = client.ListMailboxes();

                var targetDirectory = Directory.CreateDirectory(this.Options.Target);

                // delete obsolete folders
                foreach (var dir in
                    targetDirectory.GetDirectories("*.*", SearchOption.AllDirectories).OrderByDescending(d => d.FullName) // Descending to ensure child folders are first
                                   .Where(dir => !mailboxes.Any(m => IsSameDirectory(Path.Combine(targetDirectory.FullName, m), dir.FullName))))
                {
                    Log.Verbose($"Mailbox {dir} is depricated.");
                    dir.Delete(true);
                }

                foreach (var mailbox in mailboxes)
                    this.SynchronizeMailbox(client, mailbox, Directory.CreateDirectory(Path.Combine(targetDirectory.FullName, mailbox)));

                //IEnumerable<uint> uids = client.Search(SearchCondition.All());
                Log.Verbose("Disconnecting.");
            }
            Log.Info("Disconnected.");
        }

        private void SynchronizeMailbox(ImapClient client, string mailbox, DirectoryInfo target)
        {
            var info = client.GetMailboxInfo(mailbox);

            Log.Info($"Synchronizing mailbox {info.Name} ({mailbox}).");

            // Fetch online data
            var onlineUids = client.Search(SearchCondition.All(), mailbox).ToList();
            Log.Verbose($"Found {onlineUids.Count()} messages online.");

            // Match offline data
            foreach (var file in target.GetFiles("*.eml"))
            {
                var fileName = Path.GetFileNameWithoutExtension(file.Name);
                uint fileUid;
                if (!uint.TryParse(fileName.PadLeft(8, '0').Substring(0, 8), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out fileUid))
                {
                    Log.Warn($"Unexpected file '{file.Name}' found in mailbox {mailbox}.");
                    continue;
                }

                if (!onlineUids.Contains(fileUid))
                {
                    Log.Verbose($"Deleting obsolete message '{file.Name}'...");
                    file.Delete();
                }

                onlineUids.Remove(fileUid);
            }

            // Fetch new files
            Log.Info($"Fetching {onlineUids.Count} messages...");
            foreach (var onlineUid in onlineUids)
            {
                var message = client.GetMessage(onlineUid, FetchOptions.Normal, false, mailbox);
                var fileName = $"{onlineUid:x8} {CleanSubject(message.Subject)}.eml";
                this.MailMessageWriter.WriteEml(message, Path.Combine(target.FullName, fileName));
                Log.Verbose($"Fetched message '{fileName}'");
            }
        }
    }
}