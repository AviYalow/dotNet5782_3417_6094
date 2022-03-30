/*
 we add the bonus function
 */

using System;
using System.Collections;
using System.Collections.Generic;
using DalApi;
using DO;


namespace ConsoleUI
{
    partial class Program
    {
        // enumes for the menu options
        enum Options { Exit, Add, Update, ShowDetails, ShowList }
        enum Entities { Exit, Client, Base_station, Drone, Package }
        enum UpdatesOptions { Exit, Associate, Collect, Delivery, Charge, UnCharge }
        enum Show { Exit, Client, Base_station, Drone, Package, ShowDistance, ShoeDegree }
        enum ShowList { Exit, Base_station, Drones, Clients, Package, FreePackage, FreeBaseStation }
        enum Distans_2_point { base_station = 1, client }

        /// <summary>
        ///  function which allows us to receive 
        ///  a number from the user safely
        /// </summary>
        private static uint getChoose(string val)
        {
            bool success;
            uint number;
            do
            {
                Console.WriteLine(val);
                success = uint.TryParse(Console.ReadLine(), out number);
                if (!success)
                    Console.WriteLine("Error!\n");
            }
            while (!success);
            return number;
        }

        /// <Menu>
        /// Selection menu that which show to the customer 
        /// when opening the program 
        /// </Menu>
        private static void Menu(DalApi.IDal dalObject)
        {

            Options option;
            Entities entity;
            UpdatesOptions updatesOption;
            Show show;
            ShowList showList;

            do
            {
                bool check;
                uint num, id, num1, num2;
                double doubleNum1, doubleNum2, point1, point2;
                string str, name, phone;
                ArrayList backList = new ArrayList();

                str = "Choose one of the following:\n" +
                    " 1-Add,\n 2-Update,\n 3-Show Details,\n 4-Show List,\n 0-Exit";

                num = getChoose(str);
                option = (Options)num;

                switch (option)
                {
                    case Options.Add:

                        str = "Choose an entity:\n " +
                             "1-Client,\n 2-Base station,\n 3- Drone,\n 4- Package";
                        num = getChoose(str);
                        entity = (Entities)num;

                        switch (entity)
                        {
                            case Entities.Client:
                                {
                                    //received the details from the user
                                    try
                                    {
                                        addClient(dalObject, out check, out num, out doubleNum1, out doubleNum2, out name, out phone);
                                    }
                                    catch (DO.ItemFoundException exception)
                                    {
                                        Console.WriteLine(exception);
                                    }
                                    break;
                                }
                            case Entities.Base_station:
                                {
                                    //received the details from the user
                                    try
                                    {
                                        addBase(dalObject, out check, out id, out num1, out doubleNum1, out doubleNum2, out name);
                                    }
                                    catch (DO.ItemFoundException exception)
                                    {
                                        Console.WriteLine(exception);
                                    }
                                    break;
                                }
                            case Entities.Drone:
                                {
                                    //received the details from the user
                                    try
                                    {
                                        addDrone(dalObject, out check, out num, out id, out num1, out doubleNum1);
                                    }
                                    catch (DO.ItemFoundException exception)
                                    {
                                        Console.WriteLine(exception);
                                    }
                                    break;
                                }
                            case Entities.Package:
                                {
                                    try
                                    {
                                        addPackage(dalObject, out check, out num, out id, out num1, out num2);
                                    }
                                    catch (DO.ItemFoundException exception)
                                    {
                                        Console.WriteLine(exception);
                                    }
                                    break;
                                }
                            case Entities.Exit:
                                break;
                        }
                        break;


                    case Options.Update:
                        str = "Choose one of the following updates:\n " +
                            "1-Associate a package,\n 2-collect a package by drone,\n" +
                            " 3- Delivering a package to the customer,\n" +
                            " 4- Send drone to Charge\n 5- free drone from charge";
                        num = getChoose(str);
                        updatesOption = (UpdatesOptions)num;
                        try
                        {
                            switch (updatesOption)
                            {


                                //option to connect package to drone
                                case UpdatesOptions.Associate:

                                    updateAssociate(dalObject, out check, out num, out num1);
                                    break;

                                // update that the package is collected
                                case UpdatesOptions.Collect:

                                    //received the details from the user
                                    updateCollect(dalObject, out check, out num);
                                    break;

                                // update that the package is arrived to the target
                                case UpdatesOptions.Delivery:

                                    updateDelivery(dalObject, out check, out num);
                                    break;
                                /*
                                                            //sent drone to a free charging station
                                                            case UpdatesOptions.Charge:

                                                                updateCharge( dalObject, out check, out num, backList);
                                                                break;

                                                            // Release drone from charging position
                                                            case UpdatesOptions.UnCharge:

                                                                releaseDrone( dalObject, out check, out num);
                                                                break;
                                                                */
                                case UpdatesOptions.Exit:
                                    break;


                            }
                        }
                        catch (DO.ItemNotFoundException exception)
                        {

                            Console.WriteLine(exception);
                        }
                        break;

                    case Options.ShowDetails:
                        str = "Choose one of the following view option:\n " +
                             "1-Client,\n 2-Base station,\n 3- Drone,\n 4- Package\n 5-Distance";
                        num = getChoose(str);
                        show = (Show)num;
                        try
                        {
                            switch (show)
                            {
                                case Show.Client:

                                    clientById(dalObject, out check, out num);
                                    break;
                                case Show.Base_station:

                                    baseByNumber(dalObject, out check, out num);
                                    break;
                                case Show.Drone:
                                    droneBySirialNumber(dalObject, out check, out num);
                                    break;
                                case Show.Package:
                                    packageByNumber(dalObject, out check, out num);
                                    break;

                                //option to show distance between two points
                                case Show.ShowDistance:
                                    {
                                        distanceBetween2points(dalObject, out check, out num1, out doubleNum1, out doubleNum2, out point1, out point2);

                                        break;
                                    }
                                case Show.ShoeDegree:
                                    pointToDegree(dalObject, out check, out point2);
                                    break;
                                case Show.Exit:
                                    break;
                            }
                        }
                        catch (DO.ItemNotFoundException exception)
                        {

                            Console.WriteLine(exception);
                        }
                        break;
                    case Options.ShowList:
                        str = "Choose one of the following option:\n" +
                             " 1-Base stations,\n 2- Drones,\n 3-Clients,\n" +
                             " 4- Packages\n 5-packege with no drone,\n 6- Base station with free charge states";
                        num = getChoose(str);
                        showList = (ShowList)num;
                        try
                        {
                            switch (showList)
                            {
                                case ShowList.Base_station:
                                    listOfBass(dalObject);
                                    break;
                                case ShowList.Drones:
                                    listOfDrone(dalObject);
                                    break;
                                case ShowList.Clients:
                                    listOfClinet(dalObject);
                                    break;
                                case ShowList.Package:
                                    listOfPackage(dalObject);
                                    break;
                                case ShowList.FreePackage:
                                    packegeWhitNoDrone(dalObject);
                                    break;
                                case ShowList.FreeBaseStation:
                                    baseWhitFreeChargeStation(dalObject);
                                    break;
                                case ShowList.Exit:
                                    break;
                            }
                        }
                        catch (DO.ItemNotFoundException exception)
                        {

                            Console.WriteLine(exception);
                        }
                        break;


                    case Options.Exit:
                        break;

                }
            } while (option != Options.Exit);



            void baseWhitFreeChargeStation(DalApi.IDal dalObject)
            {

                foreach (DO.Base_Station print in dalObject.BaseStationList(x => x.NumberOfChargingStations > 0))
                    Console.WriteLine(print);
            }

            void packegeWhitNoDrone(DalApi.IDal dalObject)
            {

                foreach (DO.Package print in dalObject.PackegeList(x => x.OperatorSkimmerId == 0))
                    Console.WriteLine(print);
            }

            void listOfPackage(DalApi.IDal dalObject)
            {

                foreach (DO.Package print in dalObject.PackegeList(x => true))
                    Console.WriteLine(print);
            }

            void listOfClinet(DalApi.IDal dalObject)
            {

                foreach (DO.Client print in dalObject.CilentList(x=>true))
                    Console.WriteLine(print);
            }

            void listOfDrone(DalApi.IDal dalObject)
            {

                List<DO.Drone> list = (List<DO.Drone>)dalObject.DroneList();
                foreach (DO.Drone print in list)
                    Console.WriteLine(print);
            }

            void listOfBass(DalApi.IDal dalObject)
            {


                foreach (DO.Base_Station print in dalObject.BaseStationList(x => true))
                    Console.WriteLine(print);


            }

            void pointToDegree(DalApi.IDal dalObject, out bool check, out double point2)
            {
                Console.Write("Enter a longitude or latitude to ge it in degree :");
                do
                {
                    check = double.TryParse(Console.ReadLine(), out point2);
                } while (!check);
                Console.WriteLine(dalObject.PointToDegree(point2));
            }

            void distanceBetween2points(DalApi.IDal dalObject, out bool check, out uint num1, out double doubleNum1, out double doubleNum2, out double point1, out double point2)
            {
                num1 = 0;
                double[] points = new double[4];

                //received the details from the user
                Console.Write("Insert the latitude of the first point:");
                do
                {
                    check = double.TryParse(Console.ReadLine(), out doubleNum1);
                } while (!check);
                Console.Write("Enter a longitude of the first point:");
                do
                {
                    check = double.TryParse(Console.ReadLine(), out doubleNum2);
                } while (!check);
                Console.Write("Enter a latitude of the second point:");
                do
                {
                    check = double.TryParse(Console.ReadLine(), out point1);
                } while (!check);
                Console.Write("Enter a longitude of the second point:");
                do
                {
                    check = double.TryParse(Console.ReadLine(), out point2);
                } while (!check);

                Console.WriteLine($"the distance is: {0}KM", dalObject.Distance(doubleNum2, doubleNum1, point2, point1));
            }

            void packageByNumber(DalApi.IDal dalObject, out bool check, out uint num)
            {
                Console.Write("Enter packege number:");
                check = uint.TryParse(Console.ReadLine(), out num);
                Console.WriteLine(dalObject.packegeByNumber(num));
            }

            void droneBySirialNumber(DalApi.IDal dalObject, out bool check, out uint num)
            {
                Console.Write("Enter drone number:");
                check = uint.TryParse(Console.ReadLine(), out num);
                Console.WriteLine(dalObject.DroneByNumber(num));
            }

            void baseByNumber(DalApi.IDal dalObject, out bool check, out uint num)
            {
                //received the details from the user
                Console.Write("Enter base number:");
                check = uint.TryParse(Console.ReadLine(), out num);
                Console.WriteLine(dalObject.BaseStationByNumber(num));
            }

            void clientById(DalApi.IDal dalObject, out bool check, out uint num)
            {
                //received the details from the user
                Console.Write("Enter ID:");
                check = uint.TryParse(Console.ReadLine(), out num);
                Console.WriteLine(dalObject.CilentByNumber(num));
            }


            void updateDelivery(DalApi.IDal dalObject, out bool check, out uint num)
            {
                //received the details from the user
                Console.Write("Enter package number:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num);
                } while (!check);
                dalObject.PackageArrived(num);
            }

            void updateCollect(DalApi.IDal dalObject, out bool check, out uint num)
            {
                Console.Write("Enter package number:");

                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num);
                } while (!check);
                dalObject.PackageCollected(num);
            }

