using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalApi;
using System.Runtime.CompilerServices;
using static BL.Cloning;

namespace BlApi
{
    partial class BL : IBL
    {
        /// <summary>
        /// add drone to list
        /// </summary>
        /// <param name="drone"> drone to add</param>
        /// <param name="base_"> serial number of base station for first chraging</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(DroneToList drone, uint base_)
        {
            lock (dalObj)
            {
                try
                {
                    var baseStation = BaseByNumber(base_);
                    drone.Location = baseStation.Location;
                    if (drone.WeightCategory > WeightCategories.Heavy)
                        throw new InputErrorException();



                    drone.DroneStatus =baseStation.FreeState>0? DroneStatus.Maintenance : DroneStatus.Free;
                    drone.LocationName = LocationName.Base;
                    drone.LocationNext = LocationNext.None;
                    drone.DistanseToNextLocation = 0;
                    Random random = new Random();

                    dalObj.AddDrone(new DO.Drone { SerialNumber = drone.SerialNumber, Model = (DO.DroneModel)drone.Model, WeightCategory = (DO.WeightCategories)drone.WeightCategory });


                    drone.ButrryStatus = random.Next(20, 41);


                    dronesListInBl.Add(drone);
                    if(drone.DroneStatus==DroneStatus.Maintenance)
                    dalObj.DroneToCharge(drone.SerialNumber, base_);
                }
                catch (DO.ItemFoundException ex)
                {
                    throw (new ItemFoundExeption(ex));
                }
            }
        }

        /// <summary>
        /// update new location for drone
        /// </summary>
        /// <param name="drone"> serial number of drone</param>
        /// <param name="location"> new location</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDronelocation(uint drone, Location location)
        {
            lock (dalObj)
            {
                int i = dronesListInBl.FindIndex(x => x.SerialNumber == drone && x.DroneStatus != DroneStatus.Delete);
                if (i == -1)
                    throw (new ItemNotFoundException("Drone", drone));
                dronesListInBl[i].Location = location.Clone();
            }
        }

        /// <summary>
        /// update new model for drone
        /// </summary>
        /// <param name="droneId"> serial number of the drone</param>
        /// <param name="newName"> new name to change</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneName(uint droneId, DroneModel newName)
        {
            lock (dalObj)
            {
                DO.Drone droneInData;
                try
                {

                    droneInData = dalObj.DroneByNumber(droneId);
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }
                int i = dronesListInBl.FindIndex(x => x.SerialNumber == droneId && x.DroneStatus != DroneStatus.Delete);
                if (i == -1)
                    throw new ItemNotFoundException("Drone", droneId);
                dronesListInBl[i].Model = newName;
                droneInData.Model = newName.covertDroneModelBlToDal();
                dalObj.UpdateDrone(droneInData);
            }
        }

        /// <summary>
        /// search a drone by serial number
        /// </summary>
        /// <param name="droneNum"> serial number of the drone</param>
        /// <returns> drone founded</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(uint droneNum)
        {
            lock (dalObj)
            {
                var drone = dronesListInBl.Find(x => x.SerialNumber == droneNum);
                if (drone is null)
                    throw new ItemNotFoundException("Drone", droneNum);
                try
                {
                    var pacege = drone.NumPackage != 0 ? convertPackegeDalToPackegeInTrnansfer(dalObj.packegeByNumber(drone.NumPackage)) : null;
                    return new Drone
                    {
                        SerialNumber = drone.SerialNumber,
                        Model = drone.Model,
                        WeightCategory = drone.WeightCategory,
                        DroneStatus = drone.DroneStatus,
                        Location = drone.Location.Clone(),
                        ButrryStatus = drone.ButrryStatus.Value,
                        PackageInTransfer = pacege,
                        DistanseToNextLocation = drone.DistanseToNextLocation,
                        LocationNext=drone.LocationNext,
                        LocationName=drone.LocationName
                        
                    };
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }

            }
        }

        /// <summary>
        /// delete drone 
        /// </summary>
        /// <param name="droneNum"> serial number of the drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(uint droneNum)
        {
            lock (dalObj)
            {
                var drone = dronesListInBl.Find(x => x.SerialNumber == droneNum && x.DroneStatus != DroneStatus.Delete);
                if (drone is null)
                    throw new ItemNotFoundException("Drone", droneNum);
                //chack the drone not in middel of delivery
                if (drone.DroneStatus == DroneStatus.Work)
                    throw new DroneStillAtWorkException();

                dalObj.DeleteDrone(droneNum);

                for (int i = 0; i < dronesListInBl.Count; i++)
                {
                    if (dronesListInBl[i].SerialNumber == droneNum)
                    {
                        drone.DroneStatus = DroneStatus.Delete;
                        dronesListInBl[i] = drone;
                    }
                }
            }

        }



    }
}
