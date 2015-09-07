using System;

namespace APCCF6
{
    class Weather
    {
        private Tuple<int, int> wind;
        private Tuple<int, EnumClasses.Temperature> oat;
        private Tuple<double, EnumClasses.Pressure> qnh;

        public Weather(int windDir, int windVelocity, int oat, EnumClasses.Temperature tempUnity, double qnh, EnumClasses.Pressure pressUnity)
        {
            this.wind = Tuple.Create(windDir, windVelocity);
            this.oat = Tuple.Create(oat, tempUnity);
            this.qnh = Tuple.Create(qnh, pressUnity);
        }

        public Tuple<int, int> getWind()
        {
            return this.wind;
        }
        public Tuple<int, EnumClasses.Temperature> getOAT()
        {
            return this.oat;
        }        
        public Tuple<double, EnumClasses.Pressure> getQNH()
        {
            return this.qnh;
        }

        public void setWind(int windDir, int windVelocity)
        {
            this.wind = Tuple.Create(windDir, windVelocity);
        }
        public void setOAT(Tuple<int, EnumClasses.Temperature> oat)
        {
            this.oat = oat;
        }
        public void setQNH(Tuple<double, EnumClasses.Pressure> qnh)
        {
            this.qnh = qnh;
        }
    }
}
