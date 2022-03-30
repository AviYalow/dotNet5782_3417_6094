using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BO
{
    /// <summary>
    /// base station
    /// </summary>
  public  class BaseStation
    {

        public uint SerialNum { get; init; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public uint FreeState { get; set; }
        public ObservableCollection<DroneInCharge> DronesInChargeList { get; set; }
        public override string ToString()
        {
            String print= "";
            print += $"Siral Number is {SerialNum},\n";
            print += $"The Name is {Name},\n";
            print += $"Location: Latitude:{Location.Latitude} Longitude:{Location.Longitude},\n";
            print += $"Number of free state: {FreeState},\n";
            print += $"Drone in Charge: {DronesInChargeList.Count()},\n";
            foreach(DroneInCharge drone in DronesInChargeList)
            {
               print += $"Serial number: {drone.SerialNum}, butrry Status: {drone.ButrryStatus}\n";
            }
            return print;
        }

    }
}
