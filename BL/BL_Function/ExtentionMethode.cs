using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BlApi;
using BO;
using DalApi;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;


namespace BlApi
{
    
      static class ExtentionMethode
        {

        


            /// <summary>
            /// Auxiliary function that converts a base station object from
            /// the data layer to a base station object on the logical layer
            /// </summary>
            /// <param name="baseStation">serial number of the base station</param>
            /// <returns> base station on logical layer </returns>
            internal static BaseStation convertBaseDalToBaseBl(this DO.Base_Station baseStation)
            {
                return new BaseStation
                {
                    SerialNum = baseStation.baseNumber,
                    Name = baseStation.NameBase,
                    FreeState = baseStation.NumberOfChargingStations,
                    Location = new Location { Latitude = baseStation.latitude, Longitude = baseStation.longitude },
                    DronesInChargeList = null
                };
            }

            /// <summary>
            /// convert base from dal to base in list
            /// </summary>
            /// <param name="base_Station"></param>
            /// <returns></returns>
            internal static BaseStationToList convertBaseInDalToBaseStationList(this DO.Base_Station base_Station, IDal dalObj)
            {
                var base_ = new BaseStationToList
                {
                    SerialNum = base_Station.baseNumber,
                    FreeState = base_Station.NumberOfChargingStations,
                    Name = base_Station.NameBase,
                    Active=base_Station.Active?"Active":"Not Active"
                   
                };
                base_.BusyState = (uint)dalObj.ChargingDroneList(x => x.idBaseStation == base_Station.baseNumber).Count();
                return base_;
            }

            internal static ClientToList convertClientDalToClientToList(this DO.Client clientInDal, IDal dalObj)
            {
            return new ClientToList
            {
                ID = clientInDal.Id,
                Name = clientInDal.Name,
                Phone = clientInDal.PhoneNumber,
                Arrived = (uint)dalObj.PackegeList(x => x.SendClient == clientInDal.Id && x.PackageArrived != null).Count(),
                NotArrived = (uint)dalObj.PackegeList(x => x.SendClient == clientInDal.Id && x.PackageArrived == null).Count(),
                OnTheWay = (uint)dalObj.PackegeList(x => x.GetingClient == clientInDal.Id && x.PackageArrived == null).Count(),
                received = (uint)dalObj.PackegeList(x => x.GetingClient == clientInDal.Id && x.PackageArrived != null).Count(),
                Active = clientInDal.Active
                };
            }

            /// <summary>
            /// list of packages
            /// </summary>
            /// <returns>list of packages</returns>
            internal static PackageToList convertPackegeDalToPackegeToList(this DO.Package package, IDal dalObj)
            {
                PackageStatus packageStatus;
                if (package.PackageArrived != null)
                    packageStatus = PackageStatus.Arrived;
                else if (package.CollectPackageForShipment != null)
                    packageStatus = PackageStatus.Collected;
                else if (package.PackageAssociation != null)
                    packageStatus = PackageStatus.Assign;
                else
                    packageStatus = PackageStatus.Create;

            return new PackageToList
            {
                packageStatus = packageStatus,
                RecivedClient = dalObj.CilentByNumber(package.GetingClient).Name,
                SendClient = dalObj.CilentByNumber(package.SendClient).Name,
                SerialNumber = package.SerialNumber,
                priority = (Priority)package.Priority,
                WeightCategories = (WeightCategories)package.WeightCatgory,
                Drone = package.OperatorSkimmerId == 0 ? false : true,
                Create = package.ReceivingDelivery
                
                    

                };

            }

            /// <summary>
            /// convret Packege in the data layer to PackegeAtClient object in the logical layer
            /// </summary>
            /// <param name="package">Packege in the data layer </param>
            /// <param name="client1"> id of the client at the pacage</param>
            /// <returns> PackegeAtClient object</returns>
            internal static PackageAtClient convretPackegeDalToPackegeAtClient(this DO.Package package, uint client1, IDal dal)
            {
                var convert = new PackageAtClient
                {
                    SerialNum = package.SerialNumber,
                    Priority = (Priority)package.Priority,
                    WeightCatgory = (WeightCategories)package.WeightCatgory,

                };
                if (client1 == package.SendClient)
                    convert.Client2 = dal.CilentByNumber(package.GetingClient).clientInPackageFromDal();
                else
                    convert.Client2 = dal.CilentByNumber(package.SendClient).clientInPackageFromDal();

                if (package.PackageArrived != null)
                    convert.PackageStatus = PackageStatus.Arrived;
                else if (package.CollectPackageForShipment != null)
                    convert.PackageStatus = PackageStatus.Collected;
                else if (package.PackageAssociation != null)
                    convert.PackageStatus = PackageStatus.Assign;
                else
                    convert.PackageStatus = PackageStatus.Create;
                return convert;

            }

            /// <summary>
            /// Request a client object from the data layer
            /// </summary>
            /// <param name="id">ID of the client</param>
            /// <returns>client object on the logical layer</returns>
            internal static ClientInPackage clientInPackageFromDal(this DO.Client client)
            {

                return new ClientInPackage { Id = client.Id, Name = client.Name };
            }

       
            /// <summary>
            /// search drone in package
            /// </summary>
            /// <param name="number"> serial number of the drone </param>
            /// <returns>the founded drone</returns>
            internal static DroneInPackage droneToDroneInPackage(this DroneToList drone)
            {

                return new DroneInPackage { SerialNum = drone.SerialNumber, ButrryStatus = drone.ButrryStatus.Value, Location = drone.Location };
            }
            /// <summary>
            /// drone request from the data layer
            /// </summary>
            /// <param name="drone"> serial number of the  drone</param>
            /// <returns> drone </returns>
            internal static DroneToList droneFromDal(this DO.Drone drone)
            {
                return new DroneToList { SerialNumber = drone.SerialNumber, Model = drone.Model.covertDroneModelDalToBl(), WeightCategory = (WeightCategories)drone.WeightCategory };
            }
        internal static DO.Package convertPackageBltopackegeDal(this Package package)
        {
            return new DO.Package
            {
                SerialNumber = package.SerialNumber,
                SendClient = package.SendClient.Id,
                CollectPackageForShipment = package.CollectPackage,
                ReceivingDelivery = package.Create_package,
                OperatorSkimmerId =(package.Drone is null)?0: package.Drone.SerialNum,
                PackageArrived = package.PackageArrived,
                PackageAssociation = package.PackageAssociation,
                Priority = (DO.Priority)package.Priority,
                GetingClient = package.RecivedClient.Id,
                WeightCatgory = (DO.WeightCategories)package.WeightCatgory
            };
        }
           
        internal static DO.DroneModel covertDroneModelBlToDal(this DroneModel drone)
        {
            return (DO.DroneModel)drone;
        }
        internal static DroneModel covertDroneModelDalToBl(this DO. DroneModel drone)
        {
            return (DroneModel)drone;
        }

    }
    
}
