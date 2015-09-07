using System;
using Microsoft.VisualBasic.FileIO;

namespace APCCF6
{
    class Calc
    {
        private Airport airport;
        private Weather weather;
        private Aircraft aircraft;
        private Tuple<int, EnumClasses.Wind> windComponent;
        private Tuple<int, EnumClasses.Length> pressureAltitude;
        private Tuple<int, EnumClasses.Length> elevationCorrection;
        private int xWind;
        //
        private Tuple<int, EnumClasses.Mass> pressureAltitudeTakeOffLimitWeight;
        private Tuple<int, EnumClasses.Length> runwaySlopeFieldLength;
        private Tuple<int, EnumClasses.Length> runwayWindFieldLength;
        private Tuple<int, EnumClasses.Length> runwayFlapsFieldLength;
        private Tuple<int, EnumClasses.Mass> fieldLengthLimitWeight;
        //
        private Tuple<int, EnumClasses.Mass> pressureAltitudeClimbLimitWeight;
        private Tuple<int, EnumClasses.Mass> takeOffClimbLimit;

        public Calc(Airport apt, Weather wx, Aircraft ac)
        {
            this.airport = apt;
            this.weather = wx;
            this.aircraft = ac;
            this.xWind = calcCrossWind();
            this.windComponent = calcWind();
            this.elevationCorrection = calcElevationCorrection();
            this.pressureAltitude = calcPressureAltitude();
            //
            this.pressureAltitudeTakeOffLimitWeight = calcPressureAltitudeTakeOffLimitWeight();
            this.runwaySlopeFieldLength = calcRunwaySlopeLength();
            this.runwayWindFieldLength = calcRunwayWindLength();
            this.runwayFlapsFieldLength = calcRunwayFlapsLength();
            this.fieldLengthLimitWeight = calcTakeOffFieldLengthLimit();
            //
            this.pressureAltitudeClimbLimitWeight = calcPressureAltitudeClimbLimitWeight();
            this.takeOffClimbLimit = calcTakeOffClimbLimit();
        }

        public Airport getAirport()
        {
            return this.airport;
        }
        public Weather getWeather()
        {
            return this.weather;
        }
        public Aircraft getAircraft()
        {
            return this.aircraft;
        }
        public Tuple<int, EnumClasses.Wind> getWindComponent()
        {
            return this.windComponent;
        }
        public int getXWind()
        {
            return this.xWind;
        }
        public Tuple<int, EnumClasses.Length> getPressureAltitude()
        {
            return this.pressureAltitude;
        }
        public Tuple<int, EnumClasses.Length> getElevationCorrection()
        {
            return this.elevationCorrection;
        }
        public Tuple<int, EnumClasses.Length> getRunwaySlopeLength()
        {
            return this.runwaySlopeFieldLength;
        }
        public Tuple<int, EnumClasses.Length> getRunwayWindLength()
        {
            return this.runwayWindFieldLength;
        }
        public Tuple<int, EnumClasses.Length> getRunwayFlapsLength()
        {
            return runwayFlapsFieldLength;
        }
        public Tuple<int, EnumClasses.Mass> getPressureAltitudeTakeOffLimitWeight()
        {
            return pressureAltitudeTakeOffLimitWeight;
        }
        public Tuple<int, EnumClasses.Mass> getTakeOffLengthLimitWeight()
        {
            return this.fieldLengthLimitWeight;
        }
        public Tuple<int, EnumClasses.Mass> getPressureAltitudeClimbLimitWeight()
        {
            return this.pressureAltitudeClimbLimitWeight;
        }
        public Tuple<int, EnumClasses.Mass> getTakeOffClimbLimit()
        {
            return this.takeOffClimbLimit;
        }

        public void setAirport(Airport apt)
        {
            this.airport = apt;
            this.xWind = calcCrossWind();
            this.windComponent = calcWind();
        }
        public void setWeather(Weather wx)
        {
            this.weather = wx;
            this.xWind = calcCrossWind();
            this.windComponent = calcWind();
        }
        public void setAircraft(Aircraft ac)
        {
            this.aircraft = ac;
        }

