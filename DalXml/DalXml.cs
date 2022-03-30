using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{

    sealed class DalXml : IDal
    {
        //Using a design pattern of singelton
        #region singelton
        private static readonly Lazy<DalXml> lazy = new Lazy<DalXml>(() => new DalXml());
        static readonly DalXml instance = new DalXml();
        /// <summary>
        /// static constructor to ensure instance init is done just before first use
        /// </summary>
        static DalXml() { //DataSrc.Initialize();

            
          
            
        }
        /// <summary>
        ///  constructor. default => private
        /// </summary>
        DalXml() {
            List<BatteryLoad> droneInCharge = XMLTools.LoadListFromXMLSerializer<BatteryLoad>(@"BatteryLoadXml.xml");
            foreach (var drone in droneInCharge)
            {
                FreeDroneFromCharge(drone.IdDrone);
            }
            droneInCharge.Clear();
            XMLTools.SaveListToXMLSerializer(droneInCharge, @"BatteryLoadXml.xml");
        }
        /// <summary>
        /// the public Instance property for use. returns the instance
        /// </summary>
        public static DalXml Instance { get { return lazy.Value; } }// The public Instance property to use

        #region DS XML Files Path
        /// <summary>
        /// BaseStation XMLSerializer
        /// </summary>
        string BaseStationPath = @"base_StationXml.xml";
        /// <summary>
        /// Clients XElement
        /// </summary>
        string ClientsPath = @"ClientXml.xml";
        /// <summary>
        /// Drones XMLSerializer
        /// </summary>
        string DronesPath = @"DroneXml.xml";

        /// <summary>
        /// Packages XMLSerializer
        /// </summary>
        string PackagesPath = @"PackageXml.xml";
        /// <summary>
        /// Drones in charge XMLSerializer
        /// </summary>
        string DroneInChargePath = @"BatteryLoadXml.xml";

        #endregion
        #region function
        #region Add
        /// <summary>
        /// Adding a new client
        /// </summary>
        /// <param name="client"> client to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddClient(Client client)
        {
           
            XElement ClientsRootElem = XMLTools.LoadListFromXMLElement(ClientsPath);

            XElement err = (from item in ClientsRootElem.Elements()
                           where( (item.Element("Id").Value.CompareTo(client.Id.ToString())==0 ) && ( item.Element("Active").Value.CompareTo("true") == 0))
                           select item).FirstOrDefault();

            if(err!=null)
                throw (new ItemFoundException("Client", client.Id));

            XElement ClientAdd = new XElement("Client", new XElement("Id", client.Id.ToString()),
                new XElement("Name", client.Name),
                new XElement("PhoneNumber", client.PhoneNumber),
                new XElement("Longitude", client.Longitude.ToString()),
                 new XElement("Latitude", client.Latitude.ToString()),
                  new XElement("Active", client.Active.ToString())
                );
            ClientsRootElem.Add(ClientAdd);

          

            XMLTools.SaveListToXMLElement(ClientsRootElem, ClientsPath);
        }
        /// <summary>
        /// Adding a new drone
        /// </summary>
        /// <param name="drone">drone to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            if (drones.Any(x => x.SerialNumber == drone.SerialNumber))
                throw (new ItemFoundException("drone", drone.SerialNumber));
            int i = drones.FindIndex(x => x.SerialNumber == drone.SerialNumber);
            if (i == -1)
                drones.Add(new Drone()
                {
                    SerialNumber = drone.SerialNumber,
                    Model = drone.Model,
                    WeightCategory = (WeightCategories)drone.WeightCategory,
                    Active = true

                });
            else
                drones[i] = drone;

            XMLTools.SaveListToXMLSerializer(drones, DronesPath);

        }
        /// <summary>
        /// Adding a new package
        /// </summary>
        /// <param name="package"></param>
        /// <returns>Returns the serial number of the new package</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public uint AddPackage(Package package)
        {
            package.ReceivingDelivery = DateTime.Now;
            package.CollectPackageForShipment = null;
            package.PackageArrived = null;
            package.PackageAssociation = null;

            List<Package> packages = XMLTools.LoadListFromXMLSerializer<Package>(PackagesPath);

            List<string> config = XMLTools.LoadListFromXMLSerializer<string>(@"DalXmlConfig.xml");
            uint package_num = uint.Parse(config[5]);
            if (packages.Any(x => x.SerialNumber == package_num))
                throw new ItemFoundException("Packege", package_num);

            packages.Add(new Package
            {
                SerialNumber = package_num,
                SendClient = package.SendClient,
                GetingClient = package.GetingClient,
                Priority = package.Priority,
                ReceivingDelivery = package.ReceivingDelivery,
                WeightCatgory = package.WeightCatgory,
                OperatorSkimmerId = 0,
                CollectPackageForShipment = package.CollectPackageForShipment,
                PackageArrived = package.PackageArrived,
                PackageAssociation = package.PackageAssociation
            });

            package_num++;

            config[5] = package_num.ToString();

            XMLTools.SaveListToXMLSerializer(config, @"DalXmlConfig.xml");
            XMLTools.SaveListToXMLSerializer(packages, PackagesPath);
            return package_num - 1;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        /// <summary>
        ///Adding a new base station
        /// </summary>
        /// <param name="base_num">The base station number </param>
        /// <param name="name"> The name ot the station </param>
        /// <param name="numOfCharging">The amount of charging stations at the station </param>
        /// <param name="latitude">Latitude of the station</param>
        /// <param name="longitude">Longitude of the station</param>
        public void AddStation(Base_Station base_Station)
        {
            List<Base_Station> base_Stations = XMLTools.LoadListFromXMLSerializer<Base_Station>(BaseStationPath);
            if (base_Stations.Any(x => x.baseNumber == base_Station.baseNumber))
                throw (new ItemFoundException("base station", base_Station.baseNumber));
            base_Stations.Add(base_Station);

            XMLTools.SaveListToXMLSerializer(base_Stations, BaseStationPath);

        }
        #endregion
        #region Update
        /// <summary>
        /// Release a drone from charging
        /// </summary>
        /// <param name="drone"> drone number</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void FreeDroneFromCharge(uint drone)
        {
            List<BatteryLoad> droneInCharge = XMLTools.LoadListFromXMLSerializer<BatteryLoad>(DroneInChargePath);

            BatteryLoad? droneInChargeItem = droneInCharge.Find(x => x.IdDrone == drone);
            if (droneInChargeItem is null)
                throw (new ItemNotFoundException("drone", drone));
            Base_Station base_ = BaseStationByNumber(droneInChargeItem.Value.idBaseStation);
            if (base_.baseNumber == 0)
                throw (new ItemNotFoundException("Base Station", droneInChargeItem.Value.idBaseStation));
            base_.NumberOfChargingStations++;
            UpdateBase(base_);

            droneInCharge.Remove(droneInChargeItem.Value);

            //  int i = droneInCharge.FindIndex(x => x.IdDrone == drone);
            //  if (i == -1)
            //      throw (new ItemNotFoundException("drone", drone));
            //      droneInCharge.RemoveAt(i);
            XMLTools.SaveListToXMLSerializer(droneInCharge, DroneInChargePath);
        }
        /// <summary>
        /// Update that package has arrived at destination
        /// </summary>
        /// <param name="packageNumber">serial number of the package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PackageArrived(uint packageNumber)
        {
            List<Package> packages = XMLTools.LoadListFromXMLSerializer<Package>(PackagesPath);
            int i = packages.FindIndex(x => x.SerialNumber == packageNumber);
            if (i == -1)
                throw (new ItemNotFoundException("package", packageNumber));

            Package package = packages[i];
            package.PackageArrived = DateTime.Now;
            packages[i] = package;

            XMLTools.SaveListToXMLSerializer(packages, PackagesPath);
        }
        /// <summary>
        /// Updated package collected
        /// </summary>
        /// <param name="packageNumber">serial number of the package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PackageCollected(uint packageNumber)
        {
            List<Package> packages = XMLTools.LoadListFromXMLSerializer<Package>(PackagesPath);

            int i = packages.FindIndex(x => x.SerialNumber == packageNumber);
            if (i == -1)
                throw (new ItemNotFoundException("package", packageNumber));


            Package package = packages[i];
            package.CollectPackageForShipment = DateTime.Now;
            packages[i] = package;
            XMLTools.SaveListToXMLSerializer(packages, PackagesPath);
        }
        /// <summary>
        /// update fileds at a given base station
        /// </summary>
        /// <param name="base_"> a given base station </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBase(Base_Station base_)
        {
            List<Base_Station> base_Stations = XMLTools.LoadListFromXMLSerializer<Base_Station>(BaseStationPath);

            int i = base_Stations.FindIndex(x => x.baseNumber == base_.baseNumber);
            if (i == -1)
                throw (new DO.ItemNotFoundException("Base Station", base_.baseNumber));
            else
                base_Stations[i] = base_;

            XMLTools.SaveListToXMLSerializer(base_Stations, BaseStationPath);

        }
        /// <summary>
        /// update fileds at a given client
        /// </summary>
        /// <param name="client"> a given client </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateClient(Client client)
        {
           
            XElement ClientsRootElem = XMLTools.LoadListFromXMLElement(ClientsPath);
            XElement err = (from item in ClientsRootElem.Elements()
                            where ((item.Element("Id").Value.CompareTo(client.Id.ToString()) == 0) && (item.Element("Active").Value.CompareTo("true") == 0))
                            select item).FirstOrDefault();

            if (err == null)
                throw (new DO.ItemNotFoundException("client", client.Id));
            else
            {
                err.Element("Id").Value = client.Id.ToString();
                err.Element("Name").Value = client.Name;
                err.Element("PhoneNumber").Value = client.PhoneNumber;
                err.Element("Longitude").Value = client.Longitude.ToString();
                err.Element("Latitude").Value = client.Latitude.ToString();
                err.Element("Active").Value = client.Active.ToString();
            }

            XMLTools.SaveListToXMLElement(ClientsRootElem, ClientsPath);


        }
        /// <summary>
        /// update fileds at a given drone
        /// </summary>
        /// <param name="drone"> a given drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            int index = drones.FindIndex(x => x.SerialNumber == drone.SerialNumber && x.Active);
            if (index != -1)
                drones[index] = drone;
            else
                throw (new DO.ItemNotFoundException("drone", drone.SerialNumber));
            XMLTools.SaveListToXMLSerializer(drones, DronesPath);

        }
        /// <summary>
        /// Updating fields of a particular package
        /// </summary>
        /// <param name="package">particular package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePackege(Package package)
        {
            List<Package> packages = XMLTools.LoadListFromXMLSerializer<Package>(PackagesPath);
            int i = packages.FindIndex(x => x.SerialNumber == package.SerialNumber);
            if (i == -1)
                throw (new DO.ItemNotFoundException("Packege", package.SerialNumber));
            else
                packages[i] = package;
            XMLTools.SaveListToXMLSerializer(packages, PackagesPath);
        }

        /// <summary>
        /// connect package to drone
        /// </summary>
        /// <param name="packageNumber">serial number of the package that needs 
        /// to connect to drone </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ConnectPackageToDrone(uint packageNumber, uint drone_sirial_number)
        {
            List<Package> packages = XMLTools.LoadListFromXMLSerializer<Package>(PackagesPath);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);


            int i = packages.FindIndex(x => x.SerialNumber == packageNumber);
            if (i == -1)
                throw (new ItemNotFoundException("packege", packageNumber));
            if (!drones.Any(x => x.SerialNumber == drone_sirial_number))
                throw (new ItemNotFoundException("drone", drone_sirial_number));



            Package package = packages[i];
            package.OperatorSkimmerId = drone_sirial_number;

            package.PackageAssociation = DateTime.Now;
            packages[i] = package;

            XMLTools.SaveListToXMLSerializer(packages, PackagesPath);
            XMLTools.SaveListToXMLSerializer(drones, DronesPath);
        }
        /// <summary>
        /// send drone to charge
        /// </summary>
        /// <param name="drone"> drone number</param>
        /// <param name="base_"> Base station number that the drone will be sent to it </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneToCharge(uint drone, uint base_)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            List<Base_Station> base_Stations = XMLTools.LoadListFromXMLSerializer<Base_Station>(BaseStationPath);
            List<BatteryLoad> droneInCharge = XMLTools.LoadListFromXMLSerializer<BatteryLoad>(DroneInChargePath);

            if (!drones.Any(x => x.SerialNumber == drone && x.Active))
            {
                throw (new ItemNotFoundException("drone", drone));
            }
            if (base_Stations.All(x => x.baseNumber != base_))
            {
                throw (new ItemNotFoundException("base station", base_));
            }
            if (droneInCharge.Any(x => x.IdDrone == drone))
                throw new ItemFoundException("drone", drone);


            droneInCharge.Add(new BatteryLoad { IdDrone = drone, idBaseStation = base_, EntringDrone = DateTime.Now });
            for (int i = 0; i < base_Stations.Count; i++)
            {
                if (base_Stations[i].baseNumber == base_)
                {
                    var baseNew = base_Stations[i];
                    baseNew.NumberOfChargingStations--;
                    base_Stations[i] = baseNew;
                }
            }

            XMLTools.SaveListToXMLSerializer(base_Stations, BaseStationPath);
            XMLTools.SaveListToXMLSerializer(droneInCharge, DroneInChargePath);
            XMLTools.SaveListToXMLSerializer(drones, DronesPath);
        }
        #endregion
        #region Recuirs
        /// <summary>
        /// Display packege data 
        /// </summary>
        /// <param name="packageNumber">Serial number of a particular package</param>
        /// <returns> string of data</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Package packegeByNumber(uint packageNumber)
        {
            List<Package> packages = XMLTools.LoadListFromXMLSerializer<Package>(PackagesPath);

            int i = packages.FindIndex(x => x.SerialNumber == packageNumber);
            if (i == -1)
                throw (new ItemNotFoundException("package", packageNumber));
            return packages[i];
        }
        /// <summary>
        /// Display base station data desired   
        /// </summary>
        /// <param name="baseNum">Desired base station number</param>
        /// <returns> String of data </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Base_Station BaseStationByNumber(uint baseNum)
        {
            List<Base_Station> base_Stations = XMLTools.LoadListFromXMLSerializer<Base_Station>(BaseStationPath);

            if (!base_Stations.Any(x => x.baseNumber == baseNum))
                throw (new ItemNotFoundException("Base station", baseNum));
            return base_Stations[base_Stations.FindIndex(x => x.baseNumber == baseNum)];

        }
        /// <summary>
        /// return a list of all the base stations
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Base_Station> BaseStationList(Predicate<Base_Station> predicate)
        {
            List<Base_Station> base_Stations = XMLTools.LoadListFromXMLSerializer<Base_Station>(BaseStationPath);

            return from base_ in base_Stations
                   where predicate(base_)
                   select base_;
        }
        /// <summary>
        /// return list of charging drones
        /// </summary>
        /// <returns>return list of charging drones</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BatteryLoad> ChargingDroneList(Predicate<BatteryLoad> predicate)
        {
            List<BatteryLoad> droneInCharge = XMLTools.LoadListFromXMLSerializer<BatteryLoad>(DroneInChargePath);

            return from x in droneInCharge
                   where predicate(x)
                   select x;
        }
        /// <summary>
        /// Display client data desired 
        /// </summary>
        /// <param name="id">ID of desired client </param>
        /// <returns> string of data </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Client CilentByNumber(uint id)
        {
            // List<Client> clients = XMLTools.LoadListFromXMLSerializer<Client>(ClientsPath);

            XElement ClientsRootElem = XMLTools.LoadListFromXMLElement(ClientsPath);
            XElement err = (from item in ClientsRootElem.Elements()
                            where ((item.Element("Id").Value.CompareTo(id.ToString()) == 0))
                            select item).FirstOrDefault();


            if (err == null)
                throw (new ItemNotFoundException("client", id));


            return new Client
            {
                Id = uint.Parse(err.Element("Id").Value),
                Name = err.Element("Name").Value,
                PhoneNumber = err.Element("PhoneNumber").Value,
                Longitude = double.Parse(err.Element("Longitude").Value),
                Latitude = double.Parse(err.Element("Latitude").Value),
                Active = bool.Parse(err.Element("Active").Value)
            };


        }
        /// <summary>
        ///return all clients
        /// </summary>
        /// <returns> list of client</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Client> CilentList(Predicate<Client> predicate)
        {
            // List<Client> clients = XMLTools.LoadListFromXMLSerializer<Client>(ClientsPath);

            XElement ClientsRootElem = XMLTools.LoadListFromXMLElement(ClientsPath);

            return from item in ClientsRootElem.Elements()
                   let s = CilentByNumber(uint.Parse(item.Element("Id").Value))
                   where predicate(s)
                   select s;

        }
        /// <summary>
        /// clculate distans between two point 
        /// </summary>
        /// <param name="Longitude1"></param>
        /// <param name="Latitude1"></param>
        /// <param name="Longitude2"></param>
        /// <param name="Latitude2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Distance(double Longitude1, double Latitude1, double Longitude2, double Latitude2)
        {
            return DO.Point.Distance(Longitude1, Latitude1, Longitude2, Latitude2);
        }
        /// <summary>
        /// Display drone data  
        /// </summary>
        /// <param name="droneNum"> drone number</param>
        /// <returns> String of data</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DroneByNumber(uint droneNum)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            if (!drones.Any(x => x.SerialNumber == droneNum && x.Active))
                throw (new ItemNotFoundException("drone", droneNum));

            foreach (Drone item in drones)
            {
                if (item.SerialNumber == droneNum)
                {
                    return item;

                }
            }

            return drones[0];


        }
        /// <summary>
        /// return list of the drones
        /// </summary>
        /// <returns> return list of the drones</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> DroneList()
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            return drones.ToList<Drone>();
        }
        /// <summary>
        /// Returns how much electricity the drone needs:
        /// 0. Available, 1. Light weight 2. Medium weight 3. Heavy 4. Charging per minute
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<double> Elctrtricity()
        {


            List<string> config = XMLTools.LoadListFromXMLSerializer<string>(@"DalXmlConfig.xml");
            double[] temp = { double.Parse(config[0]), double.Parse(config[1]), double.Parse(config[2]),
                     double.Parse(config[3]),double.Parse(config[4])};

            double[] elctricity = new double[5];
            elctricity[(int)ButturyLoad.Free] = temp[0];
            elctricity[(int)ButturyLoad.Easy] = temp[1];
            elctricity[(int)ButturyLoad.Medium] = temp[2];
            elctricity[(int)ButturyLoad.Heavy] = temp[3];
            elctricity[(int)ButturyLoad.Charging] = temp[4];
            return elctricity;
        }

        /// <summary>
        /// return the list of all packages
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Package> PackegeList(Predicate<Package> predicate)
        {
            List<Package> packages = XMLTools.LoadListFromXMLSerializer<Package>(PackagesPath);

            // return DataSource.packages.Where(predicate).Select(x => x);
            return from x in packages
                   where predicate(x)
                   select x;
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
        #endregion
        #region Delete

        /// <summary>
        /// delete a spsific base for list
        /// </summary>
        /// <param name="sirial"> Base Station number</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteBase(uint sirial)
        {
            List<Base_Station> base_Stations = XMLTools.LoadListFromXMLSerializer<Base_Station>(BaseStationPath);


            var baseDeleteNumber = base_Stations.FindIndex(x => x.baseNumber == sirial);
            if (baseDeleteNumber == -1)
                throw (new ItemNotFoundException("Base station", sirial));
            var baseDelete = base_Stations[baseDeleteNumber];
            baseDelete.Active = false;

            base_Stations[baseDeleteNumber] = baseDelete;

            XMLTools.SaveListToXMLSerializer(base_Stations, BaseStationPath);

        }
        /// <summary>
        /// delete a spsific client 
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteClient(uint id)
        {
            // List<Client> clients = XMLTools.LoadListFromXMLSerializer<Client>(ClientsPath);

            XElement ClientsRootElem = XMLTools.LoadListFromXMLElement(ClientsPath);
            XElement err = (from item in ClientsRootElem.Elements()
                            where ((item.Element("Id").Value.CompareTo(id.ToString()) == 0) && (item.Element("Active").Value.CompareTo("true") == 0))
                            select item).FirstOrDefault();


            if (err == null)
                throw (new ItemNotFoundException("client", id));

            err.Element("Active").Value = "false";


            XMLTools.SaveListToXMLElement(ClientsRootElem, ClientsPath);
        }
        /// <summary>
        /// delete a spsific drone
        /// </summary>
        /// <param name="sirial"> drone number</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(uint sirial)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);

            if (!drones.Any(x => x.SerialNumber == sirial))
                throw (new ItemNotFoundException("drone", sirial));
            for (int i = 0; i < drones.Count(); i++)
            {
                if (drones[i].SerialNumber == sirial)
                {
                    var drone = drones[i];
                    drone.Active = false;
                    drones[i] = drone;
                    return;
                }
            }
            XMLTools.SaveListToXMLSerializer(drones, DronesPath);
        }
        /// <summary>
        /// delete a spsific packege
        /// </summary>
        /// <param name="sirial"> package number</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeletePackege(uint sirial)
        {
            List<Package> packages = XMLTools.LoadListFromXMLSerializer<Package>(PackagesPath);

            int i = packages.FindIndex(x => x.SerialNumber == sirial);
            if (i == -1)
                throw (new ItemNotFoundException("package", sirial));
            packages.Remove(packages[i]);
            XMLTools.SaveListToXMLSerializer(packages, PackagesPath);
        }
        #endregion



        #endregion
        #endregion
   
    }

}
