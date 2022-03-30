using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    /// <summary>
    /// Drone In Package
    /// </summary>
    public class DroneInPackageModel :  INotifyPropertyChanged
    {
        uint serialNum;
        double butrryStatus;
        LocationModel location;

        public  uint SerialNum {
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
        public  double ButrryStatus {
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
        public  LocationModel  Location {
            get
            {
                return location;
            }
            set
            {
               location = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Location"));
                }
            }
        }

        public static implicit operator DroneInPackageModel(BO.DroneInPackage drone)
        {
            if (drone is null)
                return null;
            return new DroneInPackageModel
            {
                ButrryStatus = drone.ButrryStatus,
                Location = drone.Location,
                SerialNum = drone.SerialNum
            };
        }

        public static implicit operator BO.DroneInPackage(DroneInPackageModel drone)
        {
            if (drone is null)
                return null;
            return new DroneInPackageModel
            {
                ButrryStatus = drone.ButrryStatus,
                Location = drone.Location,
                SerialNum = drone.SerialNum
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            String print = "";
            print += $"Siral Number: {SerialNum},\n";
            print += $"Butrry Status: {ButrryStatus}\n";
            print += Location;
            return print;

        }
    }
}
