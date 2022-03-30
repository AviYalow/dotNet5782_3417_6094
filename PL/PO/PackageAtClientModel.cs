using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace PO    
{

    /// <summary>
    /// Package At Client
    /// </summary>
    public class PackageAtClientModel: INotifyPropertyChanged
    {
        ClientInPackageModel client2;
        Priority priority;
        uint serialNum;
        WeightCategories weightCatgory;
        PackageStatus packageStatus;

        public uint SerialNum {
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
        public  WeightCategories WeightCatgory {
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
        public Priority Priority {
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
        public  PackageStatus PackageStatus
        {
            get
            {
                return packageStatus;
            }


            set
            {
               packageStatus = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PackageStatus"));
                }
            }
        }
       
        //The other client in the package.
        //The receiver for the sender and sender for the receiver
        public  ClientInPackageModel  Client2 {
            get
            {
                return client2;
            }
            set
            {
               client2 = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Client2"));
                }
            }
        }


        public static implicit operator PackageAtClientModel(BO.PackageAtClient package)
        {
            if (package is null)
                return null;
            return new PackageAtClientModel
            {
                SerialNum = package.SerialNum,
                Client2 = package.Client2,
                Priority = package.Priority,
                WeightCatgory = package.WeightCatgory,
                PackageStatus = package.PackageStatus

            };
        }

        public static implicit operator PackageAtClient(PackageAtClientModel package)
        {
            if (package is null)
                return null;
            return new PackageAtClient
            {
                SerialNum = package.SerialNum,
                Client2 = package.Client2,
                Priority = package.Priority,
                WeightCatgory = package.WeightCatgory,
                PackageStatus = package.PackageStatus

            };
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            String print = "";
            print += $"Serial Number: {SerialNum},\n";
            print += $"Weight Category: {WeightCatgory},\n";
            print += $"priority: {Priority},\n";
            print += $"Package Status: {PackageStatus},\n";
            print += $"The other client in the package: {Client2.Name}\n";
            return print;
        }
        }
}
