using System;
using System.Collections;
using BO;
using BlApi;

namespace ConsoleUI_BL
{
    class Program
    {
        // enumes for the menu options
        #region
        enum Options { Exit, Add, Update, Delete, ShowDetails, ShowList }
        enum Entities { Exit, Base_station, Drone, Client, Package }
        enum UpdatesOptions { Exit, DroneName, Base_station, Client, Charge, UnCharge, Associate, Collect, Delivery }
        enum DeleteOption { Exit, Drone, BaseStation, Client, Packege }
        enum Show { Exit, Client, Base_station, Drone, Package, ShowDistance, ShoeDegree }
        enum ShowList { Exit, Base_station, Drones, Clients, Package, FreePackage, FreeBaseStation }
        enum Distans_2_point { base_station = 1, client }
        #endregion

        /// <summary>
        ///  function which allows us to receive 
        ///  a number from the user safely
        /// </summary>
        private static uint getChoose(string str)
        {
            bool success;
            uint number;
            do
            {
                Console.WriteLine(str);
                success = uint.TryParse(Console.ReadLine(), out number);
                if (!success)
                    Console.WriteLine("Error!\n");
            }
            while (!success);
            Console.WriteLine();
            return number;
        }

        /// <Menu>
        /// Selection menu which show to the customer 
        /// when opening the program 
        /// </Menu>
        private static void Menu(BlApi.IBL bl)
        {
            if (bl is null)
            {
                throw new ArgumentNullException(nameof(bl));
            }

            Options option;
            Entities entity;
            UpdatesOptions updatesOption;
            Show show;
            ShowList showList;


            do
            {
                bool check;
                uint num, id, num1, num2;

                double doubleNum1, doubleNum2;
                string str, name, phone;


                str = "Choose one of the following:\n" +
                    " 1-Add,\n 2-Update,\n 3-Delete\n 4-Show Details,\n 5-Show List,\n 0-Exit";

                num = getChoose(str);
                option = (Options)num;

                switch (option)
                {
                    case Options.Add:

                        #region
                        str = "Choose an entity:\n " +
                             "1-Base station,\n 2-Drone,\n 3- Client,\n 4- Package";
                        num = getChoose(str);
                        entity = (Entities)num;
                        try
                        {
                            switch (entity)
                            {
                                case Entities.Base_station:
                                    AddBase(bl, out check, out id, out num1, out doubleNum1, out doubleNum2, out name);
                                    break;
                                case Entities.Drone:
                                    AddDrone(bl, out check, out num, out id, out num1);
                                    break;
                                case Entities.Client:
                                    AddClient(bl, out check, out num, out doubleNum1, out doubleNum2, out name, out phone);
                                    break;
                                case Entities.Package:
                                    AddPackage(bl, out check, out num, out id, out num1, out num2);
                                    break;

                                case Entities.Exit:
                                    break;
                            }
                        }
                        #region
                        catch (ItemNotFoundException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (ItemFoundExeption ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (NumberMoreException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (NumberNotEnoughException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (ClientOutOfRangeException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (MoreDistasThenMaximomException ex)
                        { Console.WriteLine(ex); }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        #endregion
                        #endregion
                        break;
                    #region
                    case Options.Update:
                        str = "Choose one of the following updates:\n " +
                            "1-Drone name\n 2-Base Station\n" +
                            " 3- Client\n 4- Send drone to Charge\n " +
                            "5- free drone from charge\n 6-Associate a package\n" +
                            "7-Collect a package by drone\n 8-Delivering a package to the customer\n";

                        num = getChoose(str);
                        updatesOption = (UpdatesOptions)num;
                        try
                        {
                            switch (updatesOption)
                            {
                                case UpdatesOptions.DroneName:
                                    UpdateDroneName(bl, out check, out num);
                                    break;
                                case UpdatesOptions.Base_station:
                                    UpdateBase(bl, out check, out id, out name);
                                    break;
                                case UpdatesOptions.Client:
                                    UpdateClient(bl, out check, out id, out name, out phone);
                                    break;
                                case UpdatesOptions.Charge:
                                    updateCharge(bl, out check, out num);
                                    break;
                                case UpdatesOptions.UnCharge:
                                    UpdateUnCharge(bl, out check, out num,out doubleNum1);
                                    break;
                                case UpdatesOptions.Associate:
                                    UpdateAssociate(bl, out check, out num);
                                    break;
                                case UpdatesOptions.Collect:
                                    updateCollect(bl, out check, out num);
                                    break;
                                case UpdatesOptions.Delivery:
                                    updateDelivery(bl, out check, out num);
                                    break;
                                case UpdatesOptions.Exit:
                                    break;

                            }
                        }
                        catch(DroneStillAtWorkException ex)
                        { Console.WriteLine(ex); }
                        catch(PackegeNotAssctionOrCollectedException ex)
                        { Console.WriteLine(ex); }
                        catch(DroneCantMakeDliveryException ex)
                        {
                            Console.WriteLine(ex);
                        }

                        catch (BO.InputErrorException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (BO.UpdateChargingPositionsException ex)
                        {
                            Console.WriteLine(ex);

                            freeBaseFromDrone(bl, out check, ref num, ex);
                        }
                        catch (BO.StartingException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (BO.IllegalDigitsException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (BO.NumberMoreException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (BO.NumberNotEnoughException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (BO.TryToPullOutMoreDrone ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (BO.ItemNotFoundException exception)
                        {

                            Console.WriteLine(exception);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    #endregion
                    case Options.Delete:

                        str = "Choose an entity:\n " +
                           "1-Drone,\n 2-Base station,\n 3- Client,\n 4- Package";
                        num = getChoose(str);
                        try
                        {
                            switch ((DeleteOption)num)
                            {
                                case DeleteOption.Exit:
                                    break;
                                case DeleteOption.Drone:
                                    Console.WriteLine("Please enter drone number to delete:");
                                    do
                                    {
                                        check = uint.TryParse(Console.ReadLine(), out num);
                                        if (!check)
                                            Console.WriteLine("The input is Error please enter new number.");
                                    } while (!check);

                                    bl.DeleteDrone(num);
                                    break;
                                case DeleteOption.BaseStation:
                                    Console.WriteLine("Please enter base station number to delete:");
                                    do
                                    {
                                        check = uint.TryParse(Console.ReadLine(), out num);
                                        if (!check)
                                            Console.WriteLine("The input is Error please enter new number.");
                                    } while (!check);
                                    bl.DeleteBase(num);
                                    break;
                                case DeleteOption.Client:
                                    Console.WriteLine("Please enter client number to delete:");
                                    do
                                    {
                                        check = uint.TryParse(Console.ReadLine(), out num);
                                        if (!check)
                                            Console.WriteLine("The input is Error please enter new number.");
                                    } while (!check);
                                    bl.DeleteClient(num);
                                    break;
                                case DeleteOption.Packege:
                                    Console.WriteLine("Please enter packege number to delete:");
                                    do
                                    {
                                        check = uint.TryParse(Console.ReadLine(), out num);
                                        if (!check)
                                            Console.WriteLine("The input is Error please enter new number.");
                                    } while (!check);
                                    try
                                    {
                                        bl.DeletePackege(num);
                                    }
                                    catch (BO.ThePackegeAlredySendException ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                    break;
                                default:
                                    break;
                            }


                        }
                       
                        catch (BO.ThePackegeAlredySendException ex)
                        { Console.WriteLine(ex); }
                        catch (BO.DroneStillAtWorkException ex)
                        { 
                            Console.WriteLine(ex);
                        }
                       
                      
                        catch (BO.ItemNotFoundException ex)
                        {
                            Console.WriteLine(ex);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;

                    #region
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
                                    clientById(bl, out check, out id);
                                    break;
                                case Show.Base_station:
                                    baseByNumber(bl, out check, out num);
                                    break;
                                case Show.Drone:
                                    droneBySirialNumber(bl, out check, out num);
                                    break;
                                case Show.Package:
                                    packageByNumber(bl, out check, out num);
                                    break;
                                case Show.ShowDistance:
                                    Distans(bl, out check, out doubleNum1);
                                    break;
                                case Show.ShoeDegree:
                                    //  pointToDegree(bl, out check, out point2);
                                    break;
                                case Show.Exit:
                                    break;
                            }
                        }

                        catch (ItemNotFoundException exception)
                        {

                            Console.WriteLine(exception);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
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
                                    ShowBaseList(bl);
                                    break;
                                case ShowList.Drones:
                                    ShowDroneList(bl);
                                    break;
                                case ShowList.Clients:
                                    ShowClientList(bl);
                                    break;
                                case ShowList.Package:
                                    ShowPackageList(bl);
                                    break;
                                case ShowList.FreePackage:
                                    ShowFreePackage(bl);
                                    break;
                                case ShowList.FreeBaseStation:
                                    ShowFreeBaseStation(bl);
                                    break;
                                case ShowList.Exit:
                                    break;
                            }
                        }
                        catch (TheListIsEmptyException exception)
                        {

                            Console.WriteLine(exception);
                        }
                        catch (ItemNotFoundException exception)
                        {

                            Console.WriteLine(exception);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;


                    case Options.Exit:
                        break;

                }
                #endregion
            } while (option != Options.Exit);
        }

        /// <summary>
        ///Displays specific package details
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num"> serial number of the package</param>
        public static void packageByNumber(BlApi.IBL bl, out bool check, out uint num)
        {
            Console.Write("Enter packege number:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out num);
            } while (!check);

            Console.WriteLine(bl.ShowPackage(num)); ;
        }

        /// <summary>
        ///Displays specific client details
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="id"> the id of the client</param>
        public static void clientById(BlApi.IBL bl, out bool check, out uint id)
        {
            //received the details from the user
            Console.Write("Enter ID:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out id);
            } while (!check);
            Console.WriteLine(bl.GetingClient(id));
        }

        /// <summary>
        ///Displays specific drone details
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num">serail number of the drone</param>
        public static void droneBySirialNumber(BlApi.IBL bl, out bool check, out uint num)
        {
            Console.Write("Enter drone number:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out num);
            } while (!check);
            Console.WriteLine(bl.GetDrone(num)); ;
        }

        /// <summary>
        ///Displays specific base station  details
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num"> serial number of the base station</param>
        public static void baseByNumber(BlApi.IBL bl, out bool check, out uint num)
        {
            //received the details from the user
            Console.Write("Enter base number:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out num);
            } while (!check);
            Console.WriteLine(bl.BaseByNumber(num));
        }

        /// <summary>
        /// add new package
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num">  priority </param>
        /// <param name="id">ID of the sender </param>
        /// <param name="num1">ID of the recipient</param>
        /// <param name="num2">Weight categorie </param>
        public static void AddPackage(BlApi.IBL bl, out bool check, out uint num, out uint id, out uint num1, out uint num2)
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
            Console.Write("Enter Weight categories 0 for easy 0-12Kg,1 for mediom 12-20Kg,2 for haevy 20-28Kg:");
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
            uint i = bl.AddPackege(new Package { SendClient = new ClientInPackage { Id = id }, RecivedClient = new ClientInPackage { Id = num1 }, WeightCatgory = (WeightCategories)num2, Priority = (Priority)num });
            Console.WriteLine("Thank You! your packege number is: {0}", i);
        }

        /// <summary>
        /// add new drone
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num"> weight Category</param>
        /// <param name="id">sireal number</param>
        /// <param name="num1"> serial number of the first base station</param>
        /// <param name="name"> model</param>
        public static void AddDrone(BlApi.IBL bl, out bool check, out uint num, out uint id, out uint num1)
        {
            Console.Write("Enter sireal number:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out id);
            } while (!check);
            Console.Write("chose name:");
            int i = 0;
            foreach (var model in Enum.GetValues(typeof(DroneModel)))
            {
                Console.WriteLine(i + " " + model);
                i++;
            }
            DroneModel model1;
            do
            {
                check = Enum.TryParse(Console.ReadLine(), out model1);
                if (!check)
                    Console.WriteLine("Error input");
            }
            while (!check);
            Console.Write("Enter weight Category:0 for easy 0-12Kg,1 for mediom 12-20Kg,2 for heavy 20-28Kg:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out num);

            } while (!check);

            Console.Write("Enter number of base station for the first charging: ");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out num1);
            } while (!check);

            DroneToList drone = new DroneToList
            {
                SerialNumber = id,
                Model = model1,
                WeightCategory = (WeightCategories)num,

            };

            //add new drone
            bl.AddDrone(drone, num1);
        }

        /// <summary>
        /// add new base station
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="check"></param>
        /// <param name="id"> serail number</param>
        /// <param name="num1">Number of charging stations</param>
        /// <param name="doubleNum1">latitude</param>
        /// <param name="doubleNum2">longitude</param>
        /// <param name="name">base station name</param>
        public static void AddBase(BlApi.IBL bL, out bool check, out uint id, out uint num1, out double doubleNum1, out double doubleNum2, out string name)
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

            BaseStation baseStation = new BaseStation
            {
                SerialNum = id,
                Name = name,
                Location = new Location { Longitude = doubleNum1, Latitude = doubleNum2 },
                FreeState = num1,
                DronesInChargeList = null
            };
            // add new Base station
            bL.AddBase(baseStation);

        }
        /// <summary>
        /// add new client
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="myId">ID</param>
        /// <param name="doubleNum1">latitude</param>
        /// <param name="doubleNum2">londitude</param>
        /// <param name="name">name</param>
        /// <param name="phone">phone number</param>
        public static void AddClient(BlApi.IBL bl, out bool check, out uint myId, out double doubleNum1, out double doubleNum2, out string name, out string phone)
        {
            Console.Write("Enter ID:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out myId);
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
            Client client = new Client
            {
                Id = myId,
                Name = name,
                Phone = phone,
                Location = new Location { Longitude = doubleNum1, Latitude = doubleNum2 },
                FromClient = null,
                ToClient = null
            };
            bl.AddClient(client);
        }

        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="id"> serial number</param>
        /// <param name="name">base name</param>
        /// <param name="amount"> charging stations</param>
        public static void UpdateBase(BlApi.IBL bl, out bool check, out uint id, out string name)
        {
            Console.Write("Enter base number:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out id);
            } while (!check);

            Console.Write("Enter base name:");
            name = Console.ReadLine();

            Console.Write("Enter Number of charging stations:");
            string temp = Console.ReadLine();
            bl.UpdateBase(id, name, temp);


        }

        /// <summary>
        /// Drone data update
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num">serial number</param>
        public static void updateDelivery(BlApi.IBL bl, out bool check, out uint num)
        {
            //received the details from the user
            Console.Write("Enter drone number:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out num);
            } while (!check);
            bl.PackegArrive(num);
        }

        /// <summary>
        /// Client data update
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="id"> ID</param>
        /// <param name="name"> name</param>
        /// <param name="phone"> phone number</param>
        public static void UpdateClient(BlApi.IBL bl, out bool check, out uint id, out string name, out string phone)
        {
            Console.Write("Enter ID:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out id);
            } while (!check);
            Console.Write("Enter name:");
            name = Console.ReadLine();
            Console.Write("Enter phone number:");
            phone = Console.ReadLine();

            Client client;
            if (name != "" && phone != "")
            {
                client = new Client
                {
                    Id = id,
                    Name = name,
                    Phone = phone

                };
            }
            else if (name != "" && phone == "")
            {
                client = new Client
                {
                    Id = id,
                    Name = name

                };

            }
            else
            {
                client = new Client
                {
                    Id = id,
                    Phone = phone

                };

            }

            bl.UpdateClient( client);
        }

        /// <summary>
        /// Update that package has been collected
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num">drone serial number</param>
        public static void updateCollect(BlApi.IBL bl, out bool check, out uint num)
        {
            Console.Write("Enter drone number: ");

            do
            {
                check = uint.TryParse(Console.ReadLine(), out num);
            } while (!check);
            bl.CollectPackegForDelivery(num);
        }

        /// <summary>
        /// Updated package associated
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num">drone serial number</param>
        public static void UpdateAssociate(BlApi.IBL bl, out bool check, out uint num)
        {
            //received the details from the user

            Console.Write("Enter drone number:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out num);
            } while (!check);

            bl.ConnectPackegeToDrone(num);
        }

        /// <summary>
        /// Update drone name
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num">serial number</param>
        /// <param name="name">model</param>
        public static void UpdateDroneName(BlApi.IBL bl, out bool check, out uint num)
        {
            //received the details from the user
            Console.Write("Enter serial number: ");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out num);
            } while (!check);
            int i = 0;
            foreach (var model in Enum.GetValues(typeof(DroneModel)))
            {
                Console.WriteLine(i + " " + model);
                i++;
            }
            DroneModel model1;
            do
            {
                check = Enum.TryParse(Console.ReadLine(), out model1);
                if (!check)
                    Console.WriteLine("Error input");
            } while (!check);
            bl.UpdateDroneName(num, model1);
        }

        /// <summary>
        /// update drone to be in charging
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="serial">serial number</param>
        public static void updateCharge(BlApi.IBL bl, out bool check, out uint serial)
        {
            Console.Write("Enter serial number:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out serial);
            } while (!check);

            bl.DroneToCharge(serial);
        }

        /// <summary>
        /// update drone to be uncharging
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="serial">serial number</param>
        public static void UpdateUnCharge(BlApi.IBL bl, out bool check, out uint serial,out double time)
        {
            TimeSpan timeInCharge;
            Console.Write("Enter serial number:");
            do
            {
                check = uint.TryParse(Console.ReadLine(), out serial);
            } while (!check);

            Console.Write("Enter how long it is in charge in haours:");
            do
            {
                check = double.TryParse(Console.ReadLine(), out time);
                   timeInCharge= TimeSpan.FromHours(time);
            } while (!check);

            bl.FreeDroneFromCharging(serial);
        }

        /// <summary>
        /// Displays a distance between two points
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="doubleNum1">variable for calculating the points</param>
        public static void Distans(BlApi.IBL bl, out bool check, out double doubleNum1)
        {
            check = true;
            doubleNum1 = 0;
            Location location1 = new Location(), location2 = new Location();
            for (int i = 1; i < 3; i++)
            {
                Console.WriteLine("Please enter point for location{0}", i);
                do
                {

                    Console.Write("Please enter latitude point");
                    check = double.TryParse(Console.ReadLine(), out doubleNum1);
                }
                while (!check);
                location1.Latitude = doubleNum1;
                do
                {

                    Console.Write("Please enter longitude point");
                    check = double.TryParse(Console.ReadLine(), out doubleNum1);
                }
                while (!check);
                location1.Longitude = doubleNum1;

            }
            Console.WriteLine("The distans is:{0}KM", bl.Distans(location1, location2));
        }

        /// <summary>
        /// Base statons list
        /// </summary>
        /// <param name="bl"></param>
        public static void ShowBaseList(BlApi.IBL bl)
        {
            int i = 0;
            foreach (var _base in bl.BaseStationToLists())
            {
                i++;
                Console.WriteLine("#{0}", i);
                Console.WriteLine(_base);

            }
            Console.WriteLine("Total stations on the list:{0}\n", i);
        }

        /// <summary>
        /// show List of base staions with free states
        /// </summary>
        /// <param name="bl"></param>
        public static void ShowFreeBaseStation(BlApi.IBL bl)
        {
            int i = 0;
            foreach (var _base in bl.BaseStationWhitFreeChargingStationToLists())
            {
                i++;
                Console.WriteLine("#{0}", i);
                Console.WriteLine(_base);

            }
            Console.WriteLine("Total stations on the list:{0}\n", i);
        }

        /// <summary>
        /// show List of packages that not associated to drone
        /// </summary>
        /// <param name="bl"></param>
        public static void ShowFreePackage(BlApi.IBL bl)
        {
            int i = 0;
            foreach (var pack in bl.PackageWithNoDroneToLists())
            {
                i++;
                Console.WriteLine("#{0}", i);
                Console.WriteLine(pack);

            }
            Console.WriteLine("Total packege with no drone on the list:{0}\n", i);
        }

        /// <summary>
        /// show Package list
        /// </summary>
        /// <param name="bl"></param>
        public static void ShowPackageList(BlApi.IBL bl)
        {
            int i = 0;
            foreach (var pack in bl.PackageToLists())
            {
                i++;
                Console.WriteLine("#{0}", i);
                Console.WriteLine(pack);

            }
            Console.WriteLine("Total packeges on the list:{0}\n", i);
        }

        /// <summary>
        /// show client list
        /// </summary>
        /// <param name="bl"></param>
        public static void ShowClientList(BlApi.IBL bl)
        {
            int i = 0;
            foreach (var client in bl.ClientToLists())
            {
                i++;
                Console.WriteLine("#{0}", i);
                Console.WriteLine(client);

            }
            Console.WriteLine("Total client on the list:{0}\n", i);
        }

        /// <summary>
        /// show drone list
        /// </summary>
        /// <param name="bl"></param>
        public static void ShowDroneList(BlApi.IBL bl)
        {
            int i = 0;
            foreach (var drone in bl.DroneToLists())
            {
                i++;
                Console.WriteLine("#{0}", i);
                Console.WriteLine(drone);

            }
            Console.WriteLine("Total drones on the list:{0}\n", i);
        }

        /// <summary>
        /// Releasing drones from a station
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="check"></param>
        /// <param name="num">base station serial number</param>
        /// <param name="ex"> Update Charging Positions Exception</param>
        public static void freeBaseFromDrone(BlApi.IBL bl, out bool check, ref uint num, UpdateChargingPositionsException ex)
        {
            check = true;
            string chose = "";
            try
            {
                do
                {
                    Console.WriteLine("Do you want to release number of drone in this base?\n Please enter yes\\no");
                    chose = Console.ReadLine();

                    if (chose == "yes")
                    {
                        Console.WriteLine("How match drones you want to pull out?");
                        do
                        {
                            check = uint.TryParse(Console.ReadLine(), out num);
                        } while (!check);

                        bl.FreeBaseFromDrone(ex.BaseNumber, (int)num);


                    }
                    else if (chose == "no")
                    {
                        break;
                    }
                    else
                        chose = "";
                } while (chose == "");
            }
            catch (BO.TryToPullOutMoreDrone exc)
            {
                Console.WriteLine(exc);
            }
        }

        static void Main(string[] args)
        {
           
            BlApi.IBL bl =BlFactory.GetBl();
            Menu(bl);
        }
    }
}



