using Marbas.DataCore;
using Notifications.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Marbas.Windows
{
    /// <summary>
    /// Interaction logic for AlarmWindow.xaml
    /// </summary>
    public partial class AlarmWindow : Window
    {
        public CoreModel Core { get; } = CoreModel.Instance;
        public Guid TaskId;
        public AlarmWindow()
        {
            InitializeComponent();
            Loaded += WindowStartupLocation;
            DataContext = this;
        }

        private void WindowStartupLocation(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - (Width + (Width/2));
            Top = Height * 3;
            CheckTask();
        }

        private void CheckTask()
        {
            var foundTask = Core.AllTasks.FirstOrDefault(x => x.Id == TaskId);
            if (foundTask == null)
            {
                Close();
                return;
            }
            if(foundTask.Alarm == null) return;
            Btn_setChange.Content = "Change";
            SetTime(foundTask.Alarm);
        }

        private void SetTime(string foundTaskAlarm)
        {
            var time = foundTaskAlarm.Split(':');

            Cbx_hours.Text = time[0];
            Cbx_mins.Text = time[1];
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var hour = Cbx_hours.Text;
            var mins = Cbx_mins.Text;

            var time = $"{hour}:{mins}";

            Core.AddAlarm(TaskId, time);
            Core.SendNotification("Message", $"Alarm for {time} has been set.", NotificationType.Success);
            Close();
        }

        private void AlarmWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Tag = "Closed";
        }
    }
}