        public int calcCrossWind()
        {
            Tuple<int, int> wind = this.weather.getWind();
            int windDir = wind.Item1;
            int windSpeed = wind.Item2;
            int rwyDir = this.airport.getRunwayDir();

            int dirDiff = Math.Abs(windDir - rwyDir);

            int crossWind = (int) (Math.Ceiling(Math.Abs(Math.Sin(MyMath.toRadians(dirDiff)) * windSpeed)));

            return crossWind;
        }
        public Tuple<int, EnumClasses.Wind> calcWind()
        {
            Tuple<int, int> wind = this.weather.getWind();
            int windDir = wind.Item1;
            int windSpeed = wind.Item2;
            int rwyDir = this.airport.getRunwayDir();

            //int rwyDirPlusNinety = rwyDir >= 270 ? rwyDir - 270 : rwyDir + 90;
            //int rwyDirMinusNinety = rwyDir < 90 ? 450 - rwyDir : rwyDir - 90;

            int rwyDirPlusNinety = rwyDir + 90;
            int rwyDirMinusNinety = rwyDir - 90;

            EnumClasses.Wind isHeadWind = rwyDirMinusNinety < windDir & windDir <= rwyDirPlusNinety ? EnumClasses.Wind.HWind : EnumClasses.Wind.TWind;

            int dirAngle;

            if (isHeadWind == EnumClasses.Wind.HWind)
            {
                dirAngle = Math.Abs(windDir - rwyDir);
            }
            else
            {
                int rwyPlusOneHundredEighty = rwyDir >= 180 ? rwyDir - 180 : rwyDir + 180;
                dirAngle = Math.Abs(windDir - rwyPlusOneHundredEighty);
            }

            //int dirDiff = Math.Abs(windDir - rwyDir);

            int windComponent = (int) (Math.Ceiling(Math.Cos(MyMath.toRadians(dirAngle)) * windSpeed));

            return Tuple.Create(windComponent, isHeadWind);
        }

        public Tuple<int, EnumClasses.Length> calcElevationCorrection()
        {
            if (this.weather.getQNH().Item2 == EnumClasses.Pressure.inHg)
            {
                this.weather.setQNH(Conversion.toMBar(this.weather.getQNH()));
            }
            //Working with mBar and feet
            int press = (int) (this.weather.getQNH().Item1);

            int correction = 0;

            if (press < 976)
            {
                correction = 1100;
            }
            else if (press > 1052)
            {
                correction = -1100;
            }
            else
            {
                switch (press)
                {
                    case 976:
                        correction = 1050;
                        break;
                    case 977:
                    case 978:
                        correction = 1000;
                        break;
                    case 979:
                        correction = 950;
                        break;
                    case 980:
                    case 981:
                    case 982:
                        correction = 900;
                        break;
                    case 983:
                        correction = 850;
                        break;
                    case 984:
                    case 985:
                        correction = 800;
                        break;
                    case 986:
                        correction = 750;
                        break;
                    case 987:
                    case 988:
                    case 989:
                        correction = 700;
                        break;
                    case 990:
                        correction = 650;
                        break;
                    case 991:
                    case 992:
                    case 993:
                        correction = 600;
                        break;
                    case 994:
                        correction = 550;
                        break;
                    case 995:
                    case 996:
                        correction = 500;
                        break;
                    case 997:
                        correction = 450;
                        break;
                    case 998:
                    case 999:
                    case 1000:
                        correction = 400;
                        break;
                    case 1001:
                        correction = 350;
                        break;
                    case 1002:
                    case 1003:
                        correction = 300;
                        break;
                    case 1004:
                        correction = 250;
                        break;
                    case 1005:
                    case 1006:
                    case 1007:
                        correction = 200;
                        break;
                    case 1008:
                        correction = 150;
                        break;
                    case 1009:
                    case 1010:
                    case 1011:
                        correction = 100;
                        break;
                    case 1012:
                        correction = 50;
                        break;
                    case 1013:
                    case 1014:
                        correction = 0;
                        break;
                    case 1015:
                        correction = -50;
                        break;
                    case 1016:
                    case 1017:
                    case 1018:
                        correction = -100;
                        break;
                    case 1019:
                        correction = -150;
                        break;
                    case 1020:
                    case 1021:
                        correction = -200;
                        break;
                    case 1022:
                        correction = -250;
                        break;
                    case 1023:
                    case 1024:
                    case 1025:
                        correction = -300;
                        break;
                    case 1026:
                        correction = -350;
                        break;
                    case 1027:
                    case 1028:
                    case 1029:
                        correction = -400;
                        break;
                    case 1030:
                        correction = -450;
                        break;
                    case 1031:
                    case 1032:
                    case 1033:
                        correction = -500;
                        break;
                    case 1034:
                        correction = -550;
                        break;
                    case 1035:
                    case 1036:
                        correction = -600;
                        break;
                    case 1037:
                        correction = -650;
                        break;
                    case 1038:
                    case 1039:
                    case 1040:
                        correction = -700;
                        break;
                    case 1041:
                        correction = -750;
                        break;
                    case 1042:
                    case 1043:
                    case 1044:
                        correction = -800;
                        break;
                    case 1045:
                        correction = -850;
                        break;
                    case 1046:
                    case 1047:
                        correction = -900;
                        break;
                    case 1048:
                        correction = -950;
                        break;
                    case 1049:
                    case 1050:
                    case 1051:
                        correction = -1000;
                        break;
                    case 1052:
                        correction = -1050;
                        break;
                }
            }

            return Tuple.Create(correction, EnumClasses.Length.ft);
        }
        
