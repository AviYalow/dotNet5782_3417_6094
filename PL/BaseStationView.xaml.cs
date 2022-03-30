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
using Mapsui.Utilities;
using Mapsui.Layers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using PO;

namespace PL
{

    /// <summary>
    /// Interaction logic for BaseStationView.xaml
    /// </summary>
    public partial class BaseStationView : Window
    {
        BlApi.IBL bl;
        BaseStationModel baseStation;
        ObservableCollection<BaseStationToList> lists;
        private int _noOfErrorsOnScreen = 0;

        int numberOfChargingStation;
        public BaseStationView(BlApi.IBL bL, ObservableCollection<BaseStationToList> lists)
        {
            try
            {
                InitializeComponent();
                bl = bL;
                this.lists = lists;
                baseStation = new BaseStationModel();
                baseStation.Location = new Location();

                DataContext = baseStation;
        
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }



        }
        public BaseStationView(BlApi.IBL bL, BaseStationToList base_, ObservableCollection<BaseStationToList> lists)
        {
            try
            {
                InitializeComponent();
                bl = bL;
                this.lists = lists;
                baseStation = new BaseStationModel();

                ctorByItems(base_.SerialNum);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }

            
        }

        private void ctorByItems(uint base_)
        {
            try
            {

                SerialText.IsEnabled = false;

                DroneCharge1View.Visibility = Visibility.Visible;
                updateBase(base_);
                DataContext = baseStation;
                AddButton.Visibility = Visibility.Collapsed;
                TitelLabel.Content = "Base Station update";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.Close();
            }


        }

        private void updateBase(uint base_)
        {
              bl.BaseByNumber(base_).BaseFromBl(baseStation);
            numberOfChargingStation = (int)baseStation.FreeState + baseStation.DronesInChargeList.Count();
            
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
       
            try
            {
                bl.AddBase(baseStation);
                MessageBox.Show("Add base Station Succes!");
                numberOfChargingStation = (int)baseStation.FreeState;
                lists.Add(new BaseStationToList { Active = "Active", BusyState = 0, FreeState = baseStation.FreeState, Name = baseStation.Name, SerialNum = baseStation.SerialNum });
                ctorByItems(baseStation.SerialNum);



            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString(), "ERROE"); }
        }

      

   

        private void HeaderedContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            HeaderedContentControl control = sender as HeaderedContentControl;
            try
            {
                 bl.SortList(control.Name, baseStation.DronesInChargeList ).ConvertIenmurbleToObserve(baseStation.DronesInChargeList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }
        }

        private void Latitudtext_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "0")
                ((TextBox)sender).Text = "";
        }

        private void LongitudeText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "0";
        }

        private void DroneCharge1View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new DroneWindow(bl, (((DroneInCharge)DroneCharge1View.SelectedItem).SerialNum)).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }
            finally
            {
                updateBase(baseStation.SerialNum);
                bl.BaseStationToLists().ConvertIenmurbleToObserve(lists);

            }
        }

        private void NameText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (AddButton.Visibility != Visibility.Visible)
                {
                    if (MessageBox.Show("Do you want to update the base name?", "Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        try
                        {
                            bl.UpdateBase(baseStation.SerialNum, ((TextBox)sender).Text, "");

                            MessageBox.Show("Update seccsed!");
                            updateBase(baseStation.SerialNum);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "ERROR");
                        }
                    }
                    else
                        ((TextBox)sender).Text = baseStation.Name;
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

            e.Cancel = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Closing += BaseStationView_Closing;
            this.Close();
        }

        private void BaseStationView_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteBase(baseStation.SerialNum);
                if (lists != null)
                    bl.BaseStationToLists().ConvertIenmurbleToObserve(lists);
                MessageBox.Show($"Base number{baseStation.SerialNum} deleted!");
                this.Closing += BaseStationView_Closing;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }
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

        private void update_Click(object sender, RoutedEventArgs e)
        {
            string name = NameText.Text;
            if (MessageBox.Show("Do you want to update the number of charching station\n or the baseStation name?", "Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                try
                {
                    
                    bl.UpdateBase(baseStation.SerialNum, NameText.Text, FreeStateText.Text);
                    MessageBox.Show("Update seccsed!");
                    bl.BaseStationToLists().ConvertIenmurbleToObserve(lists);

                    updateBase(baseStation.SerialNum);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR");
                    FreeStateText.Text = numberOfChargingStation.ToString();
                    NameText.Text = name;
                }
            }
            else
            {
                FreeStateText.Text = numberOfChargingStation.ToString();
                NameText.Text = name;
            }
            }

        private void Text_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
