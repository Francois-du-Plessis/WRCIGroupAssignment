using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPredictingRougthness
{
    class RougthnessChromosone
    {
        List<Double> coeffs;
        private double NumberOfGenes = 9;
        public double fitness = double.MaxValue;
        public static double mutationMagnitude = 1;
        public static double mutationChancePerValue = 1;
        public double mutateChance = 0.9;
        public double crossoverRate = 0.9;

        public RougthnessChromosone()
        {
            coeffs = new List<Double>();
            for(int x = 0; x < 5; x++)
            {
                coeffs.Add(initializeValue());
            }

            for (int x = 5; x < NumberOfGenes; x++)
            {
                coeffs.Add(1 + GeneticAlgo.random.NextDouble() * 0.2 - 0.1);
            }

        }

        public RougthnessChromosone(List<Double> coefficients)
        {
            coeffs = new List<Double>();

            foreach (Double curD in coefficients)
            {
                coeffs.Add(curD);
            }

        }


        public double initializeValue()
        {
            return GeneticAlgo.random.NextDouble() *2 - 1 ;
        }

        public RougthnessChromosone(RougthnessChromosone parent1, RougthnessChromosone parent2)
        {
            coeffs = new List<Double>();

            double r = GeneticAlgo.random.NextDouble();
            if (r < crossoverRate)
            {
                for (int x = 0; x < NumberOfGenes; x++)
                {
                    r = GeneticAlgo.random.NextDouble();
                    //                if(r < 0.5){
                    //                    coeffs.add(parent1.coeffs.get(x));
                    //                }else{
                    //                    coeffs.add(parent2.coeffs.get(x));
                    //                }

                    coeffs.Add(r * parent1.coeffs[x] + (1 - r) * parent2.coeffs[x]);
                }
            }
            else {
                for (int x = 0; x < NumberOfGenes; x++)
                {
                    coeffs.Add(parent1.coeffs[x]);
                }
            }

            r = GeneticAlgo.random.NextDouble();

            if (r < mutateChance)
            {
                MUTATE();
            }

        }

        public void MUTATE()
        {
            for (int x = 0; x < NumberOfGenes; x++)
            {
                double r = GeneticAlgo.random.NextDouble();
                if (r < mutationChancePerValue)
                {
                    double curD = coeffs[x];

                    curD = (curD - (GeneticAlgo.random.NextDouble() * mutationMagnitude - (mutationMagnitude / 2.0)));

                    coeffs[x] = curD;
                }
            }

        }

        public double CalculateRougthness(GreyImage gI)
        {
            Surface curSurface = gI.surface;
            //return coeffs[0] + (coeffs[1] * curSurface.getSpeed()) + (coeffs[2] * curSurface.getFeed()) + (coeffs[3] * curSurface.getDepth()) + (coeffs[4] * curSurface.getGa());
            return coeffs[0] + coeffs[1] * Math.Pow( curSurface.getSpeed(), coeffs[5]) + coeffs[2] * Math.Pow( curSurface.getFeed(), coeffs[6]) + coeffs[3] * Math.Pow( curSurface.getDepth(), coeffs[7]) + coeffs[4] * Math.Pow( curSurface.getGa(), coeffs[8]);
        }

        public RougthnessChromosone Clone()
        {
            return new RougthnessChromosone(this.coeffs);
        }

        public List<Double> GetCoefs()
        {
            return coeffs;
        }

        public double GetFitness()
        {
            return fitness;
        }

        public void SetFitness(double fitness)
        {
            this.fitness = fitness;
        }

        public void DrawImage()
        {
            byte[] pixels = new byte[10000];
            for(int x = 0; x < 10000; x++)
            {
                pixels[x] = Convert.ToByte(Math.Ceiling((coeffs[x + 4])*255));
            }
            GreyImageList.DrawImage(pixels, @"C:\Users\Francois\Desktop\TestImage.bmp");
        }
    }
}
