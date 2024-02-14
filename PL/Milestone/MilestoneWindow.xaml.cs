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

namespace PL.Milestone
{
    /// <summary>
    /// Interaction logic for MilestoneWindow.xaml
    /// </summary>
    public partial class MilestoneWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public MilestoneWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string projectStartDate = Microsoft.VisualBasic.Interaction.InputBox("Enter the project start date", "engineer");
            string projectEndDate = Microsoft.VisualBasic.Interaction.InputBox("Enter the project end date", "engineer");
            if (!string.IsNullOrEmpty(projectStartDate.ToString()) && !string.IsNullOrEmpty(projectEndDate.ToString()))
            {
                try
                {
                    BO.Tools.SetProjectDates(DateTime.Parse(projectStartDate), DateTime.Parse(projectEndDate));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
           
        }

        private void BtnMilsoneList_Click(object sender, RoutedEventArgs e)
        {
            new MilestoneListWindow().Show();
        }

        private void BtnCreateMilestone_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to create milestone?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                s_bl.Milestone.Create();
            }
        }
    }
}
