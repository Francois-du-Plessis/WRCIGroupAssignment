using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPredictingRougthness
{
    class Program
    {
        static GreyImageList greyImageList;
        static GeneticAlgo GA;

        static void Main(string[] args)
        {
            greyImageList = new GreyImageList();
            GA = new GeneticAlgo();
            RunGA();
            Console.ReadLine();
        }

        static void RunGA()
        {
            int counter = 1;
            double prevError = Double.MaxValue;
            int changeCounter = 0;

            do
            {
                foreach (RougthnessChromosone curModel in GA.population)
                {
                    double totalFitness = 0;
                    foreach (GreyImage greyImage in greyImageList.GetTestGreyImages())
                    {
                        double predictedRougthness = curModel.CalculateRougthness(greyImage);
                        totalFitness += Math.Pow(greyImage.surface.getRa() - predictedRougthness,2);
                    }
                    curModel.SetFitness(totalFitness);
                }

                GA.NextGeneration();

                if (counter % 100 == 0)
                {
                    Console.WriteLine(counter + " | Current Fittest: " + GA.population[0].GetFitness());
                }
                counter++;
            } while (counter < 100000);


                foreach (GreyImage greyImage in greyImageList.GetTestGreyImages())
                {
                    double predictedRougthness = GA.population[0].CalculateRougthness(greyImage);
                    Console.WriteLine("Actual ra: {0} | Predicted ra: {1} | Difference: {2}", greyImage.surface.getRa() , predictedRougthness, greyImage.surface.getRa() - predictedRougthness);
                }         
        }
    }
}
