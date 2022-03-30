using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using System.Runtime.CompilerServices;
using DalApi;

namespace BlApi
{
    public interface IBL
    {
        #region Threading
        /// <summary>
        /// Running a simulator
        /// </summary>
        /// <param name="droneNumber"></param>
        /// <param name="action"></param>
        /// <param name="StopChecking"></param>
        public void PlayThred(uint droneNumber, Action action, Func<bool> StopChecking);
        #endregion

        #region Creat
        /// <summary>
        /// add drone to list
        /// </summary>
        /// <param name="drone"> drone to add</param>
        /// <param name="base_"> serial number of base station for first chraging</param>
        public void AddDrone(DroneToList drone, uint base_);

        /// <summary>
        /// add base station
        /// </summary>
        /// <param name="baseStation"> serial number of the base station</param>
        public void AddBase(BaseStation baseStation);

        /// <summary>
        /// add client
        /// </summary>
        /// <param name="client"> client to add</param>
        public void AddClient(Client client);

        /// <summary>
        /// add packege
        /// </summary>
        /// <param name="package"> packege to add</param>
        /// <returns> serial number of the packege</returns>
        public uint AddPackege(Package package);

        #endregion

        #region Update

        #region Drone
        /// <summary>
        ///  send drone to charge
        /// </summary>
        /// <param name="dronenumber">serial number of drone</param>
        public void DroneToCharge(uint dronenumber, double? distanse = null);

        /// <summary>
        /// update new location for drone
        /// </summary>
        /// <param name="drone"> serial number of drone</param>
        /// <param name="location"> new location</param>
        public void UpdateDronelocation(uint drone, Location location);
        /// <summary>
        /// update new model for drone
        /// </summary>
        /// <param name="droneId"> serial number of the drone</param>
        /// <param name="newName"> new name to change</param>
        public void UpdateDroneName(uint droneId, DroneModel newName);

        /// <summary>
        /// Release a drone from charging
        /// </summary>
        /// <param name="droneNumber">serial number of the drone</param>
        /// <param name="timeInCharge"> the time that the drone in charge </param>
        /// <returns> butrry Status of the  drone</returns>
        public double FreeDroneFromCharging(uint droneNumber, double number = -1);

        /// <summary>
        /// Assignment between a package and a drone
        /// </summary>
        /// <param name="droneNumber"> serial number of a drone</param>
        public void ConnectPackegeToDrone(uint droneNumber);
        #endregion
        #region Packege
        /// <summary>
        /// A package that arrived at the destination
        /// </summary>
        /// <param name="droneNumber">A drone number that takes the package</param>
        public void PackegArrive(uint droneNumber, double distanse = 0);

        /// <summary>
        /// A package is collected by a drone
        /// </summary>
        /// <param name="droneNumber">A drone number that collects the package</param>
        public void CollectPackegForDelivery(uint droneNumber, double distanse = 0);

        //
        /// <summary>
        /// Updating fields of a particular package in the data layer
        /// </summary>
        /// <param name="package"> particular package</param>
        public void UpdatePackegInDal(Package package);

        //


        #endregion
        #region Base station
        /// <summary>
        /// Release a drone from a charger at a particular base station
        /// </summary>
        /// <param name="baseNumber"> serial number of the base station</param>
        /// <param name="number"> amount of drone to release</param>
        public void FreeBaseFromDrone(uint baseNumber, int number);

        /// <summary>
        /// update base station
        /// </summary>
        /// <param name="base_">erial number of the base station</param>
        /// <param name="newName"> new name</param>
        /// <param name="newNumber"> charging states</param>
        public void UpdateBase(uint base_, string newName, string newNumber);

        #endregion

        #region clients
        /// <summary>
        /// Update fields at a client
        /// </summary>
        /// <param name="client"> client </param>
        public void UpdateClient(Client client);
        #endregion

        #endregion

