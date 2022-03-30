using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;
using DalApi;
using static  BL.Cloning;
namespace BlApi
{
    partial class BL : IBL
    {

        /// <summary>
        /// convert droneToList object to drone object
        /// </summary>
        /// <param name="droneToList"> droneToList object</param>
        /// <returns> drone object </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        Drone convertDroneToListToDrone(DroneToList droneToList)
        {
            lock (dalObj)
            {
                return new Drone { SerialNumber = droneToList.SerialNumber, ButrryStatus = droneToList.ButrryStatus.Value, DroneStatus = droneToList.DroneStatus, Location = droneToList.Location, Model = droneToList.Model, WeightCategory = droneToList.WeightCategory, PackageInTransfer = convertPackegeDalToPackegeInTrnansfer(dalObj.packegeByNumber(droneToList.NumPackage)) };
        }
    }

        /// <summary>
        /// find specific drone in the list of the drones
        /// </summary>
        /// <param name="siralNuber"> serial number of the drone</param>
        /// <returns> drone founded </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneToList SpecificDrone(uint siralNuber)
        {
            lock (dalObj)
            {
                var drone = dronesListInBl.Find(x => x.SerialNumber == siralNuber && x.DroneStatus != DroneStatus.Delete);
            if (drone is null)
                throw new ItemNotFoundException("drone", siralNuber);
            return drone.Clone();
        }

        }
    }
}
