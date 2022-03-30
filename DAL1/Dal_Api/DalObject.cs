using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DO;
using Ds;
using System.Runtime.CompilerServices;

namespace Dal
{
    
    sealed partial class DalObject : DalApi.IDal
    {
        private static readonly Lazy<DalObject> lazy = new Lazy<DalObject>(() => new DalObject());
        public static DalObject Instance { get { return lazy.Value; } }

        /// <summary>
        ///Creating entities with initial initialization
        /// </summary>
        private DalObject()
        {

        }


        /// <summary>
        /// show the distance between 2 locations
        /// </summary>
        /// <param name="Longitude1">the first longitude location</param>
        /// <param name="Latitude1"> the first latitude location</param>
        /// <param name="Longitude2">the second longitude location</param>
        /// <param name="Latitude2">the second latitude location</param>
        /// <returns>distance between 2 locations</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Distance(double Longitude1, double Latitude1, double Longitude2, double Latitude2)
        {
            return DO.Point.Distance(Longitude1, Latitude1, Longitude2, Latitude2);
        }


        /// <summary>
        /// Returns a point in the form of degrees
        /// </summary>
        /// <param name="point"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string PointToDegree(double point)
        {
            return Point.Degree(point);
        }

        /// <summary>
        /// return list of charging drones
        /// </summary>
        /// <returns>return list of charging drones</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BatteryLoad> ChargingDroneList(Predicate<BatteryLoad> predicate)
        {

            return from x in DataSource.droneInCharge
                   where predicate(x)
                   select x;
        }

    }
}