        public Tuple<int, EnumClasses.Length> calcPressureAltitude()
        {
            if(this.airport.getRunwayElevation().Item2 == EnumClasses.Length.m)
            {
                this.airport.setRunwayElevation(Conversion.toFeet(this.airport.getRunwayElevation()));
            }
            if(this.elevationCorrection.Item2 == EnumClasses.Length.m)
            {
                this.elevationCorrection = Conversion.toFeet(elevationCorrection);
            }

            return Tuple.Create(this.airport.getRunwayElevation().Item1 + elevationCorrection.Item1, EnumClasses.Length.ft);
        }

        public Tuple<int, EnumClasses.Mass> calcPressureAltitudeTakeOffLimitWeight()
        {
            Tuple<int, EnumClasses.Temperature> oat = this.weather.getOAT();
            Tuple<int, EnumClasses.Length> pressureAltitude = this.pressureAltitude;

            if (oat.Item2 != EnumClasses.Temperature.Celsius)
            {
                oat = Conversion.toCelsius(oat);
            }
            if (pressureAltitude.Item2 != EnumClasses.Length.ft)
            {
                pressureAltitude = Conversion.toFeet(pressureAltitude);
            }

            int floor = MyMath.myFloor(pressureAltitude.Item1);
            int ceiling = MyMath.myCeiling(pressureAltitude.Item1);

            //Open pressure altitude floor
            //Open pressure altitude ceiling
            TextFieldParser floorFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Pressure Altitude\\Pressure Altitude " + floor + ".csv");

            int floorLimitWeight = 0;

            floorFile.TextFieldType = FieldType.Delimited;
            floorFile.SetDelimiters(",");

            while (!floorFile.EndOfData)
            {
                string[] row = floorFile.ReadFields();
                int oatInCSV = Convert.ToInt32(row[0]);
                if (oatInCSV == oat.Item1)
                {
                    floorLimitWeight = Convert.ToInt32(row[1]);
                    break;
                }
            }

            floorFile.Close();

            int limitWeight = floorLimitWeight;

            if (floor != ceiling)
            {
                TextFieldParser ceilingFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Pressure Altitude\\Pressure Altitude " + ceiling + ".csv");

                int ceilingLimitWeight = 0;

                ceilingFile.TextFieldType = FieldType.Delimited;
                ceilingFile.SetDelimiters(",");

                while (!ceilingFile.EndOfData)
                {
                    string[] row = ceilingFile.ReadFields();
                    int oatInCSV = Convert.ToInt32(row[0]);
                    if (oatInCSV == oat.Item1)
                    {
                        ceilingLimitWeight = Convert.ToInt32(row[1]);
                        break;
                    }
                }

                ceilingFile.Close();

                int targetPressureAltitude = pressureAltitude.Item1;
                int diffFloor = targetPressureAltitude - floor;

                int diffLimitWeight = Math.Abs(floorLimitWeight - ceilingLimitWeight);
                double toAdd = diffLimitWeight / 1000.0 * diffFloor;
                limitWeight += (int)toAdd;
            }

            return Tuple.Create(limitWeight, EnumClasses.Mass.Kg);
        }

