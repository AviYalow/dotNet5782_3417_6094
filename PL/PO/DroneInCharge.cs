using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PO
{
    /// <summary>
    /// Drone In Charge
    /// </summary>
    public class DroneInChargeModel :  INotifyPropertyChanged
    {
        uint serialNum;
        double butrryStatus;
        
        public uint SerialNum
        {
            get
            {
                return serialNum;
            }
            set
            {
                serialNum = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SerialNum"));
                }
            }
        }
        public double ButrryStatus
        {
            get
            {
                return butrryStatus;
            }
            set
            {
                butrryStatus = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ButrryStatus"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static implicit operator BO.DroneInCharge(DroneInChargeModel drone)
        {
            if (drone is null)
                return null;
            return new BO.DroneInCharge
            {
                ButrryStatus = drone.ButrryStatus,
                SerialNum = drone.SerialNum
            };
        }
        public static implicit operator DroneInChargeModel(BO.DroneInCharge drone)
        {
            if (drone is null)
                return null;
            return new DroneInChargeModel
            {
                ButrryStatus = drone.ButrryStatus,
                SerialNum = drone.SerialNum
            };
        }

       
    }
}
