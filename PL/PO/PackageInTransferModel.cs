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
    /// Package In Transfer
    /// </summary>
    public class PackageInTransferModel : INotifyPropertyChanged
    {
       

        uint serialNum { get; set; }
        WeightCategories weightCatgory { get; set; }
        Priority priority { get; set; }
        bool inTheWay { get; set; }//true-in the way,false-waiting to be collected
        ClientInPackageModel sendClient { get; set; }
        ClientInPackageModel recivedClient { get; set; }
        LocationModel source { get; set; }
        LocationModel destination { get; set; }
        double distance { get; set; }

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
        public WeightCategories WeightCatgory
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
        public Priority Priority
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
        public bool InTheWay
        {
            get
            {
                return inTheWay;
            }
            set
            {
                inTheWay = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("InTheWay"));
                }
            }
        }//true-in the way,false-waiting to be collected
        public ClientInPackageModel SendClient
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
        public ClientInPackageModel RecivedClient
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
        public LocationModel Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Source"));
                }
            }
        }
        public LocationModel Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Destination"));
                }
            }
        }
        public double Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Distance"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static implicit operator PackageInTransferModel(PackageInTransfer packageBl)
        {
            if (packageBl is null)
                return null;
            return new PackageInTransferModel
            {
                Destination = packageBl.Destination,
                Distance = packageBl.Distance,
                InTheWay = packageBl.InTheWay,
                Priority = packageBl.Priority,
                RecivedClient = packageBl.RecivedClient,
                SendClient = packageBl.SendClient,
                SerialNum = packageBl.SerialNum,
                Source = packageBl.Source,
                WeightCatgory = packageBl.WeightCatgory
            };
            
        }

        public static implicit operator PackageInTransfer(PackageInTransferModel package)
        {

            if (package is null)
                return null;
            return new PackageInTransferModel
            {
                Destination = package.Destination,
                Distance = package.Distance,
                InTheWay = package.InTheWay,
                Priority = package.Priority,
                RecivedClient = package.RecivedClient,
                SendClient = package.SendClient,
                SerialNum = package.SerialNum,
                Source = package.Source,
                WeightCatgory = package.WeightCatgory
            };
        }
        public override string ToString()
        {
            String print = "";
            print += $"Serial Number: {SerialNum},\n";
            print += $"Weight Category: {WeightCatgory},\n";
            print += $"priority: {Priority},\n";
            print += "in the way:";
            print += InTheWay ? "yes\n" : "no\n";
            print += $"Send Client: {SendClient.Name},\n";
            print += $"Recived Client: {RecivedClient.Name},\n";
            print += $"Source:\n {Source,5}\n";
            print += $"Destination:\n {Destination}\n";
            print += $"Distance: {Distance} KM\n";
            return print;
        }

    }
}
