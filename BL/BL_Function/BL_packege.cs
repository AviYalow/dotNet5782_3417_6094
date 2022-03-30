using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;
using DalApi;
using static BL.Cloning;

namespace BlApi
{
    partial class BL : IBL
    {
        /// <summary>
        /// add packege
        /// </summary>
        /// <param name="package"> packege to add</param>
        /// <returns> serial number of the packege</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public uint AddPackege(Package package)
        {
            lock (dalObj)
            {
                uint packegeNum = 0;
                try
                {
                    if (package.Priority > Priority.Regular || package.WeightCatgory > WeightCategories.Heavy)
                        throw new InputErrorException();


                    var send = dalObj.CilentByNumber(package.SendClient.Id);


                    if (!send.Active)
                        throw new ItemNotFoundException("Client", package.SendClient.Id);
                    Location locationsend = new Location { Latitude = send.Latitude, Longitude = send.Longitude };
                    Location locationGet = ClientLocation(package.RecivedClient.Id).Clone();
                    var butrryWithDelvery = buttryDownPackegeDelivery(convertPackegeBlToPackegeInTrnansfer(package));
                    var butrryFree = buttryDownWithNoPackege(ClosestBase(locationsend).Location, locationsend) + buttryDownWithNoPackege(ClosestBase(locationGet).Location, locationGet);
                    if (butrryWithDelvery + butrryFree > 100)
                        throw new MoreDistasThenMaximomException(package.SendClient.Id, package.RecivedClient.Id);




                    packegeNum = dalObj.AddPackage(package.convertPackageBltopackegeDal());
                }
                catch (DO.ItemFoundException ex)
                {
                    throw (new ItemFoundExeption(ex));
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }
                catch (Exception)
                { }


                return packegeNum;
            }

        }

        /// <summary>
        /// Updating fields of a particular package in the data layer
        /// </summary>
        /// <param name="package"> particular package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePackegInDal(Package package)
        {
            lock (dalObj)
            {
                try
                {
                    dalObj.UpdatePackege(package.convertPackageBltopackegeDal());
                }
                catch (DO.ItemNotFoundException ex)
                { throw new ItemNotFoundException(ex); }
                catch (Exception)
                { }
            }
        }

        /// <summary>
        ///  view a package
        /// </summary>
        /// <param name="number">serial number of package</param>
        /// <returns> package in the logical layer</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Package ShowPackage(uint number)
        {
            lock (dalObj)
            {
                try
                {
                    var dataPackege = dalObj.packegeByNumber(number);
                    return convertPackegeDalToBl(dataPackege);
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// delete packege 
        /// </summary>
        /// <param name="number"> serial nummber of package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeletePackege(uint number)
        {
            lock (dalObj)
            {
                try
                {
                    var packege = dalObj.packegeByNumber(number);
                    //cheack the packege not send yet
                    if (packege.PackageAssociation != null)
                    { throw new ThePackegeAlredySendException(); }
                    if (packege.OperatorSkimmerId != 0)
                    {
                        var drone = SpecificDrone(packege.OperatorSkimmerId);
                        drone.DroneStatus = DroneStatus.Free;

                        drone.NumPackage = 0;

                        for (int i = 0; i < dronesListInBl.Count; i++)
                        {
                            if (dronesListInBl[i].SerialNumber == drone.SerialNumber)
                                dronesListInBl[i] = drone;
                        }
                    }
                    dalObj.DeletePackege(number);
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }
                catch (Exception)
                { }
            }
        }

        /// <summary>
        /// convert packege from the data layer to the logical layer
        /// </summary>
        /// <param name="dataPackege"> package in the data layer </param>
        /// <returns>  package in the logical layer</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        Package convertPackegeDalToBl(DO.Package dataPackege)
        {
            lock (dalObj)
            {
                try
                {
                    return new Package
                    {
                        SerialNumber = dataPackege.SerialNumber,
                        SendClient = dalObj.CilentByNumber(dataPackege.SendClient).clientInPackageFromDal(),
                        CollectPackage = dataPackege.CollectPackageForShipment,
                        Create_package = dataPackege.ReceivingDelivery,
                        Drone = dataPackege.OperatorSkimmerId != 0 ? SpecificDrone(dataPackege.OperatorSkimmerId).droneToDroneInPackage() : null,
                        PackageArrived = dataPackege.PackageArrived,
                        PackageAssociation = dataPackege.PackageAssociation,
                        Priority = (Priority)dataPackege.Priority,
                        RecivedClient = dalObj.CilentByNumber(dataPackege.GetingClient).clientInPackageFromDal(),
                        WeightCatgory = (WeightCategories)dataPackege.WeightCatgory
                    };
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }
    }
}
