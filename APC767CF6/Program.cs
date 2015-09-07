using System;

namespace APCCF6
{
    class Program
    {
        static void Main(string[] args)
        {
            Aircraft b767 = new Aircraft(150360, 15, 28, EnumClasses.Mass.Kg, EnumClasses.Status.On, EnumClasses.AntiIce.Off, EnumClasses.Thrust.Optimum);
            Weather omdb = new Weather(120, 20, 35, EnumClasses.Temperature.Celsius, 29.92, EnumClasses.Pressure.inHg);
            Airport _12l = new Airport(120, 10100, 2000, EnumClasses.Length.ft, 2);
            Calc calc = new Calc(_12l, omdb, b767);
            Console.WriteLine(calc.getXWind());
            Console.WriteLine(calc.getWindComponent());

            int number = 173696;
            //Console.WriteLine("Ceiling: " + MyMath.myCeiling(number/10)*10);
            Console.WriteLine("Ceiling: " + (MyMath.myFloor(number / 10) * 10 + 5000));
            Console.WriteLine("Floor: " + MyMath.myFloor(number/10)*10);

            Console.WriteLine(calc.getPressureAltitudeTakeOffLimitWeight());
            Console.WriteLine(calc.getRunwaySlopeLength());
            Console.WriteLine(calc.getRunwayWindLength());
            Console.WriteLine(calc.calcRunwayFlapsLength());
            Console.WriteLine(calc.calcTakeOffFieldLengthLimit());
            Console.WriteLine(calc.getPressureAltitudeClimbLimitWeight());
            Console.WriteLine(calc.getTakeOffClimbLimit());

            Console.ReadLine();
        }
    }
}
