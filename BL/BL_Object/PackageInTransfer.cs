using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Package In Transfer
    /// </summary>
    public class PackageInTransfer
    {
        public uint SerialNum { get; set; }
        public WeightCategories WeightCatgory { get; set; }
        public Priority Priority { get; set; }
        public bool InTheWay { get; set; }//true-in the way,false-waiting to be collected
        public ClientInPackage SendClient { get; set; }
        public ClientInPackage RecivedClient { get; set; }
        public Location Source { get; set; }
        public Location Destination { get; set; }
        public double Distance { get; set; }
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
            print += $"Source:\n {Source,5}\n" ;
            print += $"Destination:\n {Destination}\n";
            print += $"Distance: {Distance} KM\n";
            return print;
        }

    } 
}
