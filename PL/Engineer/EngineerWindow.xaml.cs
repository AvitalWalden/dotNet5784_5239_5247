using BO;
using DO;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public enum ActionType
    {
        Create,
        Update
    }
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public ActionType CurrentAction { get; private set; }
        public EngineerWindow(int Id = 0)
        {
            InitializeComponent();
            BO.Engineer engineer = new BO.Engineer()
            {
                Id = 0,
                Name = "",
                Email = "",
                Level = BO.EngineerExperience.None
            };
            CurrentAction = ActionType.Create;
            if (Id != 0)
            {
                try
                {
                    CurrentAction = ActionType.Update;
                    if (s_bl.Engineer.Read(Id) == null)
                    {
                        throw new BO.BlAlreadyExistsException("This engineer does not exit");
                    }
                    else
                        engineer = s_bl.Engineer.Read(Id)!;

                }
                catch (BlAlreadyExistsException ex)
                {
                    MessageBox.Show(ex.Message, "error in create engineer", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            CurrentEngineer = new ObservableCollection<BO.Engineer> { engineer };
        }

        public ObservableCollection<BO.Engineer> CurrentEngineer
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(ObservableCollection<BO.Engineer>), typeof(EngineerWindow), new PropertyMetadata(null));



        /// <summary>
        /// add or update engineer
        /// </summary>
        private void ButtonAddOrUpdateEngineer_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAction == ActionType.Create)
            {
                BO.Engineer engineer = CurrentEngineer[0];
                try
                {
                    s_bl.Engineer.Create(engineer);
                    MessageBox.Show("engineer created successfully", "create engineer", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();

                }
                catch (BlInvalidValue ex)
                {
                    MessageBox.Show(ex.Message, "error in create engineer", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch(BlAlreadyExistsException ex)
                {
                    MessageBox.Show(ex.Message, "error in create engineer", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                BO.Engineer engineer = CurrentEngineer[0];
                try
                {
                    s_bl.Engineer.Update(engineer);
                    MessageBox.Show("engineer updated successfully","update engineer", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();

                }
                catch (BlInvalidValue ex)
                {
                    MessageBox.Show(ex.Message, "error in update engineer", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void BtnCurrentTask_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentEngineer[0].Task != null)
            {
                new TaskWindow(CurrentEngineer[0].Task!.Id).Show();
            }
        }

        private void BtnTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }
    }
}
