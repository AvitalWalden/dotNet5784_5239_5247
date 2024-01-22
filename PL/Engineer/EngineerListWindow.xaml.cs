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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        
        public EngineerListWindow()
        {
            InitializeComponent();
            var temp = s_bl?.Engineer.ReadAll();
            EngineerList = temp == null ? new() : new(temp!);
        }

        public ObservableCollection<BO.Engineer> EngineerList
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.None;

        private void ComboBox_SelectionExperience(object sender, SelectionChangedEventArgs e)
        {
            var temp = Experience == BO.EngineerExperience.None ?
                                        s_bl?.Engineer.ReadAll() :
                                        s_bl?.Engineer.ReadAll(item => item.Level == Experience);
            EngineerList = temp == null ? new() : new(temp!);
        }

        /// <summary>
        /// Add engineer to the DB
        /// </summary>
        private void BtnAddEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        /// <summary>
        /// refresh the list view
        /// </summary>
        private void EngineerListWindow_Activated(object sender, EventArgs e)
        {
            var temp = s_bl?.Engineer.ReadAll();
            EngineerList = (temp == null) ? new() : new(temp!);
            var updatedEngineers = s_bl?.Engineer.ReadAll();
            EngineerList = updatedEngineers == null ? new() : new(updatedEngineers!);
        }

        /// <summary>
        /// update a engineer
        /// </summary>
        private void ListView_engineerToUpdate(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? engineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (engineerInList != null)
                new EngineerWindow(engineerInList.Id).ShowDialog();
        }
    }
}