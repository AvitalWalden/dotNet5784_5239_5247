using PL.Task;
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

namespace PL.Milestone
{
    /// <summary>
    /// Interaction logic for MilestoneListWindow.xaml
    /// </summary>
    public partial class MilestoneListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public MilestoneListWindow()
        {
            InitializeComponent();
            //s_bl.Milestone.Create();
            var temp = s_bl?.Milestone.ReadAll();
            MilestoneList = temp == null ? new() : new(temp!);
        }


        public ObservableCollection<BO.Task> MilestoneList
        {
            get { return (ObservableCollection<BO.Task>)GetValue(MilestoneListProperty); }
            set { SetValue(MilestoneListProperty, value); }
        }

        public static readonly DependencyProperty MilestoneListProperty =
            DependencyProperty.Register("MilestoneList", typeof(ObservableCollection<BO.Task>), typeof(MilestoneListWindow), new PropertyMetadata(null));

        public BO.Status StatusOfTask { get; set; } = BO.Status.Unscheduled;

        private void ComboBox_SelectionExperience(object sender, SelectionChangedEventArgs e)
        {
            var temp = StatusOfTask == BO.Status.Unscheduled ?
                                       s_bl?.Milestone.ReadAll() :
                                        s_bl?.Milestone.ReadAll(item => item.Status == StatusOfTask);
            MilestoneList = temp == null ? new() : new(temp!);
        }

        /// <summary>
        /// Add task to the DB
        /// </summary>
        //private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        //{
        //    new TaskWindow().ShowDialog();
        //}

        /// <summary>
        /// refresh the list view
        /// </summary>
        private void MilestoneListWindow_Activated(object sender, EventArgs e)
        {
            var temp = s_bl?.Milestone.ReadAll();
            MilestoneList = temp == null ? new() : new(temp!);
        }

        /// <summary>
        /// update a task
        /// </summary>
        //private void ListView_taskToUpdate(object sender, MouseButtonEventArgs e)
        //{
        //    BO.Task? taskInList = (sender as ListView)?.SelectedItem as BO.Task;
        //    if (taskInList != null)
        //        new TaskWindow(taskInList.Id).ShowDialog();
        //}
    }
}
