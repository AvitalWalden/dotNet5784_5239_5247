﻿using BlApi;
using BO;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<BO.Task> CurrentTask
        {
            get { return (ObservableCollection<BO.Task>)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(ObservableCollection<BO.Task>), typeof(TaskWindowViewModel), new PropertyMetadata(null));

        public ObservableCollection<BO.Engineer> EngineerList
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(TaskWindowViewModel), new PropertyMetadata(null));
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
                catch (BlAlreadyExistsException ex)
                {
                    MessageBox.Show(ex.Message, "error in create task", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            viewModel.CurrentTask = new ObservableCollection<BO.Task> { task };
            var engineers = s_bl?.Engineer.ReadAll();
            viewModel.EngineerList = engineers == null ? new ObservableCollection<BO.Engineer>() : new ObservableCollection<BO.Engineer>(engineers!);
        }

        private void ButtonAddOrUpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAction == ActionType.Create)
            {
                BO.Task task = viewModel.CurrentTask[0];
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
                catch (BlInvalidValue ex)
                {
                    MessageBox.Show(ex.Message, "error in create task", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlAlreadyExistsException ex)
                {
                    MessageBox.Show(ex.Message, "error in create task", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                BO.Task task = viewModel.CurrentTask[0];
                try
                {
                    s_bl.Task.Update(task);
                    MessageBox.Show("task updated successfully", "update task", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();

                }
                catch (BlInvalidValue ex)
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

        private void Btn_addDependencyClick(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }
    }
}