using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Client To List
    /// </summary>
    public class ClientToList
    {

        public uint ID { get; init; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public uint Arrived { get; set; } //the number of package that send and arrived
        public bool Active { get; set; }
        public uint NotArrived { get; set; }//the number of package that send and hasn't arrived yet
        public uint received { get; set; }//the number of package that recived
        public uint OnTheWay { get; set; }//the number of package that on the way for this client
        public override string ToString()
        {
            String print = "";
            print += $"ID: {ID},\n";
            print += $"The Name is {Name},\n";
            print += $"Phone {Phone},\n";
            print += $"Amount of package that send and arrived: {Arrived},\n";
            print += $"Amount of package that send but not arrived yet: {NotArrived},\n";
            print += $"Amount of package that recived : {received},\n";
            print += $"Amount of package that on the way : {OnTheWay},\n";

            return print;
        }


    }
}