            void updateAssociate(DalApi.IDal dalObject, out bool check, out uint num, out uint num1)
            {
                //received the details from the user
                Console.Write("Enter package number:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num);
                } while (!check);
                Console.Write("Enter drone number:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num1);
                } while (!check);
                dalObject.ConnectPackageToDrone(num, num1);
            }

            void addPackage(DalApi.IDal dalObject, out bool check, out uint num, out uint id, out uint num1, out uint num2)
            {
                //received the details from the user

                Console.Write("Enter ID of the sender:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out id);
                } while (!check);
                Console.Write("Enter ID of the recipient:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num1);
                } while (!check);
                Console.Write("Enter Weight categories 0 for easy,1 for mediom,2 for haevy:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num2);
                } while (!check);
                Console.Write("Enter priority 0 for Immediate ,1 for quick ,2 for Regular:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num);
                } while (!check);
                // add new package
                dalObject.AddPackage(new DO.Package { SendClient = id, GetingClient = num1, WeightCatgory = (WeightCategories)num2, Priority = (Priority)num });
            }

            void addDrone(DalApi.IDal dalObject, out bool check, out uint num, out uint id, out uint num1, out double doubleNum1)
            {
                Console.Write("Enter sireal number:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out id);
                } while (!check);
                Console.Write("chose name:");
                int i = 1;
                foreach (var model in Enum.GetValues(typeof(DroneModel)))
                {
                    Console.WriteLine(i + " " + model);
                    i++;
                }
                DroneModel model1;
                do
                {
                    check = Enum.TryParse(Console.ReadLine(), out model1);
                    if(!check)
                        Console.WriteLine("Error input");
                }
                while (!check);
               
                Console.Write("Enter weight Category:0 for easy,1 for mediom,2 for heavy:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num);
                } while (!check);
                Console.Write("Enter amount of butrry:");
                do
                {
                    check = double.TryParse(Console.ReadLine(), out doubleNum1);
                } while (!check);
                Console.Write("Enter a statos:0 for free,1 for maintenance:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num1);
                } while (!check);

                //add new drone
                dalObject.AddDrone(new Drone { SerialNumber = id, Model = model1, WeightCategory = (WeightCategories)num });
            }

