using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace MailBackup
{
    public sealed class MailMessageWriter : IDisposable
    {
        private DirectoryInfo _tempDirectory;
        private SmtpClient _tempSmtpClient;

        internal DirectoryInfo TempDirectory
        {
            get
            {
                if (this._tempDirectory == null)
                {
                    this._tempDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), $"MailBackup Temp {Guid.NewGuid():N}"));
                    this._tempDirectory.Create();
                }
                return this._tempDirectory;
            }
        }

        private SmtpClient TempSmtpClient
            =>
                this._tempSmtpClient
                ?? (this._tempSmtpClient =
                    new SmtpClient("FilePickup") {DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory, PickupDirectoryLocation = this.TempDirectory.FullName});

        public void Dispose()
        {
            if (this._tempDirectory != null)
            {
                try
                {
                    this._tempDirectory.Delete(true);
                }
                catch
                {
                    Log.Error($"Could not delete temporary folder {this._tempDirectory.FullName}.");
                }
            }
        }

        public void WriteEml(MailMessage message, string targetPath)
        {
            this.TempSmtpClient.Send(message);
            var createdFile = this.TempDirectory.GetFiles("*.eml").Single();
            createdFile.CopyTo(targetPath);
            createdFile.Delete();
        }
    }
}