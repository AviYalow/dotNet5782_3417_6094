using System;


    namespace DO
    {
        /// <Package>
        /// Entity representing a package for delivery
        /// </Package>
        public struct Package
        {
            public uint SerialNumber { get; init; }
            public uint SendClient { get; set; }
            public uint GetingClient { get; set; }
            public WeightCategories WeightCatgory { get; set; }
            public Priority Priority { get; set; }
            public uint OperatorSkimmerId { get; set; }
           


            //Delivery time create a package
            public DateTime? ReceivingDelivery { get; set; }

            //Time to assign the package to a drone
            public DateTime? PackageAssociation { get; set; }

            //Package collection time from the sender
            public DateTime? CollectPackageForShipment { get; set; }

            //Time of arrival of the package to the recipient
            public DateTime? PackageArrived { get; set; }



         
            public override string ToString()
            {
                String printPackage = "";
                printPackage += $"Sirial Number is {SerialNumber},\n";
                printPackage += $"Send Client is {SendClient},\n";
                printPackage += $"Getting Client is {GetingClient},\n";
                printPackage += $"weight Catgory is {WeightCatgory},\n";
                printPackage += $"Priority is {Priority},\n";
                printPackage += $"operator skimmer ID is {OperatorSkimmerId},\n";
                printPackage += $"Receiving Delivery is {ReceivingDelivery},\n";

                //If the package was associated with a drone
                if (OperatorSkimmerId != 0)
                {
                    printPackage += $"Package Association is {PackageAssociation},\n";
                    //if the package have been collected
                    if (CollectPackageForShipment != new DateTime())
                    {
                        printPackage += $"collect package for shipment is {CollectPackageForShipment},\n";
                        // if the package arrived
                        if (PackageArrived != new DateTime())
                        {
                            printPackage += $"package_arrived is {PackageArrived}\n";
                        }
                        //if the package not arrived
                        else
                            printPackage += $"Shipping on the way \n";

                    }
                    //if the package haven't been collected
                    else
                        printPackage += "The shipment has not been collected yet\n";


                }
                else //If the package wasn't associated with a drone
                    printPackage += $"Package is not Association yet ,\n";
              
              
               

              


                return printPackage;
            }

        }

    }