            void addBase(DalApi.IDal dalObject, out bool check, out uint id, out uint num1, out double doubleNum1, out double doubleNum2, out string name)
            {
                Console.Write("Enter base number:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out id);
                } while (!check);
                Console.Write("Enter base name:");
                name = Console.ReadLine();
                Console.Write("Enter Number of charging stations:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num1);
                } while (!check);
                Console.Write("Enter latitude:");
                do
                {
                    check = double.TryParse(Console.ReadLine(), out doubleNum1);
                } while (!check);
                Console.Write("Enter longitude:");
                do
                {

                    check = double.TryParse(Console.ReadLine(), out doubleNum2);
                } while (!check);

                // add new Base station
                dalObject.AddStation(new Base_Station { baseNumber = id, NameBase = name, NumberOfChargingStations = num1, latitude = doubleNum1, longitude = doubleNum2 });
            }

            void addClient(DalApi.IDal dalObject, out bool check, out uint num, out double doubleNum1, out double doubleNum2, out string name, out string phone)
            {
                Console.Write("Enter ID:");
                do
                {
                    check = uint.TryParse(Console.ReadLine(), out num);
                } while (!check);
                Console.Write("Enter name:");
                name = Console.ReadLine();
                Console.Write("Enter phone number:");
                phone = Console.ReadLine();
                Console.Write("Enter latitude:");
                do
                {
                    check = double.TryParse(Console.ReadLine(), out doubleNum1);
                } while (!check);
                Console.Write("Enter londitude:");
                do
                {
                    check = double.TryParse(Console.ReadLine(), out doubleNum2);
                } while (!check);

                // add new client
                dalObject.AddClient(new Client { Id = num, Name = name, PhoneNumber = phone, Latitude = doubleNum1, Longitude = doubleNum2 });
            }
        



         
        }
        static void Main(string[] args)
        {

            IDal dalObject = DalFactory.GetDal();

            Menu(dalObject);
        }
    }
}
