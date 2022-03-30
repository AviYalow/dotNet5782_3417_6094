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

using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
       internal readonly IBL bL= BlFactory.GetBl();

     internal  static EnterMode enter;
        internal static uint clientId;
        public MainWindow()
        {
            
            InitializeComponent();
            new SelctedModeWindow(bL).ShowDialog();
            switch (enter)
            {
                case EnterMode.Meneger:

                    break;
                case EnterMode.Client:
                    new ClientView(bL, clientId, true).Show();
                    this.Closing += MainWindow_Closing;
                    this.Close();
                    break;
                case EnterMode.AddClient:
                    new ClientView(bL, true).Show();
                    this.Closing += MainWindow_Closing;
                    this.Close();
                    break;
                case EnterMode.Close:
                    this.Closing += MainWindow_Closing;
                    this.Close();
                    break;
                default:
                    break;
            }
        }

        private void DroneMainButton_Click(object sender, RoutedEventArgs e)
        {
            
            new DronesListWindow( bL).Show();
            
        }

        private void BaseStationsButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationsList(bL).Show();
        }

        private void PackagesButton_Click(object sender, RoutedEventArgs e)
        {
            new PackagesList(bL).Show();
        }

        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            new ClientsLIst(bL).Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Closing += MainWindow_Closing;
            this.Close();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