        public Tuple<int, EnumClasses.Length> calcRunwaySlopeLength()
        {
            Tuple<int, EnumClasses.Length> runwayLength = this.airport.getRunwayLength();

            if (runwayLength.Item2 != EnumClasses.Length.ft)
            {
                runwayLength = Conversion.toFeet(runwayLength);
            }

            double slope = this.airport.getSlope();

            int floor = MyMath.myFloor(runwayLength.Item1);
            int ceiling = MyMath.myCeiling(runwayLength.Item1);

            TextFieldParser floorFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Runway Slope\\Runway Slope " + floor + ".csv");

            int floorLimitLength = 0;

            floorFile.TextFieldType = FieldType.Delimited;
            floorFile.SetDelimiters(",");

            while (!floorFile.EndOfData)
            {
                string[] row = floorFile.ReadFields();
                double slopeInCSV = Convert.ToDouble(row[1]);
                if (slopeInCSV == slope)
                {
                    floorLimitLength = Convert.ToInt32(row[0]);
                    break;
                }
            }

            floorFile.Close();

            int limitLength = floorLimitLength;

            if (floor != ceiling)
            {
                TextFieldParser ceilingFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Runway Slope\\Runway Slope " + ceiling + ".csv");

                int ceilingLimitLength = 0;

                ceilingFile.TextFieldType = FieldType.Delimited;
                ceilingFile.SetDelimiters(",");

                while (!ceilingFile.EndOfData)
                {
                    string[] row = ceilingFile.ReadFields();
                    double slopeInCSV = Convert.ToDouble(row[1]);
                    if (slopeInCSV == slope)
                    {
                        ceilingLimitLength = Convert.ToInt32(row[0]);
                        break;
                    }
                }

                ceilingFile.Close();

                int targetLength = runwayLength.Item1;
                int diffFloor = targetLength - floor;

                int diffLimitLength = ceilingLimitLength - floorLimitLength;
                double toAdd = diffLimitLength / 1000.0 * diffFloor;
                limitLength += (int)toAdd;
            }

            return Tuple.Create(limitLength, EnumClasses.Length.ft);
        }

        public Tuple<int, EnumClasses.Length> calcRunwayWindLength()
        {
            Tuple<int, EnumClasses.Length> runwaySlopeLength = this.runwaySlopeFieldLength;

            if (runwaySlopeLength.Item2 != EnumClasses.Length.ft)
            {
                runwaySlopeLength = Conversion.toFeet(runwaySlopeLength);
            }

            int wind = this.windComponent.Item2 == EnumClasses.Wind.HWind ? this.windComponent.Item1 : -this.windComponent.Item1;

            int floor = MyMath.myFloor(runwaySlopeLength.Item1);
            int ceiling = MyMath.myCeiling(runwaySlopeLength.Item1);

            TextFieldParser floorFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Wind \\Wind " + floor + ".csv");

            int floorLimitLength = 0;

            floorFile.TextFieldType = FieldType.Delimited;
            floorFile.SetDelimiters(",");

            while (!floorFile.EndOfData)
            {
                string[] row = floorFile.ReadFields();
                double windInCSV = Convert.ToDouble(row[1]);
                if (windInCSV == wind)
                {
                    floorLimitLength = Convert.ToInt32(row[0]);
                    break;
                }
            }

            floorFile.Close();

            int limitLength = floorLimitLength;

            if (floor != ceiling)
            {
                TextFieldParser ceilingFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Wind\\Wind " + ceiling + ".csv");

                int ceilingLimitLength = 0;

                ceilingFile.TextFieldType = FieldType.Delimited;
                ceilingFile.SetDelimiters(",");

                while (!ceilingFile.EndOfData)
                {
                    string[] row = ceilingFile.ReadFields();
                    double windInCSV = Convert.ToDouble(row[1]);
                    if (windInCSV == wind)
                    {
                        ceilingLimitLength = Convert.ToInt32(row[0]);
                        break;
                    }
                }

                ceilingFile.Close();

                int targetLength = runwaySlopeLength.Item1;
                int diffFloor = targetLength - floor;

                int diffLimitLength = ceilingLimitLength - floorLimitLength;
                double toAdd = diffLimitLength / 1000.0 * diffFloor;
                limitLength += (int)toAdd;
            }

            return Tuple.Create(limitLength, EnumClasses.Length.ft);
        }

