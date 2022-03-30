using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Client In Package
    /// </summary>
    public class ClientInPackage
    {
        public uint Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            string print = "";
            print += $"ID: {Id},\n";
            print += $"Name: {Name}\n";
    
            return print;
        }
    }
}
