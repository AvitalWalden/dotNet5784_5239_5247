using BO;
using PL.Engineer;
using PL.Task;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnInitDB_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reboot?", "boot confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DalTest.Initialization.Do();
            }
        }

        private void BtnEngineers_Click(object sender, RoutedEventArgs e)
        {
            string engineerIdInput = Microsoft.VisualBasic.Interaction.InputBox("Enter an ID number", "engineer");
            if (!string.IsNullOrEmpty(engineerIdInput))
            {
                try
                {
                    new EngineerWindow(int.Parse(engineerIdInput)).Show();
                }
                catch (BlDoesNotExistException ex)
                {
                    MessageBox.Show(ex.Message, "error in search engineer", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
        }
    }
}
