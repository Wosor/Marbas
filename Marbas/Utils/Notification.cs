using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notifications.Wpf;

namespace Marbas.Utils
{
    public static class Notification
    {
        private static readonly NotificationManager NotificationManager = new NotificationManager();

        public static void NotificationControl(string title, string message, NotificationType type = NotificationType.Information, TimeSpan? expireTime = null)
        {
            NotificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = type
            }, expirationTime: expireTime);
        }
    }
}
