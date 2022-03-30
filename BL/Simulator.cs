using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using static BlApi.BL;
using BO;
using BlApi;



namespace BL
{
    /// <summary>
    /// Simulator of drone travel
    /// </summary>
    internal class Simulator
    {

        BlApi.BL BL;
        private Thread myThread;
        Stopwatch sw;
        private const double SPEED = 500;
        private const double HOUER_TO_MILISECOUND = 60.0 * 60.0 * 1000;
        static Dictionary<uint, uint> keys = new();
       

        public Simulator(BlApi.BL bl, uint droneNumber, Action action, Func<bool> StopChecking)
        {
            sw = new Stopwatch();
            Drone drone;
            bool sendToCharge = false;
            double timePastUntilNwo = 0,distansePast=0;
            SpeedDrone speed=SpeedDrone.Free;
            const uint SEND_DRONE_TO_CHARGE = 5;
            uint sendToChargeCounter = 5;
            try
            {
                BL = bl;

                new Thread(() =>
                {
                  try  {
                        //check the same drone dont activ the simolator towice
                        if (keys.Any(x => x.Key == droneNumber))
                            throw new DroneTryToStartSecondeSimolatorException(droneNumber);
                        keys.Add(droneNumber, droneNumber);
                        myThread = Thread.CurrentThread;
                        while (StopChecking())
                        {
                            lock (bl)
                            {
                                if (!sw.IsRunning)
                                    sw.Start();
                                drone = bl.GetDrone(droneNumber);

                                switch (drone.DroneStatus)
                                {
                                    case BO.DroneStatus.Free:

                                        try
                                        {
                                           //if the drone buttry is low then20 or all packeges are delivery or more then 5 try to connect to packege
                                            if ((drone.ButrryStatus <= 20 || sendToCharge||sendToChargeCounter==0) && drone.ButrryStatus < 100)
                                            {
                                                BaseStation base_ = new();
                                                if (drone.LocationNext==LocationNext.None)
                                                base_ = bl.ClosestBase(drone.Location, true);
                                                drone.LocationNext = LocationNext.Base;
                                                
                                                 distansePast = distanse(timePastUntilNwo, SpeedDrone.Free);
                                             
                                                if (drone.DistanseToNextLocation -distansePast<=0)
                                                {
                                                    sw.Stop();
                                                    drone.ButrryStatus -= bl.buttryDownWithNoPackege(drone.DistanseToNextLocation);
                                                    sw.Reset();
                                                    sendToChargeCounter = SEND_DRONE_TO_CHARGE;
                                                    bl.DroneToCharge(drone.SerialNumber, drone.DistanseToNextLocation);
                                                    drone.DistanseToNextLocation = 0;
                                                }
                                                else
                                                {
                                                    drone.DistanseToNextLocation -= distanse(timePastUntilNwo, SpeedDrone.Free);
                                                    changeDroneList(bl, drone);
                                                    sendToCharge = false;
                                                   
                                                }
                                                timePastUntilNwo = sw.ElapsedMilliseconds;
                                            }
                                            else
                                            {
                                                stopWatch();
                                                bl.ConnectPackegeToDrone(droneNumber);
                                            }

                                        }
                                        catch (DroneCantMakeDliveryException)
                                        {
                                            lock (bl.dalObj)
                                            {
                                                if(!sendToCharge)
                                                if (bl.dalObj.PackegeList(x => true).All(x => x.OperatorSkimmerId != 0))
                                                    sendToCharge = true;
                                                if (sendToChargeCounter > 0)
                                                    sendToChargeCounter--;
                                            }
                                        }
                                        catch (Exception)
                                        {

                                        }

                                        break;
                                    case BO.DroneStatus.Maintenance:

                                        var buttry = bl.droneChrgingAlredy(sw.ElapsedMilliseconds - timePastUntilNwo) + drone.ButrryStatus;
                                        if (buttry >= 100)
                                        {
                                            stopWatch();
                                            bl.FreeDroneFromCharging(drone.SerialNumber, buttry);
                                        }
                                        else
                                        {
                                            drone.ButrryStatus = buttry;
                                            changeDroneList(bl, drone);
                                        }
                                        break;
                                    case BO.DroneStatus.Work:
                                     
                                        if (drone.PackageInTransfer.InTheWay)
                                        {

                                            switch (drone.PackageInTransfer.WeightCatgory)
                                            {
                                                case WeightCategories.Easy:
                                              
                                                    distansePast= distanse(timePastUntilNwo, SpeedDrone.Easy);
                                                    speed = SpeedDrone.Easy;
                                                    break;
                                                case WeightCategories.Medium:
                                           
                                                   distansePast= distanse(timePastUntilNwo, SpeedDrone.Medium);
                                                    speed = SpeedDrone.Medium;
                                                    break;
                                                case WeightCategories.Heavy:
                                        
                                                   distansePast= distanse(timePastUntilNwo, SpeedDrone.Heavy);
                                                    speed = SpeedDrone.Heavy;
                                                    break;
                                                default:
                                                    break;
                                            }

                                            if (drone.DistanseToNextLocation-distansePast <= 0)
                                            {
                                                stopWatch();
                                                bl.PackegArrive(droneNumber, drone.DistanseToNextLocation);
                                                drone.DistanseToNextLocation = 0;
                                            }
                                            else
                                            {
                                                drone.ButrryStatus -= bl.buttryDownPackegeDelivery(drone.PackageInTransfer, distanse(timePastUntilNwo, speed));
                                                drone.DistanseToNextLocation -= distansePast;
                                                changeDroneList(bl, drone);
                                            }
                                        }
                                        else
                                        {

                                            distansePast = distanse(timePastUntilNwo, SpeedDrone.Free);

                                            var a = bl.Distans(drone.Location, drone.PackageInTransfer.Source);
                                            if (drone.DistanseToNextLocation-distansePast<=0)
                                            {
                                                stopWatch();
                                             
                                                bl.CollectPackegForDelivery(droneNumber, drone.DistanseToNextLocation);
                                                drone.DistanseToNextLocation = 0;
                                            }
                                            else
                                            {
                                                drone.ButrryStatus -= bl.buttryDownWithNoPackege(distanse(timePastUntilNwo, SpeedDrone.Free));
                                                drone.DistanseToNextLocation -= distansePast;
                                               changeDroneList(bl, drone);
                                            }
                                        }
                                        timePastUntilNwo = sw.ElapsedMilliseconds;

                                        break;
                                    case BO.DroneStatus.Delete:
                                        break;
                                    default:
                                        break;
                                }

                            }
                            action();
                            Thread.Sleep(500);
                        }
                        keys.Remove(droneNumber);
                        for (int i = 0; i < 10; i++)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    catch (ItemNotFoundException)
                    { keys.Remove(droneNumber); }
                    catch (DroneTryToStartSecondeSimolatorException)
                    {  }
                    catch (Exception) { }
                }).Start();

            }
            catch (ItemNotFoundException)
            { keys.Remove(droneNumber); }
            catch (DroneTryToStartSecondeSimolatorException)
            { }
            catch (Exception) { }
        }

       
        /// <summary>
        /// update drone list
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="drone"></param>
        private  void changeDroneList(BlApi.BL bl, Drone drone)
        {
            var droneInList = bl.dronesListInBl.Find(x => x.SerialNumber == drone.SerialNumber);
            droneInList.ButrryStatus = drone.ButrryStatus;
            droneInList.DistanseToNextLocation = drone.DistanseToNextLocation;
            droneInList.LocationNext = drone.LocationNext;
            droneInList.LocationName = drone.LocationName;
        }

       

          /// <summary>
        /// Calculate the distance the drone did to the charge
        /// </summary>
        /// <param name="m"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        private double distanse(double m, SpeedDrone speed)
        {
            return (sw.ElapsedMilliseconds - m)*SPEED * (((double)speed / HOUER_TO_MILISECOUND));
        }

        /// <summary>
        /// Stop the timer, and reset him
        /// </summary>
        private void stopWatch()
        {
            sw.Stop();
            sw.Reset();
        }
    }

}