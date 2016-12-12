using System;
using System.Collections.Generic;
using System.IO;
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
            //Globalization code: Forces the program to use fullstops in doubles instead of commas.
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            //End of globalization code

            List<double[]> TrainingParameters = new List<double[]>();

            TrainingParameters.Add(new double[] { 5, 0.1, 0.0001 });//original: hidden neurons, learning rate, momentum
            //TrainingParameters.Add(new double[] { 5, 0.1, 0.001 });
            //TrainingParameters.Add(new double[] { 5, 0.1, 0.01 });
            //TrainingParameters.Add(new double[] { 5, 0.1, 0.00001 });
            //TrainingParameters.Add(new double[] { 5, 0.1, 0.000001 });

            //TrainingParameters.Add(new double[] { 5, 0.01, 0.0001 });
            //TrainingParameters.Add(new double[] { 5, 0.01, 0.001 });
            //TrainingParameters.Add(new double[] { 5, 0.01, 0.01 });
            //TrainingParameters.Add(new double[] { 5, 0.01, 0.00001 });
            //TrainingParameters.Add(new double[] { 5, 0.01, 0.000001 });

            //TrainingParameters.Add(new double[] { 5, 0.001, 0.0001 });
            //TrainingParameters.Add(new double[] { 5, 0.001, 0.001 });
            //TrainingParameters.Add(new double[] { 5, 0.001, 0.01 });
            //TrainingParameters.Add(new double[] { 5, 0.001, 0.00001 });
            //TrainingParameters.Add(new double[] { 5, 0.001, 0.000001 });

            //TrainingParameters.Add(new double[] { 5, 0.0001, 0.0001 });
            //TrainingParameters.Add(new double[] { 5, 0.0001, 0.001 });
            //TrainingParameters.Add(new double[] { 5, 0.0001, 0.01 });
            //TrainingParameters.Add(new double[] { 5, 0.0001, 0.00001 });
            //TrainingParameters.Add(new double[] { 5, 0.0001, 0.000001 });

            //TrainingParameters.Add(new double[] { 3, 0.1, 0.0001 });
            //TrainingParameters.Add(new double[] { 3, 0.1, 0.001 });
            //TrainingParameters.Add(new double[] { 3, 0.1, 0.01 });
            //TrainingParameters.Add(new double[] { 3, 0.1, 0.00001 });
            //TrainingParameters.Add(new double[] { 3, 0.1, 0.000001 });

            //TrainingParameters.Add(new double[] { 10, 0.01, 0.0001 });
            //TrainingParameters.Add(new double[] { 10, 0.01, 0.001 });
            //TrainingParameters.Add(new double[] { 10, 0.01, 0.01 });
            //TrainingParameters.Add(new double[] { 10, 0.01, 0.00001 });
            //TrainingParameters.Add(new double[] { 10, 0.01, 0.000001 });

            //TrainingParameters.Add(new double[] { 15, 0.001, 0.0001 });
            //TrainingParameters.Add(new double[] { 15, 0.001, 0.001 });
            //TrainingParameters.Add(new double[] { 15, 0.001, 0.01 });
            //TrainingParameters.Add(new double[] { 15, 0.001, 0.00001 });
            //TrainingParameters.Add(new double[] { 15, 0.001, 0.000001 });

            //TrainingParameters.Add(new double[] { 10, 0.1, 0.0001 });
            //TrainingParameters.Add(new double[] { 10, 0.1, 0.001 });
            //TrainingParameters.Add(new double[] { 10, 0.1, 0.01 });
            //TrainingParameters.Add(new double[] { 10, 0.1, 0.00001 });
            //TrainingParameters.Add(new double[] { 10, 0.1, 0.000001 });

            //TrainingParameters.Add(new double[] { 15, 0.1, 0.0001 });
            //TrainingParameters.Add(new double[] { 15, 0.1, 0.001 });
            //TrainingParameters.Add(new double[] { 15, 0.1, 0.01 });
            //TrainingParameters.Add(new double[] { 15, 0.1, 0.00001 });
            //TrainingParameters.Add(new double[] { 15, 0.1, 0.000001 });

            //TrainingParameters.Add(new double[] { 15, 0.1, 0.0001 }); //hypothesized best setting, based on graphs

            foreach (double[] parameters in TrainingParameters)
            {
                Console.WriteLine("\nTraining Session (Hidden Neurons {0}, Learning Rate {1}, Momentum {2}):\n", parameters[0], parameters[1], parameters[2]);
                greyImageList = new GreyImageList(); //Runs onceoff to populate .txt containing Surface Image information
                NN = new NeuralNetwork((int)parameters[0],parameters[2]);
                RunFourParameterTest(parameters[1]);
            }

            //greyImageList = new GreyImageList(); 
            ////surfaceList = new SurfaceList();
            //NN = new NeuralNetwork();
            //RunFourParameterTest();
            Console.ReadLine();
        }

        static void RunFourParameterTest(double learningRate = 0.1)
        {
            int counter = 0;
            while (counter < 800)
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
                    NN.TrainNeuron(learningRate, actualRougthness);
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
                    StreamWriter SW1 = new StreamWriter("outputData TestSSE H" + NN.getHiddenLayerSize() + " L" + learningRate + " M" + NN.getMomentumConstant() + ".txt", true);
                    StreamWriter SW2 = new StreamWriter("outputData EvalSSE H" + NN.getHiddenLayerSize() + " L" + learningRate + " M" + NN.getMomentumConstant() + ".txt", true);
                    SW1.WriteLine(totalSSE);
                    SW2.WriteLine(totalEvalSSE);
                    SW1.Close();
                    SW2.Close();

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
