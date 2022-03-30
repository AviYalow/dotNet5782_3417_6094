using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using Ds;
using System.Runtime.CompilerServices;

namespace Dal
{
    sealed partial class DalObject : DalApi.IDal
    {

        /// <summary>
        /// Adding a new package
        /// </summary>
        /// <param name="package"></param>
        /// <returns>Returns the serial number of the new package</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public uint AddPackage(Package package)
        {
            var package_num = DataSource.Config.package_num;
            if (DataSource.packages.Any(x => x.SerialNumber == package_num))
                throw new ItemFoundException("Packege", package_num);
            package.ReceivingDelivery = DateTime.Now;
            package.CollectPackageForShipment =null;
            package.PackageArrived = null;
            package.PackageAssociation = null;
            DataSource.packages.Add(new Package { SerialNumber= DataSource.Config.package_num,SendClient=package.SendClient,GetingClient= package.GetingClient,
                 Priority= package.Priority,ReceivingDelivery=package.ReceivingDelivery,WeightCatgory=package.WeightCatgory,OperatorSkimmerId=0,
                CollectPackageForShipment= package.CollectPackageForShipment,PackageArrived= package.PackageArrived,PackageAssociation= package.PackageAssociation
            });
            DataSource.Config.package_num++;
            return DataSource.Config.package_num-1;
        }

        /// <summary>
        /// connect package to drone
        /// </summary>
        /// <param name="packageNumber">serial number of the package that needs 
        /// to connect to drone </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ConnectPackageToDrone(uint packageNumber, uint drone_sirial_number)
        {
            int i = DataSource.packages.FindIndex(x=>x.SerialNumber==packageNumber);
            if (i==-1)
                 throw (new ItemNotFoundException("packege",packageNumber));
             if (!DataSource.drones.Any(x => x.SerialNumber == drone_sirial_number))
                 throw (new ItemNotFoundException("drone",drone_sirial_number));

            
          
            Package package = DataSource.packages[i];
            package.OperatorSkimmerId = drone_sirial_number;

            package.PackageAssociation = DateTime.Now;
            DataSource.packages[i] = package;
        


        }

        /// <summary>
        /// Updated package collected
        /// </summary>
        /// <param name="packageNumber">serial number of the package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PackageCollected(uint packageNumber)
        {
            int i = DataSource.packages.FindIndex(x=>x.SerialNumber==packageNumber);
            if (i == -1)
                throw (new ItemNotFoundException("package", packageNumber));

            
            Package package = DataSource.packages[i];
            package.CollectPackageForShipment = DateTime.Now;
            DataSource.packages[i] = package;
            
        }

        /// <summary>
        /// Update that package has arrived at destination
        /// </summary>
        /// <param name="packageNumber">serial number of the package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PackageArrived(uint packageNumber)
        {
            int i = DataSource.packages.FindIndex(x => x.SerialNumber == packageNumber);
            if (i == -1)
                throw (new ItemNotFoundException("package", packageNumber));

            Package package = DataSource.packages[i];
            package.PackageArrived = DateTime.Now;
            DataSource.packages[i] = package;
        }


        /// <summary>
        /// Display packege data 
        /// </summary>
        /// <param name="packageNumber">Serial number of a particular package</param>
        /// <returns> string of data</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Package packegeByNumber(uint packageNumber)
        {
            int i = DataSource.packages.FindIndex(x => x.SerialNumber == packageNumber);
            if (i == -1)
                throw (new ItemNotFoundException("package", packageNumber));
            return DataSource.packages[i];

        }

        /// <summary>
        /// return the list of all packages
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Package> PackegeList(Predicate<Package> predicate)
        {
            // return DataSource.packages.Where(predicate).Select(x => x);
            return from x in DataSource.packages
                   where predicate(x) 
                   select x;

        }



        /// <summary>
        /// delete a spsific packege
        /// </summary>
        /// <param name="sirial"> package number</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeletePackege(uint sirial)
        {
            int i = DataSource.packages.FindIndex(x => x.SerialNumber == sirial);
            if (i == -1)
                throw (new ItemNotFoundException("package", sirial));
            DataSource.packages.Remove(DataSource.packages[i]);
        }

        /// <summary>
        /// Updating fields of a particular package
        /// </summary>
        /// <param name="package">particular package</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePackege(Package package)
        {
            int i = DataSource.packages.FindIndex(x => x.SerialNumber == package.SerialNumber);
            if (i == -1)
                throw (new DO.ItemNotFoundException("Packege", package.SerialNumber));
            else
                DataSource.packages[i] = package;
        }

    }
}
