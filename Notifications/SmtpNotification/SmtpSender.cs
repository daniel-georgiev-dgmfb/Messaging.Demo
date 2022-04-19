using System;
using System.Net.Mail;
using Kernel.Configuration;
using Kernel.Notification;

namespace SmtpNotification
{
    public class SmtpSender : SmtpClient, INotificationSender<MailMessage>
    {
        IDestinationTargetProvider _destinationTargetProvider;

        public SmtpSender(ISmptServer destinationTargetProvider)
        {
            _destinationTargetProvider = destinationTargetProvider;

            var spBase = _destinationTargetProvider as SettingsProviderBase;

            if (spBase == null)
                throw new InvalidCastException("SettingsProviderBase");

            if (spBase.HasError)
                throw spBase.Error;

            Init();
        }

        public IDestinationTargetProvider DestinationTarget
        {
            get
            {
                return _destinationTargetProvider;
            }
            set
            {
                _destinationTargetProvider = value;
            }
        }

        public void Init()
        {
            Host = _destinationTargetProvider.Path;

            if (_destinationTargetProvider.Port > 0)
                Port = _destinationTargetProvider.Port;

            Credentials = _destinationTargetProvider.NetworkCredential;

            EnableSsl = _destinationTargetProvider.SSL;
        }

        public new void Send(MailMessage notification)
        {
            base.Send(notification);
        }
    }
}