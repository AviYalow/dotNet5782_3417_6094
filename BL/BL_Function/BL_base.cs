using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalApi;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using static BL.Cloning;

namespace BlApi
{
   partial class BL : IBL
    {
        /// <summary>
        /// calculation the most collset base station to a particular location
        /// </summary>
        /// <param name="location"> particular location</param>
        /// <param name="toCharge">if we locking base for charging</param>
        /// <returns> the most collset base station </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation ClosestBase(Location location,bool toCharge=false)
        {
            lock (dalObj)
            {
                BaseStation baseStation = new BaseStation();
                baseStation.Location = new Location();
                try
                {

                    Location base_location = new Location();

                    double? distans = null, distans2;
                    foreach (DO.Base_Station base_ in dalObj.BaseStationList(x => x.Active))
                    {
                        base_location.Latitude = base_.latitude;
                        base_location.Longitude = base_.longitude;
                        distans2 = Distans(location, base_location);
                        if (toCharge)
                        {
                            if (base_.NumberOfChargingStations <= 0)
                                continue;
                        }
                        if ((distans > distans2 || distans == null))
                        {

                            distans = distans2;
                            baseStation = new BaseStation
                            {
                                Location = base_location.Clone(),
                                Name = base_.NameBase,
                                SerialNum = base_.baseNumber,
                                FreeState = base_.NumberOfChargingStations,
                                DronesInChargeList = null
                            };

                        }
                    }
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw (new ItemNotFoundException(ex));
                }
                return baseStation;
            }
        }

        /// <summary>
        /// geting location for specific base station
        /// </summary>
        /// <param name="base_number"> serial number of base station</param>
        /// <returns> Location of the base station</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Location BaseLocation(uint base_number)
        {
            lock (dalObj)
            {
                DO.Base_Station base_Station = new DO.Base_Station();
                try
                {
                    base_Station = dalObj.BaseStationByNumber(base_number);
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw (new ItemNotFoundException(ex));
                }
                Location base_location = new Location();

                base_location.Latitude = base_Station.latitude;
                base_location.Longitude = base_Station.longitude;
                return base_location;
            }
        }

        /// <summary>
        /// add base station
        /// </summary>
        /// <param name="baseStation"> serial number of the base station</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBase(BaseStation baseStation)
        {
            chckingPoint(baseStation.Location);

            lock (dalObj)
            {
                try
                {
                    dalObj.AddStation(new DO.Base_Station
                    {
                        baseNumber = baseStation.SerialNum,
                        NameBase = baseStation.Name,
                        NumberOfChargingStations = baseStation.FreeState,
                        latitude = baseStation.Location.Latitude,
                        longitude = baseStation.Location.Longitude,
                        Active = true
                    });
                }
                catch (DO.ItemFoundException ex)
                {
                    throw (new ItemFoundExeption(ex));
                }
            }
        }
        /// <summary>
        /// check if the locatiton is currect
        /// </summary>
        /// <param name="location"></param>
        private static void chckingPoint(Location location)
        {
            if (Math.Abs(location.Latitude) > 90)
                throw new InputErrorException();
            if (Math.Abs(location.Longitude) > 180)
                throw new InputErrorException();
        }

        /// <summary>
        /// update base station
        /// </summary>
        /// <param name="base_">serial number of the base station</param>
        /// <param name="newName"> new name</param>
        /// <param name="newNumber"> charging states</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBase(uint base_, string newName, string newNumber)
        {
            lock (dalObj)
            {
                var baseUpdat = new DO.Base_Station();
            try
            {
                baseUpdat = dalObj.BaseStationByNumber(base_);
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
            if (newName != "")
                baseUpdat.NameBase = newName;
            if (newNumber != "")
            {
                uint number;
                bool flag;
                flag = uint.TryParse(newNumber, out number);
                if (!flag)
                    throw new InputErrorException();
                int droneInCharge = dalObj.ChargingDroneList(x => x.idBaseStation == base_).Count();
                baseUpdat.NumberOfChargingStations = (droneInCharge <= number) ?
                   ( number-(uint)droneInCharge) : throw new UpdateChargingPositionsException(droneInCharge, base_);

            }
            dalObj.UpdateBase(baseUpdat);
        }
        }

        /// <summary>
        /// search a specific station
        /// </summary>
        /// <param name="baseNume"> serial number</param>
        /// <returns> base station </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation BaseByNumber(uint baseNume)
        {
            lock (dalObj)
            {
                try
            {
                var baseStation = dalObj.BaseStationByNumber(baseNume);
                var baseReturn = new BaseStation { SerialNum = baseNume, Location = new Location { Latitude = baseStation.latitude, Longitude = baseStation.longitude }, Name = baseStation.NameBase, FreeState = baseStation.NumberOfChargingStations };

                baseReturn.DronesInChargeList = new ObservableCollection<DroneInCharge>(DroneINChargePerBase(baseNume));

              
                return baseReturn;
            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
        }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneInCharge> DroneINChargePerBase(uint base_)
        {
            lock (dalObj)
            {
                return from drone in dalObj.ChargingDroneList(x => x.idBaseStation == base_)
                   let butrry = (SpecificDrone(drone.IdDrone).ButrryStatus + droneChrgingAlredy((DateTime.Now - drone.EntringDrone).TotalMilliseconds))
                   let newButrry = (butrry > 100) ? 100 : butrry
                   select new DroneInCharge { ButrryStatus = newButrry.Value, SerialNum = drone.IdDrone };
        }
        }


        /// <summary>
        /// delete base station
        /// </summary>
        /// <param name="base_">serial number</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteBase(uint base_)
        {
            lock (dalObj)
            {
                try
            {

                dalObj.DeleteBase(base_);

            }
            catch (DO.ItemNotFoundException ex)
            {
                throw new ItemNotFoundException(ex);
            }
            catch (Exception)
            {

            }
        }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStationToList BaseStationWhitSpscificDrone(uint drone)
        {
            lock (dalObj)
            {
                return dalObj.BaseStationByNumber(dalObj.ChargingDroneList(x => x.IdDrone == drone)
                .FirstOrDefault().idBaseStation).convertBaseInDalToBaseStationList(dalObj);
            }

        }

    }
}
