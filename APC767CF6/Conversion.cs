using System;

namespace APCCF6
{
    class Conversion : EnumClasses
    {
        public static Tuple<int, Length> toMeter(Tuple<int, Length> feet)
        {
            int meter = (int) ((double) feet.Item1 * 0.3048);

            return Tuple.Create(meter, Length.m);
        }
        public static Tuple<int, Length> toFeet(Tuple<int, Length> meter)
        {
            int feet = (int)((double)meter.Item1 * 3.2808);

            return Tuple.Create(feet, Length.ft);
        }

        public static Tuple<double, Pressure> toInHg(Tuple<double, Pressure> mBar)
        {
            double inHg = Math.Round(mBar.Item1 * 0.0295301, 2);

            return Tuple.Create(inHg, Pressure.inHg); 
        }
        public static Tuple<double, Pressure> toMBar(Tuple<double, Pressure> inHg)
        {
            double mBar = Math.Round(inHg.Item1 * 33.863753, 0);

            return Tuple.Create(mBar, Pressure.mBar);
        }

        public static Tuple<int, Temperature> toCelsius(Tuple<int, Temperature> fahrenheit)
        {
            int celsius = (int)((double)(fahrenheit.Item1 - 32) * 5.0/9.0);

            return Tuple.Create(celsius, Temperature.Celsius);
        }
        public static Tuple<int, Temperature> toFahrenheit(Tuple<int, Temperature> celsius)
        {
            int fahrenheit = (int)((double)celsius.Item1 * 9.0 / 5.0) + 32;

            return Tuple.Create(fahrenheit, Temperature.Fahrenheit);
        }

        public static Tuple<int, Mass> toPound(Tuple<int, Mass> kilogram)
        {
            int pound = (int)((double)kilogram.Item1 * 2.204622476);

            return Tuple.Create(pound, Mass.lb);
        }
        public static Tuple<int, Mass> toKilogram(Tuple<int, Mass> pound)
        {
            int kilogram = (int)((double)pound.Item1 * 0.4535924);

            return Tuple.Create(kilogram, Mass.lb);
        }
    }
}
