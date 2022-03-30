using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    /// <summary>
    /// Location
    /// </summary>
    public class LocationModel : INotifyPropertyChanged
    {
        static LocationModel location = new();

        double longitude;
        double latitude;

        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Longitude"));
                }
            }
        }
        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
               latitude = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Latitude"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public static implicit operator LocationModel(BO.Location locationOld)
        {
            if (locationOld is null)
                return null;
            location.Longitude = locationOld.Longitude;
            location.Latitude = locationOld.Latitude;

            return location;
        }

        public static implicit operator BO.Location(LocationModel locationOld)
        {
            if (locationOld is null)
                return null;
            return new BO.Location
            {
                Longitude = locationOld.longitude,
                Latitude = locationOld.latitude
            };


        }







        public override string ToString()
        {

            string print = "";
            print += $"{DO.Point.Degree(Latitude)} ";
            print += Latitude >= 0 ? "N \n" : "S \n";
            print += $"{DO.Point.Degree(Longitude)} ";
            print += Longitude >= 0 ? "E " : "W ";
            return print;
        }



    }
}
