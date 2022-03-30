using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Package To List
    /// </summary>
    public class PackageToList
    {
        public uint SerialNumber { get; set; }
        public string SendClient { get; set; }
        public string RecivedClient { get; set; }
        public Priority priority { get; set; }
        public WeightCategories WeightCategories { get; set; }
        public PackageStatus packageStatus { get; set; }
        public DateTime? Create { get; set; }
        public bool Drone { get; set; }

        public override string ToString()
        {
            String print = "";
            print += $"Serial Number: {SerialNumber},\n";
            print += $"Send Client: {SendClient},\n";
            print += $"Recived Client: {RecivedClient},\n";
            print += $"Weight Category: {WeightCategories},\n";
            print += $"Priority: {priority},\n";
            print += $"Package Status: {packageStatus},\n";
            return print;
        }

    }
}