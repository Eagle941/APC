using System;

namespace APCCF6
{
    class Airport
    {              
        private int rwyDir;
        private Tuple<int, EnumClasses.Length> rwyLength;
        private Tuple<int, EnumClasses.Length> rwyElevation;
        private double slope;

        public Airport(int rwyDir, int rwyLength, int rwyElevation, EnumClasses.Length lengthMeasure, double rwySlope)
        {
            this.rwyDir = rwyDir;
            this.rwyLength = Tuple.Create(rwyLength, lengthMeasure);
            this.rwyElevation = Tuple.Create(rwyElevation, lengthMeasure);
            this.slope = rwySlope;
        }

        public int getRunwayDir()
        {
            return this.rwyDir;
        }
        public Tuple<int, EnumClasses.Length> getRunwayLength()
        {
            return this.rwyLength;
        }
        public Tuple<int, EnumClasses.Length> getRunwayElevation()
        {
            return this.rwyElevation;
        }
        public double getSlope()
        {
            return this.slope;
        }

        public void setRunwayDir(int rwyDir)
        {
            this.rwyDir = rwyDir;
        }
        public void setRunwayLength(Tuple<int, EnumClasses.Length> length)
        {
            this.rwyLength = length;
        }
        public void setRunwayElevation(Tuple<int, EnumClasses.Length> rwyElevation)
        {
            this.rwyElevation = rwyElevation;
        }
        public void setSlope(double rwySlope)
        {
            this.slope = rwySlope;
        }
    }
}
