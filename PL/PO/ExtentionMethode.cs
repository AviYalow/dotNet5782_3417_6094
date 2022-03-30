using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PO;
using BO;

namespace PL
{
   static class ExtentionMethode
    {

        public static DroneItemModel dronefromBl(this BO.Drone model, DroneItemModel drone)
        {
            if (model is null)
                return null;
            if (drone is null)
                drone = new();

            drone.ButrryStatus = model.ButrryStatus;
            drone.DroneStatus = model.DroneStatus;

            drone.Location = model.Location;

            drone.Model = model.Model;

            drone.PackageInTransfer = model.PackageInTransfer;

            drone.SerialNumber = model.SerialNumber;

            drone.WeightCategory = model.WeightCategory;
            drone.DistanseToNextLocation = model.DistanseToNextLocation;
            drone.LocationName = model.LocationName;
            drone.LocationNext = model.LocationNext;
            return drone;
            


        }

        public static BaseStationModel BaseFromBl (this BaseStation base_ , BaseStationModel basePo)
        {
            if (base_ is null)
                return null;

            basePo.FreeState = base_.FreeState;
            basePo.Location = base_.Location;
            basePo.Name = base_.Name;
            basePo.SerialNum = base_.SerialNum;
            basePo.DronesInChargeList = base_.DronesInChargeList;
            return basePo;
            
        }

        public static ClientModel clientFromBl (this BO.Client client, ClientModel clientModel)
        {
            if (client is null)
                return null;

            clientModel.Active = client.Active;
            clientModel.Id = client.Id;
            clientModel.Location = client.Location;
            clientModel.Name = client.Name;
            foreach(var digit in client.Phone.Take(3))
            clientModel.StartPhone+=digit;
            foreach (var digit in client.Phone.Skip(3))
                clientModel.EndPhone+= digit;
            clientModel.FromClient = client.FromClient;
            clientModel.ToClient = client.ToClient;
            string a;
           
            return clientModel;
        }

        public static PackageModel packegeBlToMOdel (this Package package, PackageModel model)
        {
            if (package is null)
                return null;

            model.CollectPackage = package.CollectPackage;
             model.Create_package = package.Create_package;
          model.Drone = package.Drone;
        model.PackageArrived = package.PackageArrived;
           model.PackageAssociation = package.PackageAssociation;
          model.Priority = package.Priority;
           model.RecivedClient = package.RecivedClient;
            model.SendClient = package.SendClient;
          model.SerialNumber = package.SerialNumber;
           model.WeightCatgory = package.WeightCatgory;
            return model;
        }
    }
}
