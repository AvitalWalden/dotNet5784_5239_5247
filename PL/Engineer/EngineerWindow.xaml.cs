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
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

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
            if (Id != 0)
            {
                try
                {
                    if (s_bl.Engineer.Read(Id) == null)
                    {
                        throw new BO.BlAlreadyExistsException("This engineer does not exit");
                    }
                    else
                        engineer = s_bl.Engineer.Read(Id)!;

                }
                catch (BO.BlAlreadyExistsException ex)
                {
                    MessageBox.Show(ex.ToString());
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

    }
}
