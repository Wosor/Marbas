using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marbas.DataCore;

namespace Marbas.Controls.Alarms
{
    public class CheckAlarms
    {
        public static CoreModel Core { get; } = CoreModel.Instance;
        public static void CheckingAlarms()
        {
            foreach (var tasks in Core.AllTasks)
            {
                var now = $"{DateTime.Now.Hour:00}:{DateTime.Now.Minute:00}";

                if(now != tasks.Alarm) continue;
                AlarmCore.Send(tasks, 0);
            }
        }

        public static void CheckingEnd()
        {
            foreach (var tasks in Core.AllTasks)
            {
                var now = DateTime.Now;

                int i = DateTime.Compare(now.Date, tasks.DateEnding.Date);

                if (i != 0) continue;
                AlarmCore.Send(tasks, 1);
            }
        }
    }
}
