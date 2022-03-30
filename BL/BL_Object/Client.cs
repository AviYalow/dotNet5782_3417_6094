using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    ///client 
    /// </summary>
    public class Client
    {
        public uint Id { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }

        public ObservableCollection<PackageAtClient> FromClient { get; set; }
        public ObservableCollection<PackageAtClient> ToClient { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            String print = "";
            print += $"ID: {Id},\n";
            print += $"Name is {Name},\n";
            print += $"Phone: {Phone},\n";
            print += $"Location: {Location}\n";
            print += $"Statos client: ";
            print += Active ? "Active\n" : "Not active\n";
            print += "Packege from this client:\n";
            if (FromClient != null)
                foreach (var packege in FromClient)
                { print += $"{packege}"; }
            else
                print += "0\n";
            print += "Packege to this client:\n";
            if (ToClient != null)
                foreach (var packege in ToClient)
                { print += $"{packege}"; }
            else
                print += "0\n";

            return print;
        }


    }
}
