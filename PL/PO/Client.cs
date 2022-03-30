using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    /// <summary>
    ///client 
    /// </summary>
    public class ClientModel : INotifyPropertyChanged
    {
        uint id;
        string name;
        string endphone;
        string startPhone;
        LocationModel location;
        ObservableCollection<BO.PackageAtClient> fromClient;
        ObservableCollection<BO.PackageAtClient> toClient;
        bool active;

        public uint Id
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
        public string Name
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
        public string EndPhone
        {
            get
            {
                return endphone;
            }
            set
            {
                endphone = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        public string StartPhone
        {
            get
            {
                return startPhone;
            }
            set
            {
                startPhone = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        public LocationModel Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Location"));
                }
            }
        }

        public ObservableCollection<BO.PackageAtClient> FromClient
        {
            get
            {
                return fromClient;
            }
            set
            {
                fromClient = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FromClient"));
                }
            }
        }
        public ObservableCollection<BO.PackageAtClient> ToClient
        {
            get
            {
                return toClient;
            }
            set
            {
                toClient = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ToClient"));
                }
            }
        }
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Active"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

   

        public static implicit operator BO.Client(ClientModel client)
        {
            if (client is null)
                return null;
            return new BO.Client
            {
                Active = client.Active,
                Id = client.Id,
                Location = client.Location,
                Name = client.Name,
                Phone= client.StartPhone+client.endphone,
                FromClient = client.FromClient,
                ToClient = client.ToClient

            };
        }

        public static implicit operator BO.ClientToList(ClientModel client)
        {
            if (client is null)
                return null;
            return new BO.ClientToList { Active = true, ID = client.Id, Name = client.Name, Arrived = 0, NotArrived = 0, OnTheWay = 0, Phone = client.StartPhone + client.EndPhone, received = 0 };
        }


    }
}
