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
using BO;
using BlApi;



namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStationsList.xaml
    /// </summary>
    public partial class BaseStationsList : Window
    {
        IBL bl;

        ObservableCollection<BaseStationToList> lists;
        CollectionView view;
        PropertyGroupDescription groupDescription;
        public BaseStationsList(BlApi.IBL bL)
        {
            InitializeComponent();
            bl = bL;
            lists = new ObservableCollection<BaseStationToList>(bl.BaseStationToLists());
            DataContext = lists;
            view = (CollectionView)CollectionViewSource.GetDefaultView(BaseListView.ItemsSource);
            groupDescription = new PropertyGroupDescription("FreeState");
        }


        /// <summary>
        /// Clicking an object from the list of base stations
        /// Opens the station data in a new window for updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BaseListView.SelectedItem != null)
            {
                new BaseStationView(bl, (BaseStationToList)BaseListView.SelectedItem, lists).Show();

            }
        }

        /// <summary>
        /// Option to sort the list by clicking on the headings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Select which base stations will be displayed in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FiletrListCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (FiletrListCmb.SelectedItem == BaseStationActive)
                {
                    bl.BaseStationToLists().ConvertIenmurbleToObserve(lists);


                }
                if (FiletrListCmb.SelectedItem == BaseStationWithFreeChargingStation)
                {
                    bl.BaseStationWhitFreeChargingStationToLists().ConvertIenmurbleToObserve(lists);


                }
                else
                    bl.AllBaseStation().ConvertIenmurbleToObserve(lists);


            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString(), "ERROR"); }
        }

        /// <summary>
        /// Selection of the desired display type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoceDroneCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ChoceGroupCmb.SelectedItem == ChoceGroupCmb.Items[0] || ChoceGroupCmb.SelectedItem is null)
                {
                    if (view.GroupDescriptions != null)
                        view.GroupDescriptions.Clear();
                }
                else
                {
                    view = (CollectionView)CollectionViewSource.GetDefaultView(BaseListView.ItemsSource);
                    view.GroupDescriptions.Add(groupDescription);
                }
            }
            catch(Exception)
            { }

        }


        /// <summary>
        /// Click the 'Add New Base Station' button. 
        /// Opens a new window to add a new base station.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new BaseStationView(bl, lists).Show();

            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }
        /// <summary>
        /// Blocking the window closing in the regular way
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// Close the current window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Closing += BaseStationsList_Closing;
            this.Close();
        }

        /// <summary>
        /// help function to close the current window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseStationsList_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void refreshboutton_Click(object sender, RoutedEventArgs e)
        {
            bl.BaseStationToLists().ConvertIenmurbleToObserve(lists);
        }
    }


}
