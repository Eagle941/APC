using System;

namespace APCCF6
{
    public class Aircraft
    {
        private Tuple<int, EnumClasses.Mass> weight;
        private EnumClasses.Status airConditioning;
        private EnumClasses.AntiIce antiIce;
        private int flaps;
        private int cg;
        private EnumClasses.Thrust impC;
        
        public Aircraft(int weight, int flaps, int cg, EnumClasses.Mass uOM, EnumClasses.Status ac, EnumClasses.AntiIce ai, EnumClasses.Thrust th)
        {
            this.weight = Tuple.Create(weight, uOM);
            this.flaps = flaps;
            this.antiIce = ai;
            this.airConditioning = ac;
            this.cg = cg;
            this.impC = th;
        }

        public Tuple<int, EnumClasses.Mass> getWeight()
        {
            return this.weight;
        }
        public int getFlaps()
        {
            return this.flaps;
        }
        public EnumClasses.Status getAirConditioning()
        {
            return this.airConditioning;
        }
        public int getCG()
        {
            return this.cg;
        }
        public EnumClasses.AntiIce getAntiIce()
        {
            return this.antiIce;
        }
        public EnumClasses.Thrust getImpC()
        {
            return this.impC;
        }

        public void setWeight(int w, EnumClasses.Mass uOM)
        {
            this.weight = Tuple.Create(w, uOM);
        }
        public void setFlaps(int f)
        {
            this.flaps = f;
        }
        public void setAirConditioning(EnumClasses.Status ac)
        {
            this.airConditioning = ac;
        }
        public void setCG(int cg)
        {
            this.cg = cg;
        }
        public void setAntiIce(EnumClasses.AntiIce ai)
        {
            this.antiIce = ai;
        }
        public void setImpC(EnumClasses.Thrust th)
        {
            this.impC = th;
        }

    }
}