        public Tuple<int, EnumClasses.Length> calcRunwayFlapsLength()
        {
            Tuple<int, EnumClasses.Length> runwayWindLength = this.runwayWindFieldLength;

            if (runwayWindLength.Item2 != EnumClasses.Length.ft)
            {
                runwayWindLength = Conversion.toFeet(runwayWindLength);
            }

            int flaps = this.aircraft.getFlaps();

            int floor = MyMath.myFloor(runwayWindLength.Item1);
            int ceiling = MyMath.myCeiling(runwayWindLength.Item1);

            TextFieldParser floorFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Flaps \\Flaps " + floor + ".csv");

            int floorLimitLength = 0;

            floorFile.TextFieldType = FieldType.Delimited;
            floorFile.SetDelimiters(",");

            while (!floorFile.EndOfData)
            {
                string[] row = floorFile.ReadFields();
                double flapsInCSV = Convert.ToDouble(row[1]);
                if (flapsInCSV == flaps)
                {
                    floorLimitLength = Convert.ToInt32(row[0]);
                    break;
                }
            }

            floorFile.Close();

            int limitLength = floorLimitLength;

            if (floor != ceiling)
            {
                TextFieldParser ceilingFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Flaps\\Flaps " + ceiling + ".csv");

                int ceilingLimitLength = 0;

                ceilingFile.TextFieldType = FieldType.Delimited;
                ceilingFile.SetDelimiters(",");

                while (!ceilingFile.EndOfData)
                {
                    string[] row = ceilingFile.ReadFields();
                    double flapsInCSV = Convert.ToDouble(row[1]);
                    if (flapsInCSV == flaps)
                    {
                        ceilingLimitLength = Convert.ToInt32(row[0]);
                        break;
                    }
                }

                ceilingFile.Close();

                int targetLength = runwayWindLength.Item1;
                int diffFloor = targetLength - floor;

                int diffLimitLength = ceilingLimitLength - floorLimitLength;
                double toAdd = diffLimitLength / 1000.0 * diffFloor;
                limitLength += (int)toAdd;
            }

            return Tuple.Create(limitLength, EnumClasses.Length.ft);
        }

        public Tuple<int, EnumClasses.Mass> calcTakeOffFieldLengthLimit()
        {

            if (this.runwayFlapsFieldLength.Item2 != EnumClasses.Length.ft)
            {
                this.runwayFlapsFieldLength = Conversion.toFeet(this.runwayFlapsFieldLength);
            }

            if(pressureAltitudeTakeOffLimitWeight.Item2 != EnumClasses.Mass.Kg)
            {
                this.pressureAltitudeTakeOffLimitWeight = Conversion.toKilogram(this.pressureAltitudeTakeOffLimitWeight);
            }

            int floorWeight = MyMath.myFloor(pressureAltitudeTakeOffLimitWeight.Item1 / 10) * 10;
            int ceilingWeight = MyMath.myCeiling(pressureAltitudeTakeOffLimitWeight.Item1 / 10) * 10;
            int targetWeight = this.pressureAltitudeTakeOffLimitWeight.Item1;
            int diffFloorWeight = targetWeight - floorWeight;

            int floorLength = MyMath.myFloor(this.runwayFlapsFieldLength.Item1*10)/10;
            int ceilingLength = MyMath.myCeiling(this.runwayFlapsFieldLength.Item1*10)/10;
            int targetLength = this.runwayFlapsFieldLength.Item1;
            int diffFloorLength = targetLength - floorLength;

            TextFieldParser floorFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Field Length Limit \\Field Length Limit " + floorWeight + ".csv");
            TextFieldParser ceilingFile = new TextFieldParser("..\\..\\FPPM AME_Page_026\\Field Length Limit \\Field Length Limit " + ceilingWeight + ".csv");

            int floorLimitWeight = 0;
            int ceilingLimitWeight = 0;

            floorFile.TextFieldType = FieldType.Delimited;
            floorFile.SetDelimiters(",");

            while (!floorFile.EndOfData)
            {
                string[] row = floorFile.ReadFields();
                double lengthInCSV = Convert.ToDouble(row[0]);
                if (lengthInCSV == floorLength)
                {
                    floorLimitWeight = Convert.ToInt32(row[1]);
                }
                else if(lengthInCSV == ceilingLength)
                {
                    ceilingLimitWeight = Convert.ToInt32(row[1]);
                }
            }

            floorFile.Close();

            double toAdd = (ceilingLimitWeight - floorLimitWeight) / 100.0 * 90;
            int floorFieldLengthLimitWeight = floorLimitWeight + (int) toAdd;

            ceilingFile.TextFieldType = FieldType.Delimited;
            ceilingFile.SetDelimiters(",");

            while (!ceilingFile.EndOfData)
            {
                string[] row = ceilingFile.ReadFields();
                double lengthInCSV = Convert.ToDouble(row[0]);
                if (lengthInCSV == floorLength)
                {
                    floorLimitWeight = Convert.ToInt32(row[1]);
                }
                else if (lengthInCSV == ceilingLength)
                {
                    ceilingLimitWeight = Convert.ToInt32(row[1]);
                }
            }

            ceilingFile.Close();

            toAdd = (ceilingLimitWeight - floorLimitWeight) / 100.0 * 90;
            int ceilingFieldLengthLimitWeight = floorLimitWeight + (int) toAdd;

            toAdd = (ceilingFieldLengthLimitWeight - floorFieldLengthLimitWeight) / 10000.0 * diffFloorWeight;
            int fieldLengthLimitWeight = floorFieldLengthLimitWeight + (int) toAdd;

            if(this.aircraft.getAirConditioning() == EnumClasses.Status.Off)
            {
                fieldLengthLimitWeight += 450;
            }
            if(this.aircraft.getAntiIce() == EnumClasses.AntiIce.Engine)
            {
                fieldLengthLimitWeight -= 1400;
            }
            if(this.aircraft.getAntiIce() == EnumClasses.AntiIce.EngineWing)
            {
                fieldLengthLimitWeight -= 1900;
            }

            return Tuple.Create(fieldLengthLimitWeight, EnumClasses.Mass.Kg);
        }

