using System;
using System.Collections;
using System.Collections.Generic;
using DalApi;
using BO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections.ObjectModel;
using BL;

namespace BlApi
{
   internal sealed partial class BL : IBL
    {

        #region singelton
        /// <summary>
        /// This implemention of singelton implicitly uses LazyThreadSafetyMode.ExecutionAndPublication 
        /// as the thread safety mode for the Lazy<Singleton>. 
        /// </summary>

        private static readonly Lazy<BL> lazy =
        new Lazy<BL>(() => new BL());
        internal static BL Instance { get { return lazy.Value; } }
        private Simulator simulator;
        #endregion


       internal IDal dalObj;
      internal  List<DroneToList> dronesListInBl = new List<DroneToList>();

      internal  double heaviElctric, mediomElctric, easyElctric, freeElctric, chargingPerMinute;
        event Func<DroneToList, bool> droneToListFilter = null;
        event Func<ClientToList, bool> clientToListFilter = null;
        event Func<DO.Package, bool> packegeToListFilter = null;


        /// <summary>
        /// ctor
        /// </summary>
        private BL()
        {
            try
            {
                try
                {
                    dalObj = DalFactory.GetDal();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return;
                }
                lock (dalObj)
                {
                    List<double> electric = new List<double>();
                    foreach (var elctriv in dalObj.Elctrtricity())
                    {
                        electric.Add(elctriv);
                    }

                    freeElctric = electric[0];
                    easyElctric = electric[1];
                    mediomElctric = electric[2];
                    heaviElctric = electric[3];
                    chargingPerMinute = electric[4];

                    Random random = new Random();


                    //Copy the drone list from the data layer to the logic layer
                    foreach (DO.Drone drone in dalObj.DroneList())
                    {
                        dronesListInBl.Add(drone.droneFromDal());
                    }


                    for (int i = 0; i < dronesListInBl.Count; i++)
                    {
                        //data from drone list in data source
                        DroneToList new_drone = dronesListInBl[i];


                        //Checking for packege connected to this drone

                        DO.Package package = new DO.Package();
                        foreach (DO.Package chcking_packege in dalObj.PackegeList(x => x.OperatorSkimmerId > 0))
                        {
                            //Check if the package is associated with this drone
                            if (chcking_packege.OperatorSkimmerId == new_drone.SerialNumber)
                            {

                                //cheak if the drone has not get to its destination yet
                                if (chcking_packege.PackageArrived == null)
                                {
                                    new_drone.NumPackage = chcking_packege.SerialNumber;
                                    package = chcking_packege;
                                    new_drone.DroneStatus = DroneStatus.Work;

                                    break;
                                }
                            }
                        }

                        if (new_drone.DroneStatus == DroneStatus.Work)
                        {

                            double min_butrry;
                            var packegeInTransferObject = convertPackegeDalToPackegeInTrnansfer(dalObj.packegeByNumber(new_drone.NumPackage));
                            //If the package has not been collected yet
                            //The location of the drone will be at the sender location
                            if (package.CollectPackageForShipment != null)
                            {
                                new_drone.LocationName = LocationName.SendClient;
                                new_drone.LocationNext = LocationNext.GetinClient;
                                new_drone.Location = ClientLocation(package.SendClient);
                                new_drone.NumPackage = package.SerialNumber;
                                min_butrry = buttryDownPackegeDelivery(packegeInTransferObject) +
                                   buttryDownWithNoPackege(ClosestBase(ClientLocation(package.GetingClient)).Location, ClientLocation(package.GetingClient));
                                new_drone.ButrryStatus = random.Next((int)min_butrry + 1, 100);
                                new_drone.DistanseToNextLocation = Distans(new_drone.Location, ClientLocation(package.GetingClient));
                            }

                            //If the package was associated but not collected the location
                            //Of the drone will be at the station closest to the sender
                            else if (package.PackageAssociation != null)
                            {
                                new_drone.LocationName = LocationName.Base;
                                new_drone.LocationNext = LocationNext.SendClient;
                                new_drone.Location = ClosestBase(ClientLocation(package.SendClient)).Location;
                                min_butrry = buttryDownWithNoPackege(new_drone.Location, ClientLocation(package.SendClient)) +
                                    buttryDownPackegeDelivery(packegeInTransferObject) +
                                    buttryDownWithNoPackege(ClosestBase(ClientLocation(package.GetingClient)).Location, ClientLocation(package.GetingClient));
                                Client send = new();
                                Client get = new();
                                while(min_butrry>100)
                                {
                                     send = GetingClient((uint)random.Next(ClientToLists().Count()));
                                     get= GetingClient((uint)random.Next(ClientToLists().Count()));
                                    min_butrry = Distans(send.Location, get.Location);
                                    package.SendClient = send.Id;
                                    package.GetingClient = get.Id;
                                    dalObj.UpdatePackege(package);
                                    new_drone.Location = ClosestBase(ClientLocation(package.SendClient)).Location;
                                }
                                new_drone.ButrryStatus = random.Next((int)min_butrry + 1, 100);
                                new_drone.DistanseToNextLocation = Distans(new_drone.Location, ClientLocation(package.SendClient));

                            }


                        }

                        //If the drone doesn't in delivery
                        else
                        {
                            new_drone.DroneStatus = (DroneStatus)random.Next(2);

                            //dorne lottery in mainteance
                            if (new_drone.DroneStatus == DroneStatus.Maintenance)
                            {
                                new_drone.LocationName = LocationName.Base;
                                new_drone.LocationNext = LocationNext.None;
                                new_drone.ButrryStatus = random.Next(21);
                                DO.Base_Station base_;
                                int safe = 5;
                                do
                                {
                                    safe--;
                                    int k = random.Next(dalObj.BaseStationList(x => true).Count());

                                    base_ = dalObj.BaseStationList(x => true).Skip(k).FirstOrDefault();
                                } while (base_.NumberOfChargingStations <= 0 || safe > 0);
                                var updateBase = base_;
                                dalObj.DroneToCharge(new_drone.SerialNumber, base_.baseNumber);
                                new_drone.NumPackage = 0;
                                new_drone.Location = BaseLocation(base_.baseNumber);
                                new_drone.DistanseToNextLocation = 0;
                            }

                            //drone lottery in free
                            else if (new_drone.DroneStatus == DroneStatus.Free)
                            {

                                new_drone.LocationNext = LocationNext.None;
                                //lottry drone what is lost shipment
                                int k = random.Next(dalObj.PackegeList(x => x.PackageArrived != null).Count());
                                DO.Package package1 = dalObj.PackegeList(x => x.PackageArrived != null).Skip(k).FirstOrDefault();
                                new_drone.NumPackage = 0;
                                new_drone.Location = ClientLocation(package1.GetingClient);
                                new_drone.LocationName = LocationName.EndDelviry;
                                var min_butrry = buttryDownWithNoPackege(new_drone.Location, ClosestBase(ClientLocation(package1.GetingClient)).Location) + 1;
                                Client send = new();
                                Client get = new();
                                while (min_butrry > 100)
                                {
                                    send = GetingClient((uint)random.Next(ClientToLists().Count()));
                                    get = GetingClient((uint)random.Next(ClientToLists().Count()));
                                    min_butrry = Distans(send.Location, get.Location);
                                    package.SendClient = send.Id;
                                    package.GetingClient = get.Id;
                                    dalObj.UpdatePackege(package);
                                    new_drone.Location = ClosestBase(ClientLocation(package.SendClient)).Location;
                                }
                                new_drone.ButrryStatus = random.Next((int)buttryDownWithNoPackege(new_drone.Location, ClosestBase(ClientLocation(package1.GetingClient)).Location) + 1, 100);
                                new_drone.DistanseToNextLocation = 0;
                            }
                            for (int p = 0; p < dronesListInBl.Count; p++)
                            {
                                if (dronesListInBl[p].SerialNumber == new_drone.SerialNumber)
                                    dronesListInBl[p] = new_drone;
                            }

                        }

                    }

                }
            }
            catch(Exception)
            { }

        }

