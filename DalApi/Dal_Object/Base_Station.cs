using System;



namespace DO
{
    /// <Base_Station>
    /// An entity that will represent 
    /// a base station for the drones
    /// </Base_Station>
    public struct Base_Station
    {
        public uint baseNumber { get; init; }
        public String NameBase { get; set; }
        public uint NumberOfChargingStations { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public bool Active { get; set; }




        public override string ToString()
        {
            String print_Base_Station = "";
            print_Base_Station += $"Base Number: {baseNumber},\n";
            print_Base_Station += $"Name Base: {NameBase},\n";
            print_Base_Station += $"Number of charging stations: {NumberOfChargingStations},\n";

            //A view based on a sexagesimal of the coordinate values
            print_Base_Station += $"Longitude Status: {Point.Degree(longitude)},\n";
            print_Base_Station += $"Latitude Status: {Point.Degree(latitude)}\n";
            return print_Base_Station;
        }



    }


}
