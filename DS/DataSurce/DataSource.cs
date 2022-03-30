using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ds
{
    /// <summary>
    /// The database
    /// </summary>
    public static class DataSource
    {
        //an arrays that contain the various entity
        public static List<Drone> drones = new List<Drone>();
        public static List<Base_Station> base_Stations = new List<Base_Station>();
        public static List<Client> clients = new List<Client>();
        public static List<Package> packages = new List<Package>();
        public static List<BatteryLoad> droneInCharge = new List<BatteryLoad>();
       // public static Config config

        /// <Config>
        /// 
        /// </Config>
        public class Config
        {
            public  static  double free { get { return 0.25; } }//per minute
            public static double easyWeight { get { return 0.5; } }//per minute
            public static double mediomWeight { get { return 0.75; } }//per minute
            public static double heavyWeight { get { return 1; } }//per minute
            public static double Charging_speed { get { return 1.5; } }//per minute
            public static uint package_num = 10000;

        }
        static DataSource()
        {
            Initialize();


        }

        /// <Initialize>
        ///a quick boot of all lists of data 
        /// </Initialize>
         static void Initialize()
        {

            Random rand = new Random();

            //A function that responsible for initializing names randomly in upercase
            string randomName(Random rand, int num = 3)
            {
                string name = "";
                for (int j = 0; j < num; j++)
                {
                    name += (char)rand.Next(65, 91);
                }
                return name;
            }

            //A function that responsible for initializing names randomly-upercase
            //letter for the firt letter and lowercase for the other letters
            string personalRandomName(Random rand, int num = 3)
            {
                string name = "";
                name += (char)rand.Next(65, 91);
                for (int j = 0; j < num - 1; j++)
                {
                    name += (char)rand.Next(97, 123);
                }
                return name;
            }



            //initializing the base station's array
            base_Stations.Add(new Base_Station
            {
                baseNumber = (uint)rand.Next(1000, 10000),
                NameBase = randomName(rand),
                NumberOfChargingStations = (uint)rand.Next(5, 15),
                latitude = 31.790133,
                longitude = 34.627143,
                Active=true

            });


            base_Stations.Add(new Base_Station
            {
                baseNumber = (uint)rand.Next(1000, 10000),
                NameBase = randomName(rand),
                NumberOfChargingStations = (uint)rand.Next(4, 9),
                latitude = 32.009490,
                longitude = 34.736002,
                Active=true
            });


            //initializing the drones list in a randone values
            for (int i = 0; i < 15; i++)
            {
                drones.Add(new Drone
                {
                    SerialNumber = (uint)rand.Next(10000),
                    Model =(DroneModel) rand.Next(6),
                    WeightCategory = (WeightCategories)rand.Next(0, 3),
                    Active=true

                });


            }

            //initializing the clients list in a random values

            Client client1 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 31.8780769200329, Longitude = 34.72723782085096, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}" , Active = true };
            Client client2 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 31.890176390661207, Longitude = 34.83092321246071, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}", Active = true };
            Client client3 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 31.796158578989683, Longitude = 34.66586515449866, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}" , Active = true };
            Client client4 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 31.820190873604652, Longitude = 34.72233764519624, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}" , Active = true };
            Client client5 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 31.95219391037637, Longitude = 34.770318188724346, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}" , Active = true };
            Client client6 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 32.05327808523327, Longitude = 34.75681810343021, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}" , Active = true };
            Client client7 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 31.94281576785756, Longitude = 34.8245042988593, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}", Active = true };
            Client client8 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 31.866628498784383, Longitude = 34.73603504157726, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}", Active = true };
            Client client9 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 31.926210074685223, Longitude = 34.71376824495921, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}" , Active = true };
            Client client10 = new Client { Id = (uint)rand.Next(100000000, 999999999), Latitude = 31.905193040785168, Longitude = 34.8050634373223, Name = (personalRandomName(rand, rand.Next(3, 7))), PhoneNumber = $"05{rand.Next(0, 6)}{rand.Next(100, 999)}{rand.Next(1000, 9999)}", Active = true };

            clients.Add(client1); clients.Add(client2); clients.Add(client3); clients.Add(client4); clients.Add(client5); clients.Add(client6); clients.Add(client7); clients.Add(client8); clients.Add(client9); clients.Add(client10);


            //initializing the packages list in a random values

            Package package1 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client1.Id, GetingClient = client2.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-14, -6)) };
            package1.OperatorSkimmerId = drones[0].SerialNumber;
            package1.WeightCatgory = (WeightCategories)rand.Next((int)drones[0].WeightCategory + 1);
            package1.PackageAssociation = package1.ReceivingDelivery.Value.AddMinutes(30);
            
            package1.CollectPackageForShipment = package1.PackageAssociation.Value.AddHours(rand.Next(5));
            package1.PackageArrived = package1.CollectPackageForShipment.Value.AddHours(3);
            Config.package_num++;

            Package package2 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client3.Id, GetingClient = client4.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-10, -8)).AddHours(rand.Next(-15, 6)) };
            package2.OperatorSkimmerId = drones[1].SerialNumber;
            package2.WeightCatgory = (WeightCategories)rand.Next((int)drones[1].WeightCategory + 1);
            package2.PackageAssociation = package2.ReceivingDelivery.Value.AddMinutes(30);
            package2.CollectPackageForShipment = package2.PackageAssociation.Value.AddHours(rand.Next(5));
            package2.PackageArrived = package2.CollectPackageForShipment.Value.AddHours(3);
            Config.package_num++;

            Package package3 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client5.Id, GetingClient = client6.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-11, -5)).AddHours(rand.Next(-20, 4)).AddMinutes(rand.Next(-59, 60)) };
            package3.OperatorSkimmerId = drones[2].SerialNumber;
            package3.WeightCatgory = (WeightCategories)rand.Next((int)drones[2].WeightCategory + 1);
            package3.PackageAssociation = package3.ReceivingDelivery.Value.AddMinutes(30);
            package3.CollectPackageForShipment = package3.PackageAssociation.Value.AddHours(rand.Next(5));
            package3.PackageArrived = package3.CollectPackageForShipment.Value.AddHours(3);
            Config.package_num++;

            Package package4 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client5.Id, GetingClient = client2.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-14, -6)).AddHours(rand.Next(-20, 4)).AddMinutes(rand.Next(-59, 60)) };
            package4.OperatorSkimmerId = drones[3].SerialNumber;
            package4.WeightCatgory = (WeightCategories)rand.Next((int)drones[3].WeightCategory + 1);
            package4.PackageAssociation = package4.ReceivingDelivery.Value.AddMinutes(30);

            Config.package_num++;

            Package package5 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client6.Id, GetingClient = client10.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-19, -6)).AddHours(rand.Next(-20, 4)).AddMinutes(rand.Next(-59, 60)) };
            package5.OperatorSkimmerId = drones[4].SerialNumber;
            package5.WeightCatgory = (WeightCategories)rand.Next((int)drones[4].WeightCategory + 1);
            package5.PackageAssociation = package5.ReceivingDelivery.Value.AddMinutes(30);

            Config.package_num++;

            Package package6 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client9.Id, GetingClient = client1.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-17, -6)).AddHours(rand.Next(-20, 4)).AddMinutes(rand.Next(-59, 60)) };
            package6.OperatorSkimmerId = drones[1].SerialNumber;
            package6.WeightCatgory = (WeightCategories)rand.Next((int)drones[1].WeightCategory + 1);
            package6.PackageAssociation = package6.ReceivingDelivery.Value.AddMinutes(30);
            Config.package_num++;

            Package package7 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client10.Id, GetingClient = client2.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-20, -6)).AddHours(rand.Next(-20, 4)).AddMinutes(rand.Next(-59, 60)) };
            package7.OperatorSkimmerId = drones[2].SerialNumber;
            package7.WeightCatgory = (WeightCategories)rand.Next((int)drones[2].WeightCategory + 1);
            package7.PackageAssociation = package7.ReceivingDelivery.Value.AddMinutes(30);
            package7.CollectPackageForShipment = package7.PackageAssociation.Value.AddHours(rand.Next(5));
            
            Config.package_num++;

            Package package8 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client8.Id, GetingClient = client3.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-8, 0)).AddHours(rand.Next(-20, 4)).AddMinutes(rand.Next(-59, 60)) };
            package8.OperatorSkimmerId = 0;
            package8.WeightCatgory = (WeightCategories)rand.Next(3);
            
            Config.package_num++;

            Package package9 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client7.Id, GetingClient = client1.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-6, 0)).AddHours(rand.Next(-20, 4)).AddMinutes(rand.Next(-59, 60)) };
            package9.OperatorSkimmerId = 0;
            package9.WeightCatgory = (WeightCategories)rand.Next(3);
          
            Config.package_num++;

            Package package10 = new Package { SerialNumber = Config.package_num, Priority = (Priority)rand.Next(0, 3), SendClient = client5.Id, GetingClient = client6.Id, ReceivingDelivery = DateTime.Now.AddDays(rand.Next(-5, 0)).AddHours(rand.Next(-20, 4)).AddMinutes(rand.Next(-59, 60)) };
            package10.OperatorSkimmerId = 0;
            package10.WeightCatgory = (WeightCategories)rand.Next(3);

            Config.package_num++;


            DataSource.packages.Add(package1); DataSource.packages.Add(package2); DataSource.packages.Add(package3); DataSource.packages.Add(package4); DataSource.packages.Add(package5); DataSource.packages.Add(package6); DataSource.packages.Add(package7); DataSource.packages.Add(package8); DataSource.packages.Add(package9); DataSource.packages.Add(package10);




        }

    }


}

