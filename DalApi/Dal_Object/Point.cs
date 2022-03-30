using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace DO
    {
        /// <Point>
        /// A class that represents points on a sexagesimal 
        /// of the coordinate values
        /// </Point>
       public class Point
        {
         

            //Conversion of points in decimal base to base sexagesimal
            public static string Degree(double point)
            {
                point = (point < 0) ? point * (-1) : point;
                uint d = (uint)point;
                uint m = (uint)((point - d) * 60);
                double mph = (double)((double)m / 60);
                double s = (point - d - mph) * 3600;
                return $"{d}\x00B0 {m}' {s:0.0000}\"";
            }

            //A function that calculates distance at sea given two points
         
            public static  double Distance(double longitude1, double latitude1, double longitude2, double latitude2)
            {
                var R = 6371; // Radius of the earth in km
                var dLat = (latitude2 - latitude1) * (Math.PI / 180);

                var dLon = (longitude2 - longitude1) * (Math.PI / 180);
                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos((latitude1) * (Math.PI / 180)) * Math.Cos((latitude2) * (Math.PI / 180))
                    * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return R * c; // Distance in km return d;

            }

             
        }
    }

