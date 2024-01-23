using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public TaskListWindow()
        {
            InitializeComponent();
            var temp = s_bl?.Task.ReadAll();
            TaskList = temp == null ? new() : new(temp!);
        }

        public ObservableCollection<BO.Task> TaskList
        {
            get { return (ObservableCollection<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.Task>), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.Status StatusOfTask { get; set; } = BO.Status.Unscheduled;

        private void ComboBox_SelectionExperience(object sender, SelectionChangedEventArgs e)
        {
            var temp = StatusOfTask == BO.Status.Unscheduled ?
                                        s_bl?.Task.ReadAll() :
                                        s_bl?.Task.ReadAll(item => item.Status == StatusOfTask);
            TaskList = temp == null ? new() : new(temp!);
        }

        /// <summary>
        /// Add task to the DB
        /// </summary>
        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
        }

        /// <summary>
        /// refresh the list view
        /// </summary>
        private void TaskListWindow_Activated(object sender, EventArgs e)
        {
            var temp = s_bl?.Task.ReadAll();
            TaskList = (temp == null) ? new() : new(temp!);
            var updatedTask = s_bl?.Task.ReadAll();
            TaskList = updatedTask == null ? new() : new(updatedTask!);
        }

        /// <summary>
        /// update a task
        /// </summary>
        private void ListView_taskToUpdate(object sender, MouseButtonEventArgs e)
        {
            BO.Task? taskInList = (sender as ListView)?.SelectedItem as BO.Task;
            if (taskInList != null)
                new TaskWindow().ShowDialog();

            //new TaskWindow(taskInList.Id).ShowDialog();
        }
    }
}
