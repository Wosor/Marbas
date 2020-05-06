using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marbas.DataCore.DataTypes;

namespace Marbas.DataCore.DataControls.TaskControl
{
    public class TaskCore
    {
        public static CoreModel Core { get; } = CoreModel.Instance;
        public static Tasks CreateTask()
        {
            return new Tasks
            {
                Id = Guid.NewGuid(),
                Display = true,
                Editing = false,
                Completed = false,
                DateAdded = DateTime.Now,
                DateEnding = DateTime.Now
            };
        }

        public static Tasks FindTask(Guid g)
        {
            var foundTask = Core.AllTasks.FirstOrDefault(x => x.Id == g);
            return foundTask;
        }

        public static void RemoveTask(Guid g)
        {
            var foundTask = FindTask(g);

            Core.AllTasks.Remove(foundTask);
        }

        internal static void Complete(Guid g)
        {
            var foundTask = FindTask(g);
            if (foundTask.Completed == false)
            {
                foundTask.Completed = true;
                return;
            }

            foundTask.Completed = false;
            foundTask.Strike = "";

        }

        public static void Edit(Guid g)
        {
            var foundTask = FindTask(g);

            if (foundTask.Editing == false)
            {
                foundTask.Editing = true;
                foundTask.Display = false;
            }
            else
            {
                foundTask.Editing = false;
                foundTask.Display = true;
            }
        }
        public static void AddAlarm(Guid g, string time)
        {
            var foundTask = FindTask(g);

            foundTask.Alarm = time;
        }

        public static void ChangeEndDate(Guid g, DateTime date)
        {
            var foundTask = FindTask(g);

            foundTask.DateEnding = date;
        }

        public static void TaskStartup(ObservableCollection<Tasks> tasks)
        {
            foreach (var task in tasks)
            {
                Core.AllTasks.Add(task);
            }
        }


    }
}