        #region Request
        /// <summary>
        /// show the distance between 2 locations
        /// </summary>
        /// <param name="location1">the first location</param>
        /// <param name="location2"> the second location</param>
        /// <returns> distance </returns>
        public double Distans(Location location1, Location location2);
        /// <summary>
        /// Returns a point in the form of degrees
        /// </summary>
        /// <param name="point"> a point</param>
        /// <returns>point in the form of degrees</returns>
        public string PointToDegree(double point);
        /// <summary>
        /// sort list by spscific parameter
        /// </summary>
        /// <param name="obj">ordenry list by this parameter parameter </param>
        /// <param name="drones">ordener this list </param>
        /// <returns>ordenry list</returns>
        public IEnumerable<T> SortList<T>(string obj, IEnumerable<T> drones);
        #region Drone
        /// <summary>
        /// find specific drone in the list of the drones
        /// </summary>
        /// <param name="siralNuber"> serial number of the drone</param>
        /// <returns> drone founded </returns>
        public DroneToList SpecificDrone(uint siralNuber);
        /// <summary>
        /// return list of drones
        /// </summary>
        /// <returns> return list of drones</returns>
        public IEnumerable<DroneToList> DroneToLists();
        /// <summary>
        /// return list of drones by they can make delivery for packege
        /// </summary>
        /// <returns> return list of drones</returns>
        public IEnumerable<DroneToList> DroneToListPasibalForPackege(Package package);
        /// <summary>
        /// return list of drones by maximum weight
        /// </summary>
        /// <returns> return list of drones</returns>
        public IEnumerable<DroneToList> DroneToListsByWhight(WeightCategories? weight = null);
        /// <summary>
        /// return list of drones by spsific status
        /// </summary>
        /// <returns> return list of drones</returns>
        public IEnumerable<DroneToList> DroneToListsByStatus(DroneStatus? droneStatus = null);

        /// <summary>
        /// return filter list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneToList> FilterDronesList();

        public IEnumerable<DroneInCharge> DroneINChargePerBase(uint base_);
        /// <summary>
        /// return all drone thier sirial number start with num
        /// </summary>
        /// <param name="num">parmeter of chcing</param>
        /// <returns></returns>
        public IEnumerable<DroneToList> DroneToListFilterByNumber(string num);

        /// <summary>
        /// return all drone include unactiv drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneToList> AllDroneToLists();

        public IEnumerable<uint> DronesNumber();

        /// <summary>
        /// search a drone by serial number
        /// </summary>
        /// <param name="droneNum"> serial number of the drone</param>
        /// <returns> drone founded</returns>
        public Drone GetDrone(uint droneNum);

        #endregion
        #region Packege
        /// <summary>
        /// list of packages
        /// </summary>
        /// <returns>list of packages</returns>
        public IEnumerable<PackageToList> PackageToLists();
        /// <summary>
        /// packege thet connect to drone but not collected
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PackageToList> PackageConnectedButNutCollectedLists(bool filterPackege = true);
        /// <summary>
        /// packege thier collected but not arrive
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PackageToList> PackageCollectedButNotArriveLists(bool filterPackege = true);
        /// <summary>
        /// packege they get to the resive client
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PackageToList> PackageArriveLists(bool filterPackege = true);
        /// <summary>
        /// packeges thir only crate
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PackageToList> PackageWithNoDroneToLists(bool filterPackege = true);

        /// <summary>
        /// filter packeg list by weight
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public IEnumerable<PackageToList> PackageWeightLists(WeightCategories? weight = null);
        /// <summary>
        /// filter list by priority
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        public IEnumerable<PackageToList> PackagePriorityLists(Priority? priority = null);

        public IEnumerable<PackageToList> PackegeBySpsificStatus(PackageStatus? status = null);

        public IEnumerable<PackageToList> PackageToDateLists(DateTime? date = null);

