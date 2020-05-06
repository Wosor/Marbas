using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marbas.DataCore;
using Marbas.DataCore.DataTypes;
using Notifications.Wpf;

namespace Marbas.Controls.Alarms
{
    public class AlarmCore
    {
        public static CoreModel Core { get; } = CoreModel.Instance;
        public static void CheckTime()
        {
            CheckAlarms.CheckingAlarms();
        }

        public static void CheckDate()
        {
            CheckAlarms.CheckingEnd();
        }

        public static void Send(Tasks tasks, int type)
        {

            if (type == 0)
            {
                Core.SendNotification("Alarm", $"Alarm for {tasks.Text} .", NotificationType.Error);
            }
            if (type == 1)
            {
                Core.SendNotification("Ending", $"{tasks.Text} Is Ending to day.", NotificationType.Error);
            }
        }
    }
}
