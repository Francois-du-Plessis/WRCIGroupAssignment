using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPredictingRougthness
{
    class Program
    {
        static SurfaceList surfaceList;
        static NeuralNetwork NN;
        static void Main(string[] args)
        {
            //GreyImageList gIL = new GreyImageList(); //Runs onceoff to populate .txt containing Surface Image information
            surfaceList = new SurfaceList();
            NN = new NeuralNetwork();
            RunFourParameterTest();
            Console.ReadLine();
        }

        static void RunFourParameterTest()
        {
            
            int counter = 0;
            while (counter < 500000)
            {
                counter++;

                double totalSSE = 0;
                double totalEvalSSE = 0;

                foreach (Surface surface in surfaceList.getOptiData())
                {
                    List<double> predictedRougthness = NN.Predict(surface);
                    List<double> actualRougthness = new List<double>();
                    actualRougthness.Add(surface.getScaledRa());
                    NN.TrainNeuron(0.1, actualRougthness);
                    totalSSE = Math.Pow(predictedRougthness[0] - surface.getScaledRa(), 2);
                }

                foreach (Surface surface in surfaceList.getEvalData())
                {
                    List<double> predictedRougthness = NN.Predict(surface);
                    List<double> actualRougthness = new List<double>();
                    actualRougthness.Add(surface.getScaledRa());
                    NN.TrainNeuron(0.1, actualRougthness);
                    totalEvalSSE = Math.Pow(predictedRougthness[0] - surface.getScaledRa(), 2);
                }

                if(counter%100 == 0)
                {
                    Console.WriteLine("{0} | TestSSE: {1} | EvalSSE: {2}",counter,totalSSE,totalEvalSSE);
                }
            }

            foreach (Surface surface in surfaceList.getOptiData())
            {
                List<double> predictedRougthness = NN.Predict(surface);
                Console.WriteLine("Actual RA: {0} | PredictedRA: {1} | Difference: {2}", surface.getRa(), SurfaceList.descaleRa(predictedRougthness[0]), surface.getRa()- SurfaceList.descaleRa(predictedRougthness[0]));
            }

            foreach (Surface surface in surfaceList.getEvalData())
            {
                List<double> predictedRougthness = NN.Predict(surface);
                Console.WriteLine("Actual RA: {0} | PredictedRA: {1} | Difference: {2}", surface.getRa(), SurfaceList.descaleRa(predictedRougthness[0]), surface.getRa() - SurfaceList.descaleRa(predictedRougthness[0]));

            }
        }
    }
}
