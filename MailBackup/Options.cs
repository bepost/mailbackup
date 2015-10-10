using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace MailBackup
{
    public sealed class Options
    {
        [Option('i', "imap", Required = true, HelpText = "The imapserver to sync from. Use semicolon for portnumber.")]
        public string ImapServer { get; set; }

        [Option('p', "password", Required = false, HelpText = "The password to sign into the imap server.")]
        public string Password { get; set; }

        [Option('t', "target", Required = true, HelpText = "The targetfolder to synchronize to.")]
        public string Target { get; set; }

        [Option('d', "delete", Required = false, DefaultValue = false, HelpText = "Delete messages that are deleted from the server.")]
        public bool Delete { get; set; }

        [Option('u', "username", Required = false, HelpText = "The username to sign into the imap server.")]
        public string UserName { get; set; }

        [HelpOption('?', "help")]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        private Options()
        {
        }

        public static Options Parse(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
                return null;

            Console.WriteLine(HeadingInfo.Default);
            Console.WriteLine(CopyrightInfo.Default);

            return options;
        }
    }
}