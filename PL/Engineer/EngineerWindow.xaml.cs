using BO;
using DO;
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

        private void ButtonAddOrUpdateEngineer_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAction == ActionType.Create)
            {
                BO.Engineer engineer = CurrentEngineer[0];
                try
                {
                    s_bl.Engineer.Create(engineer);
                    MessageBox.Show("engineer created successfully");
                    this.Close();

                }
                catch (BlInvalidValue ex)
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
                    MessageBox.Show("engineer updated successfully");
                    this.Close();

                }
                catch (BlInvalidValue ex)
                {
                    MessageBox.Show(ex.Message, "error in update engineer", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
