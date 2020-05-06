using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Marbas.Controls.Alarms;
using Marbas.DataCore.DataControls.TaskControl;
using Marbas.DataCore.DataTypes;
using Marbas.Utils;
using Notifications.Wpf;
using PropertyChanged;

namespace Marbas.DataCore
{
    public class CoreModel
    {
        public static CoreModel Instance = new CoreModel();

        public TimeSpan CheckSpan = TimeSpan.FromMinutes(1);
        public ObservableCollection<Tasks> AllTasks { get; set; } = new ObservableCollection<Tasks>();
        public ObservableCollection<Container<string>> TimeHours { get; set; } = new ObservableCollection<Container<string>>();
        public ObservableCollection<Container<string>> TimeMins { get; set; } = new ObservableCollection<Container<string>>();

        public Timer Timer;

        public void CheckAlarms()
        {
            AlarmCore.CheckTime();

            Timer.Start();
        }

        public void CheckDates()
        {
            AlarmCore.CheckDate();
        }

        public void AddNewTask()
        {
            AllTasks.Add(TaskCore.CreateTask());
        }

        public void DeleteTask(Guid g)
        {
            TaskCore.RemoveTask(g);
        }

        internal void CompleteTask(Guid g)
        {
            TaskCore.Complete(g);
        }

        public void EditTask(Guid g)
        {
            TaskCore.Edit(g);
        }
        public void AddAlarm(Guid g, string time)
        {
            TaskCore.AddAlarm(g, time);
        }

        public void ChangeEndDate(Guid g, DateTime date)
        {
            TaskCore.ChangeEndDate(g, date);
        }

        public void LoadTasks(string path)
        {
            TaskCore.TaskStartup(Tasks.GetFromFile(path));
        }

        public void SaveTasks(string path)
        {
            Tasks.SaveToFile(path, AllTasks);
        }

        public void SendNotification(string title, string message, NotificationType type)
        {
            Notification.NotificationControl(title, message, type);
        }

        public void SetTime()
        {
            SettingTime.SetHours();
            SettingTime.SetMins();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class Container<T>
    {
        public T Value { get; set; }

        public Container(T value) => Value = value;
    }
}
