using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using Kernel.Notification.Notification;

namespace SmtpNotification
{
    public class SmtpHelper //: ISmtpHelper
    {
        public MailMessage BuildMailMessage(NotificationMessage notificationMessage)
        {
            var emailMessage = notificationMessage.EmailMessage;

            var message = new MailMessage
            {
                Body = emailMessage.Message,
                BodyEncoding = Encoding.UTF8,
                Subject = emailMessage.Subject,
                From = new MailAddress(emailMessage.From),
                IsBodyHtml = emailMessage.MessageIsHtml
            };

            PopulateRecipients(emailMessage.To, message.To);

            PopulateRecipients(emailMessage.Cc, message.CC);

            PopulateRecipients(emailMessage.Bcc, message.Bcc);

            if (emailMessage.Attachments != null)
            {
                foreach (var attachment in emailMessage.Attachments)
                {
                    var stream = new MemoryStream(attachment.Attachment, false);

                    var att = new Attachment(stream, attachment.AttachmentName, attachment.MimeType);

                    message.Attachments.Add(att);
                }
            }

            return message;
        }

        private static void PopulateRecipients(IEnumerable<string> source, ICollection<MailAddress> target)
        {
            if (source == null)
                return;

            foreach (var address in source)
            {
                target.Add(new MailAddress(address));
            }
        }
    }
}