        public Tuple<int, EnumClasses.Mass> calcPressureAltitudeClimbLimitWeight()
        {
            Tuple<int, EnumClasses.Temperature> oat = this.weather.getOAT();
            Tuple<int, EnumClasses.Length> pressureAltitude = this.pressureAltitude;

            if (oat.Item2 != EnumClasses.Temperature.Celsius)
            {
                oat = Conversion.toCelsius(oat);
            }
            if (pressureAltitude.Item2 != EnumClasses.Length.ft)
            {
                pressureAltitude = Conversion.toFeet(pressureAltitude);
            }

            int floor = MyMath.myFloor(pressureAltitude.Item1);
            int ceiling = MyMath.myCeiling(pressureAltitude.Item1);

            TextFieldParser floorFile = new TextFieldParser("..\\..\\FPPM AME_Page_027\\Pressure Altitude\\Pressure Altitude " + floor + ".csv");

            int floorLimitWeight = 0;

            floorFile.TextFieldType = FieldType.Delimited;
            floorFile.SetDelimiters(",");

            if(oat.Item1 >= 0)
            {
                while (!floorFile.EndOfData)
                {
                    string[] row = floorFile.ReadFields();
                    int oatInCSV = Convert.ToInt32(row[0]);
                    if (oatInCSV == oat.Item1)
                    {
                        floorLimitWeight = Convert.ToInt32(row[1]);
                        break;
                    }
                }

                floorFile.Close();
            }
            else
            {
                while (!floorFile.EndOfData)
                {
                    string[] row = floorFile.ReadFields();
                    int oatInCSV = Convert.ToInt32(row[0]);
                    if (oatInCSV == 0)
                    {
                        floorLimitWeight = Convert.ToInt32(row[1]);
                        break;
                    }
                }

                floorFile.Close();


            }

            int limitWeight = floorLimitWeight;

            if (floor != ceiling)
            {
                TextFieldParser ceilingFile = new TextFieldParser("..\\..\\FPPM AME_Page_027\\Pressure Altitude\\Pressure Altitude " + ceiling + ".csv");

                ceilingFile.TextFieldType = FieldType.Delimited;
                ceilingFile.SetDelimiters(",");

                int ceilingLimitWeight = 0;

                if(oat.Item1 >= 0)
                {
                    while (!ceilingFile.EndOfData)
                    {
                        string[] row = ceilingFile.ReadFields();
                        int oatInCSV = Convert.ToInt32(row[0]);
                        if (oatInCSV == oat.Item1)
                        {
                            ceilingLimitWeight = Convert.ToInt32(row[1]);
                            break;
                        }
                    }

                    ceilingFile.Close();
                }
                else
                {
                    while (!ceilingFile.EndOfData)
                    {
                        string[] row = ceilingFile.ReadFields();
                        int oatInCSV = Convert.ToInt32(row[0]);
                        if (oatInCSV == 0)
                        {
                            ceilingLimitWeight = Convert.ToInt32(row[1]);
                            break;
                        }
                    }

                    ceilingFile.Close();
                }

                int targetPressureAltitude = pressureAltitude.Item1;
                int diffFloor = targetPressureAltitude - floor;

                int diffLimitWeight = Math.Abs(floorLimitWeight - ceilingLimitWeight);
                double toAdd = diffLimitWeight / 1000.0 * diffFloor;
                limitWeight += (int)toAdd;
            }

            return Tuple.Create(limitWeight, EnumClasses.Mass.Kg);
        }

