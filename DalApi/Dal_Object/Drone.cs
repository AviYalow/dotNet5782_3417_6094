using System;


    namespace DO
    {
        /// <Drone>
        /// An entity that will represent 
        /// a Drone for deliverly 
        /// </Drone>
        public struct Drone 
        {
            public uint SerialNumber { get; init; }
            public DroneModel Model { get; set; }
            public WeightCategories WeightCategory { get; set; }
            public bool Active { get; set; }
            
            




            public override string ToString()
            {
                String printDrown = "";
                printDrown += $"Siral Number is {SerialNumber},\n";
                printDrown += $"Model {Model}\n";
                printDrown += $"Weight Category is {WeightCategory},\n";
                
                return printDrown;
            }

        }

    }