        /// <summary>
        /// show the distance between 2 locations
        /// </summary>
        /// <param name="location1">the first location</param>
        /// <param name="location2"> the second location</param>
        /// <returns> distance </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Distans(Location location1, Location location2)
        {
            lock (dalObj)
            {
                return dalObj.Distance(location1.Longitude, location1.Latitude, location2.Longitude, location2.Latitude);
            }
        }

        /// <summary>
        /// Returns a point in the form of degrees
        /// </summary>
        /// <param name="point"> a point</param>
        /// <returns>point in the form of degrees</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string PointToDegree(double point)
        {
            lock (dalObj)
            {
                return dalObj.PointToDegree(point);
            }
        }

        /// <summary>
        /// sort list by spscific parameter
        /// </summary>
        /// <param name="obj">ordenry list by this parameter parameter </param>
        /// <param name="drones">ordener this list </param>
        /// <returns>ordenry list</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<T> SortList<T>(string obj, IEnumerable<T> drones = null)
        {
            
            
                PropertyInfo property = typeof(T).GetProperty(obj);
                if (property is null)
                    throw new TheListIsEmptyException();
                return from x in drones
                       orderby property.GetValue(x)
                       select x.Clone();
            

        }
        /// <summary>
        /// filter any list by getting event and list
        /// </summary>
        /// <typeparam name="T"> genric object to return</typeparam>
        /// <param name="list">full list </param>
        /// <param name="func">event func</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<T> filerList<T>(IEnumerable<T> list, Func<T, bool> func)
        {
           
            lock (dalObj)
            {
                if (func != null && func.GetInvocationList() != null)
                    foreach (Func<T, bool> prdict in func.GetInvocationList())
                    {
                        list = list.Where(prdict);
                    }

                return list;
            }
        }

       public void PlayThred(uint droneNumber, Action action, Func<bool> StopChecking)
        {

            try
            {
                simulator = new Simulator(this, droneNumber, action, StopChecking);
            }
          
            catch (DroneTryToStartSecondeSimolatorException)
            { throw new DroneTryToStartSecondeSimolatorException(droneNumber); }
            catch (Exception) { }

        }
    }
}
