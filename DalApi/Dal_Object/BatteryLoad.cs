using System;


    namespace DO
    {
        /// <BtarryLoad>
        /// An entity that will represent a position
        /// for loading drones
        /// </BtarryLoad>
        public struct BatteryLoad
        {
            public uint idBaseStation { get; set; }

            public uint IdDrone { get; set; }

            public DateTime EntringDrone { get; init; }
            

            public override string ToString()
            {
                String printBtarryLoad = "";
                printBtarryLoad += $"ID BaseStation is {idBaseStation},\n";
                printBtarryLoad += $"ID drown is {IdDrone}\n";
                printBtarryLoad += $"Time of geting drone {EntringDrone}\n";
                
                return printBtarryLoad;
            }
        }

    }
