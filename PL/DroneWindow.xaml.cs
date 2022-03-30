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
using Microsoft.VisualBasic;
using BlApi;
using BO;
using PO;
using System.Reflection;
using Microsoft.Win32;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL bl;
        DroneItemModel drone;
        BO.DroneModel droneModel;
        BO.DroneStatus droneStatus;
        private int _noOfErrorsOnScreen = 0;
        bool addDrone;
        ObservableCollection<DroneToList> lists;
        bool simulatorActive;
       
        public DroneWindow(BlApi.IBL bl, ObservableCollection<DroneToList> lists = null)
        {
            try
            {
            
                InitializeComponent();
                drone = new();
                addDrone = true;
                this.lists = lists;
                this.DataContext = drone;
                StatusComb.Items.Add(BO.DroneStatus.Maintenance);
                droneStatus = DroneStatus.Maintenance;
                StatusComb.SelectedItem = StatusComb.Items[0];
                this.bl = bl;
                WeightChoseCombo.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
                ModelComboBox.ItemsSource = Enum.GetValues(typeof(BO.DroneModel));
                BaseChosingCombo.Items.Clear();
                BaseChosingCombo.ItemsSource = bl.BaseStationWhitFreeChargingStationToLists();
                DroneLabel.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        public DroneWindow(BlApi.IBL bl, uint droneFromListView, ObservableCollection<DroneToList> lists = null)
        {
            ctorUpdate(bl, droneFromListView, lists);

        }
        /// <summary>
        /// crate the window update
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="droneFromListView"></param>
        /// <param name="lists"></param>
        private void ctorUpdate(IBL bl, uint droneFromListView, ObservableCollection<DroneToList> lists = null)
        {
            try
            {
                InitializeComponent();
                this.bl = bl;
                drone = new();
                bl.GetDrone(droneFromListView).dronefromBl(drone);
                this.lists = lists;
                statusOption();
                this.DataContext = drone;
                ModelComboBox.ItemsSource = Enum.GetValues(typeof(BO.DroneModel));
                UpdateDronWindow(droneFromListView, false);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }
        /// <summary>
        /// update the drone view window to be in update window
        /// </summary>
        /// <param name="droneFromListView"></param>
        /// <param name="updeat"></param>
        private void UpdateDronWindow(uint droneFromListView, bool updeat = true)
        {
            try
            {

                this.Dispatcher.Invoke(() =>
                {
                    if (updeat)
                          bl.GetDrone(droneFromListView).dronefromBl(drone);
                    TitelDroneLabel.Content = "Updte Drone Window";
                    droneModel = drone.Model;
                    addDrone = false;
                    updeat = true;
                    SirialNumberTextBox.IsEnabled = false;
                    droneStatus = drone.DroneStatus;
                    DroneLabel.Visibility = Visibility.Visible;
                    BaseChosingCombo.Visibility = Visibility.Collapsed;
                    if (lists != null)
                        bl.DroneToLists().ConvertIenmurbleToObserve(lists);


                });
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void statusOption()
        {

            if (this.drone.DroneStatus == BO.DroneStatus.Free)
            {
                StatusComb.ItemsSource = Enum.GetValues(typeof(DroneStatus));
                StatusComb.IsEnabled = true;
            }
            else if (this.drone.DroneStatus == BO.DroneStatus.Maintenance)
            {
                DroneStatus[] status = { DroneStatus.Free, DroneStatus.Maintenance };
                StatusComb.ItemsSource = status;
              
                
                StatusComb.IsEnabled = true;

            }
            else if (this.drone.DroneStatus == BO.DroneStatus.Work)
            {
                StatusComb.ItemsSource = Enum.GetValues(typeof(DroneStatus));


                StatusComb.IsEnabled = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            e.Cancel = true;

        }

        private void DronesWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {


            if (BaseChosingCombo.SelectedItem is null)
            {
                if (BaseChosingCombo.SelectedItem is null)
                    InputMissingLocationLabel.Visibility = Visibility.Visible;

                return;
            }
            try
            {
               // droneToList.Model = (BO.DroneModel)ModelComboBox.SelectedItem;
                bl.AddDrone(drone, ((BO.BaseStationToList)BaseChosingCombo.SelectedItem).SerialNum);
                UpdateDronWindow(drone.SerialNumber);

                bl.GetDrone(drone.SerialNumber).dronefromBl(drone);
                MessageBox.Show($"Drone number {drone.SerialNumber} \n add to list!", "succesful");
               

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            exit();
        }

        private void exit()
        {
            if (simulatorActive)
            {
                simulatorActive = false;
                bl.PlayThred(drone.SerialNumber, () => { }, () => simulatorActive);
            }
            Closing += DroneWindow_Closing;
            this.Close();
        }

        private void DroneWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;

        }

        private void BaseChosingCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            InputMissingLocationLabel.Visibility = Visibility.Collapsed;

        }

        private void StatusComb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!simulatorActive)
                if (drone != null && StatusComb.SelectedItem != null && (BO.DroneStatus)StatusComb.SelectedItem != droneStatus)
                {

                    try
                    {
                        switch (drone.DroneStatus)
                        {
                            case BO.DroneStatus.Free:
                                bl.FreeDroneFromCharging(drone.SerialNumber);
                                MessageBox.Show($"Drone number {drone.SerialNumber} free from charge");
                                break;
                            case BO.DroneStatus.Maintenance:
                                bl.DroneToCharge(drone.SerialNumber);
                                MessageBox.Show($"Drone number {drone.SerialNumber} send to charge");
                                break;
                            case BO.DroneStatus.Work:
                                bl.ConnectPackegeToDrone(drone.SerialNumber);
                                MessageBox.Show($"Drone number {drone.SerialNumber} connect to packege ");
                                break;
                            case BO.DroneStatus.Delete:
                                bl.DeleteDrone(drone.SerialNumber);
                                exit();
                                MessageBox.Show($"Drone number {drone.SerialNumber} is Deleted ");
                                break;
                            default:
                                break;

                        }
                        droneStatus = drone.DroneStatus;
                        statusOption();
                        if (lists != null)
                            bl.FilterDronesList().ConvertIenmurbleToObserve(lists);
                        UpdateDronWindow(drone.SerialNumber);


                    }

                    catch (Exception ex)
                    {

                        StatusComb.SelectedItem = droneStatus;
                        MessageBox.Show(ex.ToString(), "ERROR");
                    }

                }
        }

        private void ModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (drone != null&&!addDrone)
                    if (droneModel != (BO.DroneModel)ModelComboBox.SelectedItem)
                    {
                        bl.UpdateDroneName(drone.SerialNumber, (BO.DroneModel)ModelComboBox.SelectedItem);
                        bl.GetDrone(drone.SerialNumber).dronefromBl(drone);
                        droneModel = drone.Model;
                        
                        if (lists != null)
                            bl.FilterDronesList().ConvertIenmurbleToObserve(lists);

                    }
                if (drone is null)
                    drone.Model = (BO.DroneModel)ModelComboBox.SelectedItem;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        private void TextBlock_PreviewMouseRightButtonDownPackegeWindow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new PackageView(bl, drone.PackageInTransfer.SerialNum).ShowDialog();
                UpdateDronWindow(drone.SerialNumber);
                if (lists != null)
                    bl.FilterDronesList().ConvertIenmurbleToObserve(lists);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        private void Simulator_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                simulatorActive = true;
                bl.PlayThred(drone.SerialNumber, () => UpdateDronWindow(drone.SerialNumber), () => simulatorActive);
            }catch (  DroneTryToStartSecondeSimolatorException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }
            catch(Exception)
            { }

        }

        private void Manual_Click(object sender, RoutedEventArgs e)
        {
            simulatorActive = false;
            bl.PlayThred(drone.SerialNumber, () => { }, () => simulatorActive);


        }

        private void ModelComboBox_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                if (drone != null)
                    if (drone.Model != (BO.DroneModel)ModelComboBox.SelectedItem)
                    {
                        bl.UpdateDroneName(drone.SerialNumber, (BO.DroneModel)ModelComboBox.SelectedItem);

                        DroneLabel.DataContext = bl.GetDrone(drone.SerialNumber);
                        if (lists != null)
                            bl.FilterDronesList().ConvertIenmurbleToObserve(lists);

                    }
                if (drone is null)
                    drone.Model = (BO.DroneModel)ModelComboBox.SelectedItem;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        private void Binding_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void ModelComboBox_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            try
            {
                if (drone != null)
                    if (drone.Model != (BO.DroneModel)ModelComboBox.SelectedItem)
                    {
                        bl.UpdateDroneName(drone.SerialNumber, (BO.DroneModel)ModelComboBox.SelectedItem);

                        DroneLabel.DataContext = bl.GetDrone(drone.SerialNumber);
                        if (lists != null)
                            bl.FilterDronesList().ConvertIenmurbleToObserve(lists);

                    }
                if (drone is null)
                    drone.Model = (BO.DroneModel)ModelComboBox.SelectedItem;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
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
    }
}