        public IEnumerable<PackageToList> PackageFromDateLists(DateTime? date = null);
        /// <summary>
        ///  view a package
        /// </summary>
        /// <param name="number">serial number of package</param>
        /// <returns> package in the logical layer</returns>
        public Package ShowPackage(uint number);
        #endregion
        #region Base station
        /// <summary>
        /// calculation the most collset base station to a particular location
        /// </summary>
        /// <param name="location"> particular location</param>
        /// <returns> the most collset base station </returns>
        public BaseStation ClosestBase(Location location, bool toCharge = false);

        /// <summary>
        /// geting location for specific base station
        /// </summary>
        /// <param name="base_number"> serial number of base station</param>
        /// <returns> Location of the base station</returns>
        public Location BaseLocation(uint base_number);

        /// <summary>
        /// show base station list 
        /// </summary>
        /// <returns> base station list </returns>
        public IEnumerable<BaseStationToList> BaseStationToLists();
        /// <summary>
        /// return base station with free place for drone
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseStationToList> BaseStationWhitFreeChargingStationToLists();
        /// <summary>
        /// return All base station include not activ base
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseStationToList> AllBaseStation();
        /// <summary>
        /// return base wite spsific drone 
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        public BaseStationToList BaseStationWhitSpscificDrone(uint drone);

        /// <summary>
        /// search a specific station
        /// </summary>
        /// <param name="baseNume"> serial number</param>
        /// <returns> base station </returns>
        public BaseStation BaseByNumber(uint baseNume);
        #endregion

        #region clients

        /// <summary>
        /// return the client location
        /// </summary>
        /// <param name="id"> id client</param>
        /// <returns> client location</returns>
        public Location ClientLocation(uint id);

        /// <summary>
        /// list of clients
        /// </summary>
        /// <returns> list of clients</returns>
        public IEnumerable<ClientToList> ClientToLists(bool filter = true);

        /// <summary>
        /// IEnumerable of client how need to get packege and they not get 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientToList> ClientActiveHowGetingPackegesToLists(bool filter = true);

        /// <summary>
        /// IEnumerable of client how need to get packege and they get 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientToList> ClientActiveHowGetingPackegesAndArriveToLists(bool filter = true);

        /// <summary>
        /// IEnumerable of client how need to get packege 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientToList> ClientActiveHowGetingPackegesAndNotArriveToLists(bool filter = true);

        /// <summary>
        /// IEnumerable of client how send packege and not arrive
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientToList> ClientActiveHowSendPackegesAndNotArriveToLists(bool filter = true);

        /// <summary>
        /// IEnumerable of client how send packege and arrive
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientToList> ClientActiveHowSendAndArrivePackegesToLists(bool filter = true);

        /// <summary>
        /// IEnumerable of client how send packege
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientToList> ClientActiveHowSendPackegesToLists(bool filter = true);

        /// <summary>
        /// return Id's client list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<uint> ClientById(string id);

        /// <summary>
        /// return client list by filter
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public IEnumerable<ClientToList> FilterClientList(bool active = true);

        /// <summary>
        /// return client list it clientInPackege type
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<ClientInPackage> ClientInPackagesList(bool filter = true);

        /// <summary>
        /// Receiving a client by ID
        /// </summary>
        /// <param name="id"> client ID</param>
        /// <returns> client</returns>
        public Client GetingClient(uint id);

        #endregion

        #endregion

        #region Delete
        /// <summary>
        /// delete drone 
        /// </summary>
        /// <param name="droneNum"> serial number of the drone</param>
        public void DeleteDrone(uint droneNum);

        /// <summary>
        /// delete base station
        /// </summary>
        /// <param name="base_"> serial number</param>
        public void DeleteBase(uint base_);

        /// <summary>
        /// delete client
        /// </summary>
        /// <param name="id"> client id</param>
        public void DeleteClient(uint id);

        /// <summary>
        /// delete packege 
        /// </summary>
        /// <param name="number"> serial nummber of package</param>
        public void DeletePackege(uint number);
        #endregion

     

    }
}
