using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Package
    /// </summary>
    public class Package
    {
        public uint SerialNumber { get; init; }
        public ClientInPackage SendClient { get; set; }

        public ClientInPackage RecivedClient { get; set; }
        public WeightCategories WeightCatgory { get; set; }
        public Priority Priority { get; set; }
        public DroneInPackage Drone { get; set; }

        //Delivery time create a package
        public DateTime? Create_package { get; set; }

        //Time to assign the package to a drone
        public DateTime? PackageAssociation { get; set; }

        //Package collection time from the sender
        public DateTime? CollectPackage { get; set; }

        //Time of arrival of the package to the recipient
        public DateTime? PackageArrived { get; set; }
        public override string ToString()
        {
            String print = "";
            print += $"Serial Number: {SerialNumber},\n";
            print += $"Send Client: {SendClient.Name},\n";
            print += $"Recived Client: {RecivedClient.Name},\n";
            print += $"Weight Category: {WeightCatgory},\n";
            print += $"priority: {Priority},\n";
            print += Drone is null ? "drone: no drone\n" : $"drone: {Drone},\n";
            print += $"Delivery time create a package: {Create_package},\n";
            print += $"Time to assign:";
            if (PackageAssociation != DateTime.MinValue)
                print += $"{PackageAssociation},\n";
            else
            {
                print += "packege not assoction yet\n";
                return print;
            }
            print += $"Time Package collection:";
            if (CollectPackage != DateTime.MinValue)
                print += $"{CollectPackage},\n";
            else
            { print += "packege not collect yet\n"; return print; }


            print += $"Time of arrival:";
            if (PackageArrived != DateTime.MinValue)
                print += $"{ PackageArrived}\n";
            else
            {
                print += "packege not arrive yet\n";
                return print;
            }

            return print;
        }
    }
}
