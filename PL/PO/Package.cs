using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BO;

namespace PO
{
    /// <summary>
    /// Package
    /// </summary>
    public class PackageModel:  INotifyPropertyChanged
    {
        uint serialNumber;
        ClientInPackageModel sendClient;

        ClientInPackageModel recivedClient;
        WeightCategories weightCatgory;
        Priority priority;
        DroneInPackageModel drone;

        //Delivery time create a package
        DateTime? create_package;

        //Time to assign the package to a drone
        DateTime? packageAssociation;

        //Package collection time from the sender
        DateTime? collectPackage;

        //Time of arrival of the package to the recipient
        DateTime? packageArrived;

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
        public  ClientInPackage  SendClient
        {
            get
            {
                return sendClient;
            }
            set
            {
              sendClient = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SendClient"));
                }
            }

        }

        public  ClientInPackage  RecivedClient
        {
            get
            {
                return recivedClient;
            }


            set
            {
                recivedClient = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RecivedClient"));
                }
            }
        }
        
        public  WeightCategories WeightCatgory
        {
            get
            {
                return weightCatgory;
            }


            set
            {
                weightCatgory = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("WeightCatgory"));
                }
            }
        }
        
        public  Priority Priority
        {
            get
            {
                return priority;
            }


            set
            {
                priority = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Priority"));
                }
            }
        }
      
        public  DroneInPackageModel Drone
        {
            get
            {
                return drone;
            }
            set
            {
                drone = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Drone"));
                }
            }
        }

        //Delivery time create a package
        public  DateTime? Create_package {
            get
            {
                return create_package;
            }
            set
            {
                create_package = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Create_package"));
                }
            }
        }

        //Time to assign the package to a drone
        public  DateTime? PackageAssociation
        {
            get
            {
                return packageAssociation;
            }
            set
            {
                packageAssociation = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PackageAssociation"));
                }
            }
        }

        //Package collection time from the sender
        public  DateTime? CollectPackage {
            get
            {
                return collectPackage;
            }
            set
            {
                collectPackage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CollectPackage"));
                }
            }
        }

        //Time of arrival of the package to the recipient
        public  DateTime? PackageArrived {
            get
            {
                return packageArrived;
            }
            set
            {
                packageArrived = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PackageArrived"));
                }
            }
        }


        public static implicit operator Package(PackageModel package)
        {
            if (package is null)
                return null;
            return new Package
            {
                CollectPackage = package.CollectPackage,
                Create_package = package.Create_package,
                Drone = package.Drone,
                PackageArrived = package.PackageArrived,
                PackageAssociation = package.PackageAssociation,
                Priority = package.Priority,
                RecivedClient = package.RecivedClient,
                SendClient = package.SendClient,
                SerialNumber = package.SerialNumber,
                WeightCatgory = package.WeightCatgory
            };
        }
       

        public event PropertyChangedEventHandler PropertyChanged;
   
    }
}
