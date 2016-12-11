using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPredictingRougthness
{
    class Program
    {
        static GreyImageList greyImageList;
        static NeuralNetwork NN;
        static GeneticAlgo GA;
        static void Main(string[] args)
        {
            greyImageList = new GreyImageList();
            //surfaceList = new SurfaceList();
            NN = new NeuralNetwork();
            TrainNN();
            GA = new GeneticAlgo();
            DetermineParameters();
            Console.ReadLine();
        }

        static void TrainNN()
        {
            int counter = 0;
            while (counter < 200)
            {
                counter++;

                double totalSSE = 0;
                double totalEvalSSE = 0;

                foreach (GreyImage greyImage in greyImageList.GetTestGreyImages())
                {
                    Surface surface = greyImage.surface;
                    List<double> predictedRougthness = NN.Predict(greyImage);
                    List<double> actualRougthness = new List<double>();
                    actualRougthness.Add(surface.getScaledRa());
                    NN.TrainNeuron(0.1, actualRougthness);
                    totalSSE = Math.Pow(GreyImageList.descaleRa(predictedRougthness[0]) - surface.getRa(), 2);
                }

                foreach (GreyImage greyImage in greyImageList.GetEvalGreyImages())
                {
                    Surface surface = greyImage.surface;
                    List<double> predictedRougthness = NN.Predict(greyImage);
                    List<double> actualRougthness = new List<double>();
                    actualRougthness.Add(surface.getScaledRa());
                    totalEvalSSE = Math.Pow(GreyImageList.descaleRa(predictedRougthness[0]) - surface.getRa(), 2);
                }

                if (counter%1 == 0)
                {
                    Console.WriteLine("{0} | TestSSE: {1} | EvalSSE: {2}",counter,totalSSE,totalEvalSSE);
                }
            }

            foreach (GreyImage greyImage in greyImageList.GetTestGreyImages())
            {
                Surface surface = greyImage.surface;
                List<double> predictedRougthness = NN.Predict(greyImage);
                Console.WriteLine("Actual RA: {0} | PredictedRA: {1} | Difference: {2}", surface.getRa(), GreyImageList.descaleRa(predictedRougthness[0]), surface.getRa()- GreyImageList.descaleRa(predictedRougthness[0]));
            }

            foreach (GreyImage greyImage in greyImageList.GetEvalGreyImages())
            {
                Surface surface = greyImage.surface;
                List<double> predictedRougthness = NN.Predict(greyImage);
                Console.WriteLine("Actual RA: {0} | PredictedRA: {1} | Difference: {2}", surface.getRa(), GreyImageList.descaleRa(predictedRougthness[0]), surface.getRa() - GreyImageList.descaleRa(predictedRougthness[0]));

            }
        }

        static void DetermineParameters()
        {
            double GivenRoughtness = 1.69;
            int counter = 1;
            double prevError = Double.MaxValue;
            int changeCounter = 0;

            do
            {
                foreach (RougthnessChromosone curModel in GA.population)
                {
                    double totalFitness = 0;
                    Surface surface = new Surface(curModel.GetCoefs()[0], curModel.GetCoefs()[1], curModel.GetCoefs()[2], curModel.GetCoefs()[3],-1);
                    GreyImage curPattern = new GreyImage("noname", surface, curModel.GetCoefs().GetRange(4, curModel.GetCoefs().Count - 4));
                    curModel.bestRoughtness = GreyImageList.descaleRa(NN.Predict(curPattern)[0]);
                    curModel.SetFitness(Math.Pow(curModel.bestRoughtness - GivenRoughtness, 2));
                    totalFitness += curModel.GetFitness();
                    curModel.SetFitness(totalFitness);
                }

                //if (counter != 0)
                //{
                //    prevError = genetic.population.get(0).GetCost();
                //}
                GA.NextGeneration();

                //if ((genetic.population.get(0).GetCost() - prevError) / genetic.population.get(0).GetCost() >= 0.0)
                //{
                //    changeCounter++;
                //}
                //else {
                //    changeCounter = 1;
                //}


                //if (counter % 1000 == 0)
                //{
                //    if (counter % 3000 == 0)
                //    {
                //        NutritionProblemModel.mutationMagnitude *= 2.0;
                //        //Genetic.TOURNAMENT_SIZE += 2;
                //    }
                //    else {
                //        NutritionProblemModel.mutationMagnitude /= 2;  //DEFUALT 2.0
                //    }
                //    if (counter % 10000 == 0)
                //    {
                //        Genetic.TOURNAMENT_SIZE += 5;
                //    }
                //    //GeneticModel.mutateChance -= 0.01;
                //}

                if (counter % 1 == 0)
                {
                    Console.WriteLine(counter + " | Current Fittest: " + GA.population[0].GetFitness() + " | NN Rougness: " + GA.population[0].bestRoughtness);
                }
                counter++;
            } while (counter < 1000);

            String output = "";

           Console.WriteLine(GA.population[0].GetCoefs()[0] + "; " + GA.population[0].GetCoefs()[1] + "; " + GA.population[0].GetCoefs()[2] + "; " + GA.population[0].GetCoefs()[3]);


            GA.population[0].DrawImage();
        }
    }
}
