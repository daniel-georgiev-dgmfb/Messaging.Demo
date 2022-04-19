using System;
using Kernel.Notification;
using Kernel.Notification.Notification;

namespace SmtpNotification
{
    public class SmtpNotifier : INotifier<SmtpSender, NotificationMessage>
    {
        SmtpSender _sender;

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public SmtpSender Sender
        {
            get { return _sender; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpNotifier"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public SmtpNotifier(SmtpSender sender)
        {
            _sender = sender;
        }

        public void SendNotification(object notificationMessage)
        {
            this.SendNotification((NotificationMessage)notificationMessage);
        }

        /// <summary>
        /// Sends the notification.
        /// </summary>
        /// <param name="notificationMessage">The notification message.</param>
        public void SendNotification(NotificationMessage notificationMessage)
        {
            var message = new SmtpHelper().BuildMailMessage(notificationMessage);

            using (message)
            {
                _sender.Send(message);
            }
        }

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_sender != null)
                    _sender.Dispose();
            }
        }

        #endregion
    }
}