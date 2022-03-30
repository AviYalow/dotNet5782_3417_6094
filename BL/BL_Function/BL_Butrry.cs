using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalApi;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace BlApi
{
    partial class BL : IBL
    {


        /// <summary>
        /// Calculate how much percentage of battery is needed to delivery a package
        /// </summary>
        /// <param name="packageInTransfer"> package</param>
        /// <returns> percentage of battery needed</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal double buttryDownPackegeDelivery(PackageInTransfer packageInTransfer, double distance = 0)
        {

            double battery_drop = 0;
            distance = distance == 0 ? packageInTransfer.Distance : distance;
            switch (packageInTransfer.WeightCatgory)
            {
                case WeightCategories.Easy:
                    battery_drop = butrryBySpeedAndElectic(distance, SpeedDrone.Easy, easyElctric);
                    break;
                case WeightCategories.Medium:
                    battery_drop = butrryBySpeedAndElectic(distance, SpeedDrone.Medium, mediomElctric);
                    break;
                case WeightCategories.Heavy:
                    battery_drop = butrryBySpeedAndElectic(distance, SpeedDrone.Heavy, heaviElctric);
                    break;
                default:
                    break;

            }
            return battery_drop;
        }
        /// <summary>
        /// clculete buttry by distas div. speed get time multy. elctric
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="speed"></param>
        /// <param name="elctricUsed"></param>
        /// <returns></returns>
        private double butrryBySpeedAndElectic(double distance, SpeedDrone speed, double elctricUsed)
        {
            return ((distance / ((double)speed)) * (double)ButrryPer.Minute) * elctricUsed;
        }

        /// <summary>
        /// calculate how match butrry drone need to get from base to client or client to base
        /// </summary>
        /// <param name="fromLocation"> source location</param>
        /// <param name="toLocation"> destination location</param>
        /// <returns> percentage of battery needed</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal double buttryDownWithNoPackege(Location fromLocation, Location toLocation)
        {
            lock (dalObj)
            {
                double distans = Distans(fromLocation, toLocation);
                double buttry = butrryBySpeedAndElectic(distans, SpeedDrone.Free, freeElctric);
                return buttry;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal double buttryDownWithNoPackege(double distance)
        {
            lock (dalObj)
            {

                return butrryBySpeedAndElectic(distance, SpeedDrone.Free, freeElctric);
               
            }
        }

        /// <summary>
        /// send drone to charge
        /// </summary>
        /// <param name="droneNumber">serial number of drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneToCharge(uint droneNumber, double? distanse = null)
        {

            lock (dalObj)
            {
                var drone = SpecificDrone(droneNumber);
                BaseStation baseStation = ClosestBase(drone.Location, true);
                if (drone.DroneStatus != DroneStatus.Free)
                {
                    throw new DroneStillAtWorkException();
                }

                double buttry = distanse is null ? buttryDownWithNoPackege(drone.Location, baseStation.Location) :
                  buttryDownWithNoPackege(distanse.Value);
                if (drone.ButrryStatus - buttry < 0)
                {
                    throw new NoButrryToTripException(buttry);
                }

                try
                {

                    if (baseStation.FreeState <= 0)
                        throw (new NoPlaceForChargeException(baseStation.SerialNum));
                    dalObj.DroneToCharge(droneNumber, baseStation.SerialNum);
                    drone.ButrryStatus -= buttry;
                    drone.DistanseToNextLocation = 0;
                    drone.LocationName = LocationName.Base;
                    drone.DroneStatus = DroneStatus.Maintenance;
                    drone.Location = baseStation.Location;
                    dronesListInBl[dronesListInBl.FindIndex(x => x.SerialNumber == droneNumber)] = drone;

                    drone.LocationNext = LocationNext.None;


                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw (new ItemNotFoundException(ex));
                }
                catch (DO.ItemFoundException ex)
                {
                    throw new ItemFoundExeption(ex);
                }
            }

        }

        /// <summary>
        ///Charging drone function 
        /// </summary>
        /// <param name="droneNumber">serial number of the drone</param>
        /// <param name="timeInCharge"> the time that the drone in charge </param>
        /// <returns> butrry Status of the  drone</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double FreeDroneFromCharging(uint droneNumber, double number = -1)
        {
            lock (dalObj)
            {
                //locking for drone
                var drone = dronesListInBl.Find(x => x.SerialNumber == droneNumber);
                if (drone == null)
                    throw new ItemNotFoundException("Drone", droneNumber);
                //locking the drone in charge
                DO.BatteryLoad? information = dalObj.ChargingDroneList(x => x.IdDrone == droneNumber).FirstOrDefault();

                if (information is null)
                    throw new ItemNotFoundException("Drone", droneNumber);
                //calcoulet how mach he chraging alredy
                double buttry = number == -1 ? droneChrgingAlredy((DateTime.Now - information.Value.EntringDrone).TotalMilliseconds) :
                        number;

                drone.ButrryStatus = buttry > 100 ? 100 : buttry + drone.ButrryStatus;
                drone.DroneStatus = DroneStatus.Free;

                dalObj.FreeDroneFromCharge(drone.SerialNumber);
                dronesListInBl[dronesListInBl.FindIndex(x => x.SerialNumber == drone.SerialNumber)] = drone;


                return drone.ButrryStatus.Value;
            }

        }

        /// <summary>
        /// Release a drone from a charger at a particular base station
        /// </summary>
        /// <param name="baseNumber"> serial number of the base station</param>
        /// <param name="number"> amount of drone to release</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void FreeBaseFromDrone(uint baseNumber, int number = -1)
        {
            lock (dalObj)
            {
                if (number != -1)

                    try
                    {
                        if (dalObj.ChargingDroneList(x => x.idBaseStation == baseNumber).Count() - number < 0)
                        {
                            throw (new TryToPullOutMoreDrone());
                        }
                    }
                    catch (DO.ItemNotFoundException ex)
                    {
                        throw new ItemNotFoundException(ex);
                    }


                int i = 0;
                var returnDrone = new DroneInCharge();
                List<DroneInCharge> list = new List<DroneInCharge>();
                foreach (var droneChrging in dalObj.ChargingDroneList(x => x.idBaseStation == baseNumber))
                {
                    if (number != -1)
                    {
                        if (i <= number)
                        {
                            //FreeDroneFromCharging(droneChrging.IdDrone, droneChrging.EntringDrone - DateTime.Now);
                            FreeDroneFromCharging(droneChrging.IdDrone);
                            i++;

                        }
                    }
                    else
                        //FreeDroneFromCharging(droneChrging.IdDrone, droneChrging.EntringDrone - DateTime.Now);
                        FreeDroneFromCharging(droneChrging.IdDrone);
                }
            }

        }

        /// <summary>
        /// Calculates the percentage of battery of the drone based on the charging time it was
        /// </summary>
        /// <param name="span">charging time the drone was</param>
        /// <returns> percentage of battery</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal double droneChrgingAlredy(double span)
        {
            var time = (span); var butrryPerMinute = chargingPerMinute / 1000;
            time *= butrryPerMinute;
            return time;
        }

        /// <summary>
        /// Calculate how much percentage of battery the drone will need for full shipping
        /// </summary>
        /// <param name="drone"> drone</param>
        /// <param name="package"> package</param>
        /// <returns> percentage of battery </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        double batteryCalculationForFullShipping(Location drone, Package package)
        {
            lock (dalObj)
            {
                return buttryDownWithNoPackege(drone, ClientLocation(package.SendClient.Id)) + buttryDownPackegeDelivery(convertPackegeBlToPackegeInTrnansfer(package)) +
                buttryDownWithNoPackege(ClosestBase(ClientLocation(package.RecivedClient.Id)).Location, ClientLocation(package.RecivedClient.Id));
            }
        }
    }
}
