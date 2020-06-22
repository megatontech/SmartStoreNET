using SmartStore.Core.Domain;
using SmartStore.Core.Domain.Media;
using SmartStore.Core.Domain.Messages;
using SmartStore.Core.Email;
using SmartStore.Core.IO;
using SmartStore.Core.Logging;
using SmartStore.Services.Messages;
using System;
using System.IO;
using System.Linq;

namespace SmartStore.Services.DataExchange.Export.Deployment
{
    public class EmailFilePublisher : IFilePublisher
    {
        #region Private Fields

        private IEmailAccountService _emailAccountService;

        private IQueuedEmailService _queuedEmailService;

        #endregion Private Fields

        #region Public Constructors

        public EmailFilePublisher(
            IEmailAccountService emailAccountService,
            IQueuedEmailService queuedEmailService)
        {
            _emailAccountService = emailAccountService;
            _queuedEmailService = queuedEmailService;
        }

        #endregion Public Constructors



        #region Public Methods

        public virtual void Publish(ExportDeploymentContext context, ExportDeployment deployment)
        {
            var emailAccount = _emailAccountService.GetEmailAccountById(deployment.EmailAccountId);
            var smtpContext = new SmtpContext(emailAccount);
            var count = 0;

            foreach (var email in deployment.EmailAddresses.SplitSafe(",").Where(x => x.IsEmail()))
            {
                var queuedEmail = new QueuedEmail
                {
                    From = emailAccount.ToEmailAddress(),
                    SendManually = false,
                    To = email,
                    Subject = deployment.EmailSubject.NaIfEmpty(),
                    Body = deployment.EmailSubject.NaIfEmpty(),
                    CreatedOnUtc = DateTime.Now,
                    EmailAccountId = deployment.EmailAccountId
                };

                foreach (var path in context.GetDeploymentFiles())
                {
                    var name = Path.GetFileName(path);

                    queuedEmail.Attachments.Add(new QueuedEmailAttachment
                    {
                        StorageLocation = EmailAttachmentStorageLocation.Blob,
                        MediaStorage = new MediaStorage { Data = File.ReadAllBytes(path) },
                        Name = name,
                        MimeType = MimeTypes.MapNameToMimeType(name)
                    });
                }

                _queuedEmailService.InsertQueuedEmail(queuedEmail);
                ++count;
            }

            context.Log.Info("{0} email(s) created and queued for deployment.".FormatInvariant(count));
        }

        #endregion Public Methods
    }
}