# MailBackup
A tool to sync an imap-compatible mailbox to local files, in order to include them in a backup set.

## Why synchronize?
Cloud computing is good. It's great in many ways. However, not having a physical hold of your data
is a major drawback.  Users are supposed to trust the system works, and no data will be lost. Ever.

This is an assumption I an not willing to make. Cloud providers have actual humans working there.
That implies there is room for human error. What if the person in charge of backup operations at 
icloud.com, Outlook.com or gmail.com screws up. Murphy's Law ensures that systems will fail the moment
backups become unusable.

For file shares there are many solutions available. Synchronize your cloud data to a physical computer
and enable shadow copies on the share adds a single layor of safety. (Shadow copies are required, because
if the data disappears in the cloud, odds are the synchronization will remove the files offline too.)

For an extra layer of safety you can use a conventional backup system. For instance using backup disks,
use backup software to do incremental backups or even use a service like CrashPlan to replicate your
backup offline.

## What about email?
But for could hosted email this isn't as easy. Many email clients do not actually download all your email, 
but only fetch the message when the user opens the email. And often only partially.
If you have an email client that does download your mail, it's not guaranteed the files produced deep down 
inside will be useful to recover from in case of emergency.

At this point MailBackup comes to the rescue. Using MailBackup, you can fetch all email messages from 
a cloud service -- or any imap-service for that matter -- and synchronize the messages to physical files
on your harddrive. There will be a single file per email message, to enable good incremental backups and
the files will be in the very open .eml format.

## Usage
For simplicity, the tool synchronizes one imap account to a folder once each time ran. To run the tool
on a frequent basis, nothing beats the built in Windows Task Scheduler.

    -i, --imap        Required. The imapserver to sync from.    
    --ssl             Flag SSL should be used.
    --port            The port of the imapserver. Default: 143, or 993 when SSL is chosen.
    
    -u, --username    Required. The username to sign into the imap server.
    -p, --password    The password to sign into the imap server.
    -m, --method      The authentication method to use. 
                      [ Login (default), Plain, CramMd5, DigestMd5, OAuth, OAuth2, 
                        Ntlm, Ntlmv2, NtlmOverSspi, Gssapi, ScramSha1, Srp ]
        
    -t, --target      Required. The targetfolder to synchronize to.    
    -d, --delete      Delete messages that are deleted from the server. (Default: False) 
       
    -v, --verbose     Verbose output.    
    -?, --help        Display this help screen.

Example:

    MailBackup.exe -i imap.domain.com -u myaccount -p NotTellingYou -t C:\Backup\myaccount@domain.com --delete
