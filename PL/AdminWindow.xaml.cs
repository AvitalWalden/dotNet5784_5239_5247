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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void BtnTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }

        private void BtnEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void BtnMilestone_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
