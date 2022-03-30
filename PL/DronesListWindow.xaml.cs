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
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        BlApi.IBL bl;
      ObservableCollection<DroneToList> lists;
        CollectionView view;
        PropertyGroupDescription groupDescription;
        BO.DroneToList drone;
        public DronesListWindow( BlApi.IBL bl)
        {
            try
            {
                InitializeComponent();

                this.bl = bl;
                lists = new ObservableCollection<DroneToList>(bl.FilterDronesList());
                WeightSelctor.Items.Add("");
                StatusSelector.Items.Add("");
                foreach (var item in Enum.GetValues(typeof(BO.WeightCategories)))
                    WeightSelctor.Items.Add(item);
                foreach (var item in Enum.GetValues(typeof(BO.DroneStatus)))
                    StatusSelector.Items.Add(item);
                drone = new BO.DroneToList();
                DataContext = lists;

                view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
                groupDescription = new PropertyGroupDescription("Model");
            }
            catch(Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        private void WeightSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {try
            {
                if (WeightSelctor.SelectedItem is null)
                    return;
                if (WeightSelctor.SelectedItem == WeightSelctor.Items[0])
                {
                    bl.DroneToListsByWhight().ConvertIenmurbleToObserve(lists);
                }
                else
                     bl.DroneToListsByWhight((BO.WeightCategories)WeightSelctor.SelectedItem).ConvertIenmurbleToObserve(lists);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (StatusSelector.SelectedItem is null)
                    return;
                if (StatusSelector.SelectedItem == StatusSelector.Items[0])
                {
                     bl.DroneToListsByStatus().ConvertIenmurbleToObserve(lists);

                }
                else
                    bl.DroneToListsByStatus((BO.DroneStatus)StatusSelector.SelectedItem).ConvertIenmurbleToObserve(lists);
            }
            catch(Exception ex)

            { MessageBox.Show(ex.ToString()); }
        }


        private void ChoseDrone(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DronesListView.SelectedItem != null)
                {
                    new DroneWindow(bl, ((BO.DroneToList)DronesListView.SelectedItem).SerialNumber,lists).Show();
                 
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow( bl,lists).Show();
          
        }

        private void Button_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Closing += DronesListWindow_Closing;
            this.Close();
        }

        private void DronesListWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

        }



        private void HeaderedContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            HeaderedContentControl control = sender as HeaderedContentControl;
            try
            {
                bl.SortList(control.Name,lists).ConvertIenmurbleToObserve(lists);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
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
                e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.NumPad0
                || e.Key == Key.NumPad1|| e.Key == Key.NumPad2|| e.Key == Key.NumPad3|| e.Key == Key.NumPad4
                || e.Key == Key.NumPad5|| e.Key == Key.NumPad6|| e.Key == Key.NumPad7|| e.Key == Key.NumPad8
                || e.Key == Key.NumPad9)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                {

                    return;

                }
            //let this key be written inside the textbox

            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }

        private void selectByNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text.Any(x => x < '0' || x > '9'))
                return;
            bl.DroneToListFilterByNumber(text.Text).ConvertIenmurbleToObserve(lists);
        }

        private void gropListCB_Checked(object sender, RoutedEventArgs e)
        {
            if (gropListCB.IsChecked==true)
            {
                view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
                groupDescription = new PropertyGroupDescription("Model");
                if (!view.GroupDescriptions.Any(x=>x.Equals(groupDescription)))
                view.GroupDescriptions.Add(groupDescription);
                
            }
            else
            {
                view.GroupDescriptions.Clear();
             
            }
        }

        private void refreshboutton_Click(object sender, RoutedEventArgs e)
        {
            bl.DroneToLists().ConvertIenmurbleToObserve(lists);
        }
    }
}
