using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using S22.Imap;

namespace MailBackup
{
    public sealed class Options
    {
        [Option('i', "imap", Required = true, HelpText = "The imapserver to sync from.")]
        public string ImapServer { get; set; }

        [Option( "port", Required = false, HelpText = "The port of the imapserver. Default: 143, or 993 when SSL is chosen.")]
        public int ImapPort { get; set; } = 0;

        [Option( "ssl", Required = false, HelpText = "Flag SSL should be used.")]
        public bool ImapSsl { get; set; }= false;

        [Option('p', "password", Required = false, HelpText = "The password to sign into the imap server.")]
        public string Password { get; set; }

        [Option('t', "target", Required = true, HelpText = "The targetfolder to synchronize to.")]
        public string Target { get; set; }

        [Option('m', "method", Required = false, HelpText = "The authentication method to use. [ Login (default), Plain, CramMd5, DigestMd5, OAuth, OAuth2, Ntlm, Ntlmv2, NtlmOverSspi, Gssapi, ScramSha1, Srp ]")]
        public AuthMethod AuthMethod { get; set; } = AuthMethod.Login;

        [Option('d', "delete", Required = false, DefaultValue = false, HelpText = "Delete messages that are deleted from the server.")]
        public bool Delete { get; set; }

        [Option('u', "username", Required = true, HelpText = "The username to sign into the imap server.")]
        public string UserName { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Verbose output.")]
        public bool Verbose { get; set; }

        [HelpOption('?', "help")]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
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