using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using Marbas.DataCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Marbas.DataCore.DataTypes;
using Marbas.Windows;
using Microsoft.Xaml.Behaviors.Core;

namespace Marbas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CoreModel Core { get; } = CoreModel.Instance;

        public ICommand CloseCommand { get; set; }

        public AlarmWindow AlarmWindow;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += WindowStartupLocation;
            DataContext = this;
            CloseCommand = new ActionCommand(() => {Application.Current.Shutdown();});
        }

        private void WindowStartupLocation(object sender, RoutedEventArgs e)
        {
            StateChanged += (o, args) =>
            {
                if (WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Normal;
                    Activate();
                    Focus();
                }
            };
            
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = Height / 2;
            Core.LoadTasks(Tasks.DataFile);
            Core.CheckDates();
            Core.Timer = new Timer
            {
                AutoReset = false,
                Interval = Core.CheckSpan.TotalMilliseconds
            };
            Core.Timer.Elapsed += (o, args) => Core.CheckAlarms();
            Core.Timer.Start();
            Core.SetTime();
        }


        private void Btn_AddTask_OnClick(object sender, RoutedEventArgs e)
        {
            Core.AddNewTask();
            Core.SaveTasks(Tasks.DataFile);
        }

        private void Btn_Remove_OnClick(object sender, RoutedEventArgs e)
        {
            if(!(sender is Button b)) return;
            if(!(b.Tag is Guid g)) return;
            if(MessageBox.Show("Are you sure you wish to delete this task?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            Core.DeleteTask(g);
            Core.SaveTasks(Tasks.DataFile);
        }

        private void Chb_isComplete_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!(sender is CheckBox b)) return;
            if (!(b.Tag is Guid g)) return;
            Core.CompleteTask(g);
            Core.SaveTasks(Tasks.DataFile);
        }


        private void Btn_Edit_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button b)) return;
            if (!(b.Tag is Guid g)) return;
            Core.EditTask(g);
            Core.SaveTasks(Tasks.DataFile);
        }

        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is DatePicker b)) return;
            if (!(b.Tag is Guid g)) return;
            if (b.SelectedDate == null) return;
            Core.ChangeEndDate(g, (DateTime)b.SelectedDate);
            Core.SaveTasks(Tasks.DataFile);
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button b)) return;
            if (!(b.Tag is Guid g)) return;

            if (AlarmWindow != null && !AlarmWindow.Tag.ToString().Equals("Closed"))
            {
                AlarmWindow.Topmost = true;
                AlarmWindow.Topmost = false;
                AlarmWindow.Focus();
            }
            else
            {
                AlarmWindow = new AlarmWindow();
                AlarmWindow.TaskId = g;
                AlarmWindow.Show();
            }
        }
    }
}
