using BlApi;
using BO;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public enum ActionType
    {
        Create,
        Update
    }

    public class TaskWindowViewModel : DependencyObject
    {
        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindowViewModel), new PropertyMetadata(null));

        public ObservableCollection<BO.Engineer> EngineerList
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(TaskWindowViewModel), new PropertyMetadata(null));


        public ObservableCollection<BO.TaskInList> Dependencies
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(DependenciesProperty); }
            set { SetValue(DependenciesProperty, value); }
        }

        public static readonly DependencyProperty DependenciesProperty =
            DependencyProperty.Register("Dependencies", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskWindowViewModel), new PropertyMetadata(null));

        public ObservableCollection<BO.TaskInList> TasksList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TasksListProperty); }
            set { SetValue(TasksListProperty, value); }
        }

        public static readonly DependencyProperty TasksListProperty =
            DependencyProperty.Register("TasksList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));

    }

    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public ActionType CurrentAction { get; private set; }

        private TaskWindowViewModel viewModel;

        public TaskWindow(int Id = -1)
        {
            InitializeComponent();

            // Initialize ViewModel
            viewModel = new TaskWindowViewModel();

            // Set DataContext
            DataContext = viewModel;

            BO.Task task = new BO.Task()
            {
                Id = 0,
                Description = "",
                Alias = "",
                CreatedAtDate = DateTime.Now,
                Status = BO.Status.Unscheduled
            };
            CurrentAction = ActionType.Create;
            if (Id != -1)
            {
                try
                {
                    CurrentAction = ActionType.Update;
                    if (s_bl?.Task.Read(Id) == null)
                    {
                        throw new BO.BlAlreadyExistsException("This task does not exit");
                    }
                    else
                    {
                        task = s_bl.Task.Read(Id)!;
                    }

                }
                catch (BO.BlAlreadyExistsException ex)
                {
                    MessageBox.Show(ex.Message, "error in create task", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            viewModel.CurrentTask = new BO.Task() {
                Id = task.Id,
                Alias = task.Alias,
                Description = task.Description,
                Engineer = task.Engineer,
                CompleteDate = task.CompleteDate,
                ComplexityLevel = task.ComplexityLevel,
                CreatedAtDate = task.CreatedAtDate,
                Status = task.Status,
                DeadlineDate = task.DeadlineDate,
                Deliverables = task.Deliverables,
                Dependencies = task.Dependencies,
                Milestone = task.Milestone,
                Remarks = task.Remarks,
                RequiredEffortTime = task.RequiredEffortTime,
                ScheduledStartDate  = task.ScheduledStartDate,
                StartDate = task.StartDate
            };
            var engineers = s_bl?.Engineer.ReadAll();
            viewModel.EngineerList = engineers == null ? new ObservableCollection<BO.Engineer>() : new ObservableCollection<BO.Engineer>(engineers!);
            var dependendies = s_bl?.Task.ReadAll().Select(task =>
            {
                if (task == null) { return null; }
                return new BO.TaskInList()
                {
                    Id = task.Id,
                    Alias = task.Alias,
                    Description = task.Description,
                    Status = task.Status
                };
            }).Where(t => t != null).ToList();
            viewModel.Dependencies = dependendies == null ? new ObservableCollection<BO.TaskInList>() : new ObservableCollection<BO.TaskInList>(dependendies!);
            viewModel.TasksList = viewModel.CurrentTask.Dependencies != null ? new ObservableCollection<BO.TaskInList>(viewModel.CurrentTask.Dependencies) : new ObservableCollection<BO.TaskInList>();
        }

        private void ButtonAddOrUpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAction == ActionType.Create)
            {
                BO.Task task = viewModel.CurrentTask;
                try
                {
                    s_bl.Task.Create(task);
                    MessageBox.Show("task created successfully", "create task", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();

                }
                catch (BO.BlEngineerIsAlreadyBusy ex)
                {
                    MessageBox.Show(ex.Message, "error in create task", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BO.BlInvalidValue ex)
                {
                    MessageBox.Show(ex.Message, "error in create task", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BO.BlAlreadyExistsException ex)
                {
                    MessageBox.Show(ex.Message, "error in create task", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                BO.Task task = viewModel.CurrentTask;
                try
                {
                    s_bl.Task.Update(task);
                    MessageBox.Show("task updated successfully", "update task", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();

                }
                catch (BO.BlEngineerIsAlreadyBusy ex)
                {
                    MessageBox.Show(ex.Message, "error in create task", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BO.BlAlreadyExistsException ex)
                {
                    MessageBox.Show(ex.Message, "error in create task", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BO.BlInvalidValue ex)
                {
                    MessageBox.Show(ex.Message, "error in update task", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void IdOrCost_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox? text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;
            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;
            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
            e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
            || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;
            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            //allow control system keys
            if (Char.IsControl(c)) return;
            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox
                            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }

        private void ComboBox_AddDependency(object sender, SelectionChangedEventArgs e)
        {
            BO.TaskInList? dependencyTask = (sender as ComboBox)?.SelectedItem as BO.TaskInList;

            MessageBoxResult result = MessageBox.Show("Do you want to add the selected item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (viewModel.CurrentTask.Dependencies == null)
                    viewModel.CurrentTask.Dependencies = new List<TaskInList>();

                if (dependencyTask != null)
                {
                    viewModel.CurrentTask.Dependencies.Add(dependencyTask!);
                    viewModel.TasksList.Add(dependencyTask!);
                }
            }
        }

        private void ListViewDeleteDependency_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete the selected item?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    viewModel.CurrentTask.Dependencies!.Remove((BO.TaskInList)listBox.SelectedItem);
                    viewModel.TasksList.Remove((BO.TaskInList)listBox.SelectedItem);
                }
                listBox.SelectedItem = null;
            }
        }
    }
}