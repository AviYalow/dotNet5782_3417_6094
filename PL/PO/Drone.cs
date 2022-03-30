using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BL.Cloning;
using System.Windows;

namespace PO
{
    /// <summary>
    /// Drone
    /// </summary>
    public class DroneItemModel : INotifyPropertyChanged
    {

        static BO.Drone Drone = new();
        // static DroneItemModel itemModel = new();

        private uint serialNumber;
        private DroneModel model;
        WeightCategories weightCategory;
        double butrryStatus;
        DroneStatus droneStatus;
        LocationModel location;
        PackageInTransferModel packageInTransfer;
        LocationName locationName;
        LocationNext locationNext;
        double distanseToNextLocation;
        public uint SerialNumber
        {
            get
            {
                return serialNumber;
            }
            set
            {
                serialNumber = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SerialNumber"));
                }
            }
        }
        public DroneModel Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Model"));
                }
            }
        }
        public WeightCategories WeightCategory
        {
            get
            {
                return weightCategory;
            }
            set
            {
                weightCategory = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("WeightCategory"));
                }
            }
        }
        public PackageInTransferModel PackageInTransfer
        {
            get
            {
                return packageInTransfer;
            }
            set
            {
                packageInTransfer = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PackageInTransfer"));
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
                    PropertyChanged(this, new PropertyChangedEventArgs("butrryStatus"));
                }
            }
        }


        public DroneStatus DroneStatus
        {
            get
            {
                return droneStatus;
            }
            set
            {
                droneStatus = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DroneStatus"));
                }
            }
        }
        public LocationModel Location
        {
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

        public LocationName LocationName
        {
            get { return locationName; }
            set
            {
                locationName = value;


                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("locationName"));
                }
            }
        }
        public LocationNext LocationNext
        {
            get
            { return locationNext; }
            set
            {
                locationNext = value;


                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("locationNext"));
                }
            }
        }
        public double DistanseToNextLocation
        {
            get
            { return distanseToNextLocation; }
            set
            {
                distanseToNextLocation = value;


                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("distanseToNextLocation"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;




        public static implicit operator BO.Drone(DroneItemModel model)
        {
            if (model is null)
                return null;
            return new BO.Drone
            {
                WeightCategory = model.WeightCategory,
                SerialNumber = model.SerialNumber,
                ButrryStatus = model.ButrryStatus,
                Model = model.Model,
                DroneStatus = model.DroneStatus,
                PackageInTransfer = model.PackageInTransfer,
                Location = new Location { Latitude = model.Location.Latitude, Longitude = model.Location.Longitude }


            };
        }
        public static implicit operator BO.DroneToList(DroneItemModel model)
        {
            if (model is null)
                return null;
            uint packege = model.PackageInTransfer is null ? 0 : model.packageInTransfer.SerialNum;
            return new DroneToList
            {
                ButrryStatus = model.ButrryStatus,
                Model = model.Model,
                DroneStatus = model.DroneStatus,
                Location = model.Location,
                NumPackage = packege,
                SerialNumber = model.SerialNumber,
                WeightCategory = model.WeightCategory,
                DistanseToNextLocation=model.DistanseToNextLocation,
                LocationName=model.LocationName,
                LocationNext=model.LocationNext
            };

        }



    }


}
