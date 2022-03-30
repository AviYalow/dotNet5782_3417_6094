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
using BO;
using BlApi;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ClientView.xaml
    /// </summary>
    public partial class ClientView : Window

    {
        ClientModel client;
        IBL bl;
        bool clientMode;
        ObservableCollection<ClientToList> list;
        private int _noOfErrorsOnScreen = 0;

        public ClientView(BlApi.IBL bL, bool clientView = false, ObservableCollection<ClientToList> lists = null)
        {
            InitializeComponent();
            bl = bL;
            list = lists;
            client = new();
            client.Location = new();
            DataContext = client;
            clientMode = clientView;

            ListPackegeFromClient.Visibility = Visibility.Collapsed;

        }

        public ClientView(BlApi.IBL bL, ObservableCollection<ClientToList> lists, ClientToList clientFromList, bool clientView = false)
        {
            ctorupdate(bL, lists, clientFromList.ID, clientView);

        }

        private void ctorupdate(IBL bL, ObservableCollection<ClientToList> lists, uint clientFromList, bool clientView)
        {
            InitializeComponent();
            list = lists;
            bl = bL;
            client = new();
            client.Location = new();
            clientMode = clientView;

            this.DataContext = this.client;
            UpdateClient(clientFromList);

        }

        public ClientView(BlApi.IBL bL, uint clientFromList, bool clientView = false, ObservableCollection<ClientToList> lists = null)
        {

            ctorupdate(bL, lists, clientFromList, clientView);
        }
        private void UpdateClient(uint id)
        {
            bl.GetingClient(id).clientFromBl(this.client);


            TitelClientLabel.Content = "Updte Client Window";

            ListPackegeFromClient.Visibility = Visibility.Visible;
            OkButton.Visibility = Visibility.Collapsed;



        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Closing += ClientView_Closing;
            this.Close();
        }

        private void ClientView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddClient(client);
                MessageBox.Show("Add client \n" + client.ToString() + "\nsucceed!");
                list.Add(client);
                UpdateClient(client.Id);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString(), "ERROR"); }
        }



        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to update the client detels?", "Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                try
                {
                    bl.UpdateClient(client);
                    UpdateClient(client.Id);
                    MessageBox.Show("Update seccsed!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR");

                }
            }
        }



        private void HeaderedContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                HeaderedContentControl control = sender as HeaderedContentControl;
                if (control.Name.LastOrDefault() == 'S')
                {
                    control.Name = control.Name.Remove(control.Name.Count() - 1);
                    bl.SortList(control.Name, client.FromClient).ConvertIenmurbleToObserve(client.FromClient);
                }
                if (control.Name.LastOrDefault() == 'P')
                {
                    control.Name = control.Name.Remove(control.Name.Count() - 1);
                    bl.SortList(control.Name, client.ToClient).ConvertIenmurbleToObserve(client.ToClient);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }
        }

        private void ListPackegeFromClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListPackegeFromClient.SelectedItem != null)
            {

                new PackageView(bl, ((PackageAtClient)ListPackegeFromClient.SelectedItem).SerialNum, StatusPackegeWindow.SendClient).ShowDialog();
                bl.GetingClient(client.Id).clientFromBl(this.client);

                ListPackegeFromClient.SelectedItem = null;
                if (list != null)
                    bl.FilterClientList().ConvertIenmurbleToObserve(list);
                this.DataContext = this.client;
            }
        }

        private void ListPackegeToClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListPackegeToClient.SelectedItem != null)
            {

                new PackageView(bl, ((PackageAtClient)ListPackegeToClient.SelectedItem).SerialNum, StatusPackegeWindow.GetingClient, clientMode).ShowDialog();
                ListPackegeToClient.SelectedItem = null;
                bl.GetingClient(client.Id).clientFromBl(this.client);
                this.DataContext = this.client;

                ListPackegeFromClient.SelectedItem = null;
                if (list != null)
                    bl.FilterClientList().ConvertIenmurbleToObserve(list);
            }
        }

        private void AddPAckegeButton_Click(object sender, RoutedEventArgs e)
        {
            new PackageView(bl, client.Id.ToString(), StatusPackegeWindow.SendClient, clientMode).ShowDialog();
            bl.GetingClient(client.Id).clientFromBl(this.client);
            if (list != null)
                bl.FilterClientList().ConvertIenmurbleToObserve(list);


        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this clien?", "Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                try
                {
                    bl.DeleteClient(client.Id);
                    MessageBox.Show($"client {client.Id.ToString()} deleted!");
                    if (list != null)
                        bl.FilterClientList().ConvertIenmurbleToObserve(list);
                    this.Closing += ClientView_Closing;
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR");

                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void idTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (idTextBox.Text == "")
                idTextBox.Text = "0";
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void letitudeTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            ((TextBox)sender).SelectAll();

        }
    }
}
