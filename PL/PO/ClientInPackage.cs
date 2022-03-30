using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace PO
{
    /// <summary>
    /// Client In Package
    /// </summary>
    public class ClientInPackageModel :  INotifyPropertyChanged
    {
        
        private uint id;
        private string name;
        public  uint Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }
        public  string Name
        {
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

        public static implicit operator ClientInPackageModel(ClientInPackage clientBl)
        {
            if (clientBl is null)
                return null;
            return new ClientInPackageModel
            {
                Id = clientBl.Id,
                Name = clientBl.Name
            };
        }
        public static implicit operator ClientInPackage(ClientInPackageModel client)
        {
            if (client is null)
                return null;
            return new ClientInPackage
          {
              Id = client.Id,
              Name = client.Name
          };
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            string print = "";
            print += $"ID: {Id},\n";
            print += $"Name: {Name}\n";
    
            return print;
        }
    }
}
