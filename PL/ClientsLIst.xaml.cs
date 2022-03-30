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
using System.Collections.ObjectModel;
using BlApi;
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for ClientsLIst.xaml
    /// </summary>
    public partial class ClientsLIst : Window
    {
        CollectionView view;
        PropertyGroupDescription groupDescription;
        BlApi.IBL bl;
         ObservableCollection<ClientToList> lists = new ObservableCollection<ClientToList>();
        public ClientsLIst(BlApi.IBL bl)
        {
            InitializeComponent();
            this.bl = bl;
            lists = new(bl.ClientToLists());
            DataContext = lists;
            view = (CollectionView)CollectionViewSource.GetDefaultView(clientListView.ItemsSource);
            groupDescription = new PropertyGroupDescription("Name");
           
        }

        private void HeaderedContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            HeaderedContentControl control = sender as HeaderedContentControl;
            try
            {
                bl.SortList(control.Name, lists).ConvertIenmurbleToObserve(lists);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new ClientView(bl, false, lists).Show();
            }catch(Exception)
            { }
          
        }

        private void clientListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (clientListView.SelectedItem != null)
            {try
                {
                    new ClientView(bl, lists, (BO.ClientToList)clientListView.SelectedItem).Show();
                }catch(Exception)
                { }
              
                clientListView.SelectedItem = null;
            }
        }

      

        private void SenderClientCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if(SenderClientCmb.SelectedItem is null)
            {
                bl.ClientActiveHowSendPackegesToLists(false).ConvertIenmurbleToObserve(lists);
            }
           else if(SenderClientCmb.SelectedItem == SendItem)
            {  bl.ClientActiveHowSendPackegesToLists().ConvertIenmurbleToObserve(lists); }
            else if (SenderClientCmb.SelectedItem == SendItemAndPackegeArrive)
            { bl.ClientActiveHowSendAndArrivePackegesToLists().ConvertIenmurbleToObserve(lists); }
            else if (SenderClientCmb.SelectedItem == SendItemAndPackegeNotArrive)
            { bl.ClientActiveHowSendPackegesAndNotArriveToLists().ConvertIenmurbleToObserve(lists); }
        }

        private void GetingClientCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (getingClientCmb.SelectedItem is null)
            {
                clientListView.ItemsSource = bl.ClientActiveHowGetingPackegesToLists(false);
            }
            else if (getingClientCmb.SelectedItem == GetItem)
            { bl.ClientActiveHowGetingPackegesToLists().ConvertIenmurbleToObserve(lists); }
            else if (getingClientCmb.SelectedItem == GetItemAndPackegeArrive)
            { bl.ClientActiveHowGetingPackegesAndArriveToLists().ConvertIenmurbleToObserve(lists); }
            else if (getingClientCmb.SelectedItem == GetItemAndPackegeNotArrive)
            { bl.ClientActiveHowGetingPackegesAndNotArriveToLists().ConvertIenmurbleToObserve(lists); }
        }

        private void AllClient_Checked(object sender, RoutedEventArgs e)
        {
            if(!AllClient.IsChecked.Value)
            bl.FilterClientList(false).ConvertIenmurbleToObserve(lists);
            if (AllClient.IsChecked.Value)
                bl.FilterClientList().ConvertIenmurbleToObserve(lists);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Closing += ClientsLIst_Closing;
            this.Close();
        }

        private void ClientsLIst_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void refreshboutton_Click(object sender, RoutedEventArgs e)
        {
            bl.ClientToLists().ConvertIenmurbleToObserve(lists);
        }
    }
}
