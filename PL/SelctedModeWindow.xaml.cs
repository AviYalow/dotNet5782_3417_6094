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
using BlApi;
using BO;

namespace PL
{
  public  enum EnterMode { Meneger,Client,AddClient,Close};
    /// <summary>
    /// Interaction logic for SelctedModeWindow.xaml
    /// </summary>
    public partial class SelctedModeWindow : Window
    {
      
        IBL bl;
        public SelctedModeWindow(IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            ChoseClientCmb.ItemsSource = bl.ClientById("");
        }

        private void AddNewClient_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.enter = EnterMode.AddClient;
            this.Closing += SelctedModeWindow_Closing;
            this.Close();
        }

        private void MengerButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.enter = EnterMode.Meneger;
            this.Closing += SelctedModeWindow_Closing;
            this.Close();
        }

        private void ClientByNumberMode_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.enter = EnterMode.Client;
            uint.TryParse(ChoseClientCmb.Text, out MainWindow.clientId);
            this.Closing += SelctedModeWindow_Closing;
            this.Close();
        }

        private void SelctedModeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void ChoseClientCmb_TextInput(object sender, TextCompositionEventArgs e)
        {
            ChoseClientCmb.ItemsSource = bl.ClientById(ChoseClientCmb.Text);
        }

        private void ChoseClientCmb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home || e.Key == Key.End ||
                e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.NumPad0)
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            EnterAsClientGrid.Visibility = Visibility.Visible;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.enter = EnterMode.Close;
            this.Closing += SelctedModeWindow_Closing;
            this.Close();
        }
    }
}
