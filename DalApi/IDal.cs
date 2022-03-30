using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace DalApi
{
    public interface IDal
    {
        /// <summary>
        /// add drone to list
        /// </summary>
        /// <param name="drone"> drone to add</param>
        public void AddDrone(Drone drone);
        /// <summary>
        /// add package to list
        /// </summary>
        /// <param name="package"> package to add</param>
        /// <returns></returns>
        public uint AddPackage(Package package);

        /// <summary>
        /// add client to list
        /// </summary>
        /// <param name="client"> client to add</param>
        public void AddClient(Client client);

        /// <summary>
        /// add base station to list
        /// </summary>
        /// <param name="base_Station"> base station number</param>
        public void AddStation(Base_Station base_Station);


        /// <summary>
        /// Updated package collected
        /// </summary>
        /// <param name="packageNumber">serial number of the package</param>
        public void PackageCollected(uint packageNumber);

        /// <summary>
        /// connect package to drone
        /// </summary>
        /// <param name="packageNumber">serial number of the package that needs 
        /// to connect to drone </param>
        public void ConnectPackageToDrone(uint packageNumber, uint drone_sirial_number);
        /// <summary>
        /// Update that package has arrived at destination
        /// </summary>
        /// <param name="packageNumber">serial number of the package</param>
        public void PackageArrived(uint packageNumber);

        /// <summary>
        /// Display packege data 
        /// </summary>
        /// <param name="packageNumber">Serial number of a particular package</param>
        /// <returns> string of data</returns>
        public Package packegeByNumber(uint packageNumber);
        /// <summary>
        /// return the list of all packages
        /// </summary>
        public IEnumerable<Package> PackegeList(Predicate<Package> predicate);
      

        /// <summary>
        /// Updating fields of a particular package
        /// </summary>
        /// <param name="package">particular package</param>
        public void UpdatePackege(Package package);
        /// <summary>
        /// delete a spsific packege
        /// </summary>
        /// <param name="sirial"> package number</param>
        public void DeletePackege(uint sirial);

        /// <summary>
        /// Display drone data  
        /// </summary>
        /// <param name="droneNum"> drone number</param>
        /// <returns> String of data</returns>
        public Drone DroneByNumber(uint droneNum);
        /// <summary>
        /// Returns how much electricity the drone needs:
        /// 0. Available, 1. Light weight 2. Medium weight 3. Heavy 4. Charging per minute
        /// </summary>
        public IEnumerable<double> Elctrtricity();
        /// <summary>
        /// update fileds at a given drone
        /// </summary>
        /// <param name="drone"> a given drone</param>
        public void UpdateDrone(Drone drone);

        /// <summary>
        /// delete a spsific drone
        /// </summary>
        /// <param name="sirial"> drone number</param>
        public void DeleteDrone(uint sirial);
        /// <summary>
        /// return list of the drones
        /// </summary>
        /// <returns> return list of the drones</returns>
        public IEnumerable<Drone> DroneList();

        /// <summary>
        /// send drone to charge
        /// </summary>
        /// <param name="drone"> drone number</param>
        /// <param name="base_"> Base station number that the drone will be sent to it </param>
        public void DroneToCharge(uint drone, uint base_);

        /// <summary>
        /// Release a drone from charging
        /// </summary>
        /// <param name="drone"> drone number</param>
        public void FreeDroneFromCharge(uint drone);

        /// <summary>
        /// return a list of all the base stations
        /// </summary>
        public IEnumerable<Base_Station> BaseStationList(Predicate<Base_Station> predicate);
       
        /// <summary>
        /// Display base station data desired   
        /// </summary>
        /// <param name="baseNum">Desired base station number</param>
        /// <returns> String of data </returns>
        public Base_Station BaseStationByNumber(uint baseNum);
        /// <summary>
        /// delete a spsific base for list
        /// </summary>
        /// <param name="sirial"> Base Station number</param>
        public void DeleteBase(uint sirial);
        /// <summary>
        /// update fileds at a given base station
        /// </summary>
        /// <param name="base_"> a given base station </param>
        public void UpdateBase(Base_Station base_);

        /// <summary>
        ///return all clients
        /// </summary>
        /// <returns> list of client</returns>
        public IEnumerable<Client> CilentList(Predicate<Client> predicate);
        /// <summary>
        /// Display client data desired 
        /// </summary>
        /// <param name="id">ID of desired client </param>
        /// <returns> string of data </returns>
        public Client CilentByNumber(uint id);
        /// <summary>
        /// delete a spsific client 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteClient(uint id);
        /// <summary>
        /// update fileds at a given client
        /// </summary>
        /// <param name="client"> a given client </param>
        public void UpdateClient(Client client);


        /// <summary>
        /// show the distance between 2 locations
        /// </summary>
        /// <param name="Longitude1">the first longitude location</param>
        /// <param name="Latitude1"> the first latitude location</param>
        /// <param name="Longitude2">the second longitude location</param>
        /// <param name="Latitude2">the second latitude location</param>
        /// <returns>distance between 2 locations</returns>
        public double Distance(double Longitude1, double Latitude1,
            double Longitude2, double Latitude2);
        /// <summary>
        /// Returns a point in the form of degrees
        /// </summary>
        /// <param name="point"></param>
        public string PointToDegree(double point);

        /// <summary>
        /// return list of charging drones
        /// </summary>
        /// <returns>return list of charging drones</returns>
        public IEnumerable<BatteryLoad> ChargingDroneList(Predicate<BatteryLoad> predicate);


    }
}
