using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO    
{

    /// <summary>
    /// Package At Client
    /// </summary>
    public class PackageAtClient
    {
        public uint SerialNum { get; set; }
        public WeightCategories WeightCatgory { get; set; }
        public Priority Priority { get; set; }
        public PackageStatus PackageStatus { get; set; }
       
        //The other client in the package.
        //The receiver for the sender and sender for the receiver
        public ClientInPackage Client2 { get; set; }

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