        public Tuple<int, EnumClasses.Mass> calcTakeOffClimbLimit()
        {
            Tuple<int, EnumClasses.Mass> pressureAltitudeClimbLimitWeight = this.pressureAltitudeClimbLimitWeight;
            int flaps = this.aircraft.getFlaps();
            EnumClasses.AntiIce antiIce = this.aircraft.getAntiIce();
            EnumClasses.Status packs = this.aircraft.getAirConditioning();

            if (pressureAltitudeClimbLimitWeight.Item2 != EnumClasses.Mass.Kg)
            {
                pressureAltitudeClimbLimitWeight = Conversion.toKilogram(pressureAltitudeClimbLimitWeight);
            }

            int floor;
            int ceiling;

            if(Convert.ToInt32(pressureAltitudeClimbLimitWeight.Item1.ToString()[2]) < 5)
            {
                ceiling = MyMath.myCeiling(pressureAltitudeClimbLimitWeight.Item1 / 10) * 10;
                floor = MyMath.myFloor(pressureAltitudeClimbLimitWeight.Item1 / 10) * 10 + 5000;
            }
            else
            {
                ceiling = MyMath.myFloor(pressureAltitudeClimbLimitWeight.Item1 / 10) * 10 + 5000;
                floor = MyMath.myFloor(pressureAltitudeClimbLimitWeight.Item1 / 10) * 10;
            }

            TextFieldParser floorFile = new TextFieldParser("..\\..\\FPPM AME_Page_027\\Flaps\\Flaps " + floor + ".csv");

            int floorLimitWeight = 0;

            floorFile.TextFieldType = FieldType.Delimited;
            floorFile.SetDelimiters(",");

            while (!floorFile.EndOfData)
            {
                string[] row = floorFile.ReadFields();
                int flapsInCSV = Convert.ToInt32(row[0]);
                if (flapsInCSV == flaps)
                {
                    floorLimitWeight = Convert.ToInt32(row[1]);
                    break;
                }
            }

            floorFile.Close();

            int limitWeight = floorLimitWeight;

            if (floor != ceiling)
            {
                TextFieldParser ceilingFile = new TextFieldParser("..\\..\\FPPM AME_Page_027\\Flaps\\Flaps " + ceiling + ".csv");

                ceilingFile.TextFieldType = FieldType.Delimited;
                ceilingFile.SetDelimiters(",");

                int ceilingLimitWeight = 0;

                while (!ceilingFile.EndOfData)
                {
                    string[] row = ceilingFile.ReadFields();
                    int flapsInCSV = Convert.ToInt32(row[0]);
                    if (flapsInCSV == flaps)
                    {
                        ceilingLimitWeight = Convert.ToInt32(row[1]);
                        break;
                    }
                }

                ceilingFile.Close();

                int targetPressureAltitude = pressureAltitudeClimbLimitWeight.Item1;
                int diffFloor = targetPressureAltitude - floor;

                int diffLimitWeight = Math.Abs(floorLimitWeight - ceilingLimitWeight);
                double toAdd = diffLimitWeight / 5000.0 * diffFloor;
                limitWeight += (int)toAdd;
            }
                        
            switch (flaps)
            {
                case 5:
                case 15:
                    if(antiIce == EnumClasses.AntiIce.Engine)
                    {
                        limitWeight -= 2000;
                    }
                    else if(antiIce == EnumClasses.AntiIce.EngineWing)
                    {
                        limitWeight -= 2400;
                    }
                    break;
                case 20:
                    if(antiIce == EnumClasses.AntiIce.Engine)
                    {
                        limitWeight -= 1900;
                    }
                    else if(antiIce == EnumClasses.AntiIce.EngineWing)
                    {
                        limitWeight -= 2300;
                    }
                    break;
            }
            
            if(packs == EnumClasses.Status.Off)
            {
                limitWeight += 1700;
            }

            return Tuple.Create(limitWeight, EnumClasses.Mass.Kg);
        }
    }
}
