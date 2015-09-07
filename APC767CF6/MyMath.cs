using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APCCF6
{
    class MyMath
    {
        public static int myFloor(int number)
        {
            double thousand = number / 1000.0;

            return (int) Math.Floor(thousand) * 1000;
        }

        public static int myCeiling(int number)
        {
            double thousand = number / 1000.0;

            return (int)Math.Ceiling(thousand) * 1000;
        }
        public static double toRadians(int angle)
        {
            return (Math.PI / 180.0) * angle;
        }
    }
}
