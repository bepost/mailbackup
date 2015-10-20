using System.Reflection;
using System.Runtime.InteropServices;
using CommandLine;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("MailBackup")]
[assembly: AssemblyDescription("A tool to sync an imap-compatible mailbox to local files, in order to include them in a backup set.")]
#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
[assembly: AssemblyCompany("Bepost")]
[assembly: AssemblyProduct("MailBackup")]
[assembly: AssemblyCopyright("Copyright © Bepost 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.0.*")]

