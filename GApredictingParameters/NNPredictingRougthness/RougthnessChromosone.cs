using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPredictingRougthness
{
    class RougthnessChromosone
    {
        List<Double> coeffs;
        private double NumberOfGenes = 4 + 10000;
        public double fitness = double.MaxValue;
        public static double mutationMagnitude = 0.1;
        public static double mutationChancePerValue = 0.5;
        public double mutateChance = 0.5;
        public double crossoverRate = 0.5;
        public double bestRoughtness = -1;

        public RougthnessChromosone()
        {
            coeffs = new List<Double>();

            coeffs.Add(GreyImageList.initeSpeed());
            coeffs.Add(GreyImageList.initFeed());
            coeffs.Add(GreyImageList.initDepth());
            coeffs.Add(GreyImageList.initGa());
            for (int x = 4; x < NumberOfGenes; x++)
            {
                coeffs.Add(initializeValue());
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
            return 0.8 + GeneticAlgo.random.NextDouble() * 0.2 ;
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
            for (int x = 0; x < 4; x++)
            {
                double r = GeneticAlgo.random.NextDouble();
                if (r < mutationChancePerValue)
                {
                    double curD = coeffs[x];

                    curD = Math.Max(0, (curD - (GeneticAlgo.random.NextDouble() * 1 - (1 / 2.0))));

                    coeffs[x] = curD;
                }
            }

            for (int x = 4; x < NumberOfGenes; x++)
            {
                double r = GeneticAlgo.random.NextDouble();
                if (r < mutationChancePerValue)
                {
                    double curD = coeffs[x];

                    curD = Math.Min(Math.Max(0, (curD - (GeneticAlgo.random.NextDouble() * 0.1 - (0.1 / 2.0)))),1);

                    coeffs[x] = curD;
                }
            }
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
