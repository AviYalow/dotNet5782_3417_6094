using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BO;

namespace PO
{
    /// <summary>
    /// base station
    /// </summary>
  public  class BaseStationModel:  INotifyPropertyChanged
    {
        uint serialNum;
        string name;
        LocationModel location;
        ObservableCollection<DroneInCharge> dronesInChargeList;
        uint freeState;
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

        public  string Name {
            get
            {
                return name;
            }
            set
            {
                name = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        public LocationModel  Location
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
        public uint FreeState
        {
            get
            {
                return freeState;
            }
            set
            {
                freeState = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FreeState"));
                }
            }
        }
        public ObservableCollection<DroneInCharge > DronesInChargeList {
            get
            {
              
                return dronesInChargeList; 
            }
            set
            {
              
                
                    dronesInChargeList=value;
               
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DronesInChargeList"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        
        public static implicit operator BaseStation(BaseStationModel base_)
        {
            if (base_ is null)
                return null;
            return new BaseStation
            {
                FreeState = base_.FreeState,
                Location = base_.Location,
                Name = base_.Name,
                SerialNum = base_.serialNum,
                DronesInChargeList = base_.dronesInChargeList
            };
        }

       






        public override string ToString()
        {
            String print= "";
            print += $"Siral Number is {SerialNum},\n";
            print += $"The Name is {Name},\n";
            print += $"Location: Latitude:{Location.Latitude} Longitude:{Location.Longitude},\n";
            print += $"Number of free state: {FreeState},\n";
            print += $"Drone in Charge: {DronesInChargeList.Count()},\n";
            foreach(DroneInChargeModel  drone in DronesInChargeList)
            {
               print += $"Serial number: {drone.SerialNum}, butrry Status: {drone.ButrryStatus}\n";
            }
            return print;
        }

    }
}
