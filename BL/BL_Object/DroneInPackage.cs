using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Drone In Package
    /// </summary>
    public class DroneInPackage
    {
        public uint SerialNum { get; set; }
        public double ButrryStatus { get; set; }
        public Location Location { get; set; }

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
