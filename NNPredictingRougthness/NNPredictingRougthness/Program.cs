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
        static void Main(string[] args)
        {
            greyImageList = new GreyImageList(); 
            //surfaceList = new SurfaceList();
            NN = new NeuralNetwork();
            RunFourParameterTest();
            Console.ReadLine();
        }

        static void RunFourParameterTest()
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

                if(counter%1 == 0)
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
    }
}
