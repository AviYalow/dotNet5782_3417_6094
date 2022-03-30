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
        /// convert Packege object to PackegeInTrnansfer object
        /// </summary>
        /// <param name="package">Packege object </param>
        /// <returns> PackegeInTrnansfer object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public PackageInTransfer convertPackegeBlToPackegeInTrnansfer(Package package)
        {
            lock (dalObj)
            {
                var returnPackege = new PackageInTransfer { Priority = package.Priority, SendClient = package.SendClient, RecivedClient = package.RecivedClient, SerialNum = package.SerialNumber, WeightCatgory = package.WeightCatgory, Source = ClientLocation(package.SendClient.Id), Destination = ClientLocation(package.RecivedClient.Id) };
            returnPackege.Distance = Distans(returnPackege.Source, returnPackege.Destination);
            returnPackege.InTheWay = (package.PackageArrived != new DateTime()) ? true : false;
            return returnPackege;
        }
        }

        /// <summary>
        /// A package is collected by a drone
        /// </summary>
        /// <param name="droneNumber">A drone number that collects the package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CollectPackegForDelivery(uint droneNumber,double distanse=0)
        {
            lock (dalObj)
            {
                var drone = dronesListInBl.Find(x => x.SerialNumber == droneNumber && x.DroneStatus != DroneStatus.Delete);
            if (drone == null)
                throw new ItemNotFoundException("Drone", droneNumber);
            try
            {
                    var packegeDal = dalObj.packegeByNumber(drone.NumPackage);
                var pacege = convertPackegeDalToPackegeInTrnansfer(packegeDal);
                if (pacege.InTheWay != false&& packegeDal.PackageArrived!=null)
                {throw new FunctionErrorException("ShowPackage||AddPackege"); }

                Location location = ClientLocation(pacege.SendClient.Id).Clone();

                drone.ButrryStatus -= distanse == 0? buttryDownWithNoPackege(drone.Location, location): buttryDownWithNoPackege(distanse);

                if (drone.ButrryStatus < 0)
                { new FunctionErrorException("BatteryCalculationForFullShipping"); }

                drone.Location = location;
                    drone.LocationName = LocationName.SendClient;
                    drone.LocationNext = LocationNext.GetinClient;
                    drone.DistanseToNextLocation = Distans(location, ClientLocation(pacege.RecivedClient.Id));
                dalObj.PackageCollected(pacege.SerialNum);
                dronesListInBl[dronesListInBl.FindIndex(x => x.SerialNumber == droneNumber)] = drone;
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
        }
        }

        /// <summary>
        /// A package that arrived at the destination
        /// </summary>
        /// <param name="droneNumber">A drone number that takes the package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PackegArrive(uint droneNumber, double distanse = 0)
        {
            lock (dalObj)
            {
                var drone = dronesListInBl.Find(x => x.SerialNumber == droneNumber && x.DroneStatus != DroneStatus.Delete);
            if (drone == null)
                throw new ItemNotFoundException("Drone", droneNumber);
            try
            {
                var packege = convertPackegeDalToPackegeInTrnansfer(dalObj.packegeByNumber(drone.NumPackage));
                if (packege.InTheWay == false)
                    throw new PackegeNotAssctionOrCollectedException();
            
                drone.ButrryStatus -=buttryDownPackegeDelivery(packege,distanse);
                

                drone.Location = ClientLocation(packege.RecivedClient.Id).Clone();
                    drone.LocationName = LocationName.EndDelviry;
                    drone.LocationNext = LocationNext.None;
                    drone.DistanseToNextLocation = 0;
                    drone.DroneStatus = DroneStatus.Free;
                drone.NumPackage = 0;
                packege.InTheWay = false;
                dalObj.PackageArrived(packege.SerialNum);
                dronesListInBl[dronesListInBl.FindIndex(x => x.SerialNumber == droneNumber)] = drone;
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
                catch(Exception)
                { }
        }
        }

        /// <summary>
        /// Assignment between a package and a drone
        /// </summary>
        /// <param name="droneNumber"> serial number of a drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ConnectPackegeToDrone(uint droneNumber)
        {
            lock (dalObj)
            {
                try
                {
                    var drone = dronesListInBl.Find(x => x.SerialNumber == droneNumber && x.DroneStatus != DroneStatus.Delete);
                    if (drone is null)
                    { throw new ItemNotFoundException("Drone", droneNumber); }
                    if (drone.DroneStatus != DroneStatus.Free)
                    { throw new DroneCantMakeDliveryException(); }


                    DO.Package? returnPackege = null;
                    foreach
                    (var packege in dalObj.PackegeList(x => x.OperatorSkimmerId == 0 &&
                    batteryCalculationForFullShipping(drone.Location, convertPackegeDalToBl(x)) < drone.ButrryStatus &&
                    (WeightCategories)x.WeightCatgory <= drone.WeightCategory))

                    {
                        if (returnPackege is null)
                            returnPackege = packege;
                        if (returnPackege?.Priority < packege.Priority)
                            returnPackege = packege;
                        else if (returnPackege?.Priority == packege.Priority)
                            if (returnPackege?.WeightCatgory < packege.WeightCatgory)
                                returnPackege = packege;
                            else if (returnPackege?.WeightCatgory == packege.WeightCatgory)
                                if (Distans(drone.Location, ClientLocation(packege.SendClient)) < Distans(drone.Location, ClientLocation(returnPackege.Value.SendClient)))
                                    returnPackege = packege;

                    }
                    if (returnPackege is null)
                        throw new DroneCantMakeDliveryException();
                    drone.NumPackage = returnPackege.Value.SerialNumber;
                    
                    drone.LocationNext = LocationNext.SendClient;
                    drone.DistanseToNextLocation = Distans(drone.Location,ClientLocation(returnPackege.Value.SendClient));
                    drone.DroneStatus = DroneStatus.Work;
                    try
                    {
                        dalObj.ConnectPackageToDrone(returnPackege.Value.SerialNumber, droneNumber);
                    }
                    catch (DO.ItemNotFoundException ex)
                    {
                        throw new ItemNotFoundException(ex);
                    }

                    for (int i = 0; i < dronesListInBl.Count; i++)
                    {
                        if (dronesListInBl[i].SerialNumber == drone.SerialNumber)
                        {
                            dronesListInBl[i] = drone;
                            break;
                        }
                    }
                }
                catch(DroneCantMakeDliveryException)
                { throw new DroneCantMakeDliveryException(); }
                catch (Exception)
                { }
        }

        }

        /// <summary>
        ///  convert packege in data layer to packegeInTrnansfer object in the logical layer
        /// </summary>
        /// <param name="package"> packege in data layer</param>
        /// <returns> packegeInTrnansfer object in the logical layer</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        PackageInTransfer convertPackegeDalToPackegeInTrnansfer(DO.Package package)
        {
            lock (dalObj)
            {
                try
                {
                    var returnPackege = new PackageInTransfer
                    {
                        SerialNum = package.SerialNumber,
                        WeightCatgory = (WeightCategories)package.WeightCatgory,
                        Priority = (Priority)package.Priority,
                        Source = ClientLocation(package.SendClient).Clone(),
                        Destination = ClientLocation(package.GetingClient).Clone(),
                        SendClient = dalObj.CilentByNumber(package.SendClient).clientInPackageFromDal(),
                        RecivedClient = dalObj.CilentByNumber(package.GetingClient).clientInPackageFromDal()
                    };
                    returnPackege.Distance = Distans(returnPackege.Source, returnPackege.Destination);
                    returnPackege.InTheWay = (package.PackageArrived is null&&package.CollectPackageForShipment!=null && package.OperatorSkimmerId != 0) ? true : false;
                    return returnPackege;
                }
                catch(DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }
                catch(Exception)
                { return null; }
        }
        }

    }
}
