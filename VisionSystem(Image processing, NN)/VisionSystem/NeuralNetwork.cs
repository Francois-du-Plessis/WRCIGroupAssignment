using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace VisionSystem
{
    class NeuralNetwork: Optimisation
    {
        private int bias = -1;
        private double learnRate;
        private double[,] weights;
        private double empError;
        private double genError;

        public NeuralNetwork(SurfaceList List, double rate, double iniWeight) : base(List)
        {
            learnRate = rate;
            weights = new double[5, 2];
            for (int y = 0; y < weights.GetLength(1); y++)
            {
                for (int x = 0; x < weights.GetLength(0); x++)
                {
                    weights[x, y] = iniWeight;
                }
            }
            empError = 0;
            genError = 0;
        }

        protected override void OptiDataTray(Surface temp)
        {
            base.OptiDataTray(temp);
            inputs[4] = bias;
        }

        public double Predict(Surface temp, int yIndex) // Prediction Method
        {
            OptiDataTray(temp);//Put Surface attributes into an array
            double net = 0;
            for (int x = 0; x < weights.GetLength(0); x++)//Compute output of summation unit
            {
                net = net + inputs[x] * weights[x, yIndex];
            }
            return net;//Return output
        }


        private void Train()
        {
            for (int x = 0; x < optiData.Count; x++)
            {
                double net = 0;
                double activFunc = 0;
                int yIndex = 0;
                Surface temp = (Surface)optiData[x]; // Temporary surface object
                if (temp.getSpeed() > 0.500)
                {
                    yIndex = 1;
                }
                net = Predict(temp, yIndex); // Compute output of a summation unit
                activFunc = ActivFunc(net); // Compute the output of a sigmoid func
                UpdateWeights(net, activFunc, yIndex); // Update the respective input weights
            }
        }

        public void Train(int i)
        {
            for (int x = 0; x < i; x++)
            {
                Train();
            }
            empError = TestSSE();
            genError = TrainSSE();
        }
        private double ActivFunc(double net)
        {
            double activFunc = 0;
            double fp = 0;
            fp = Math.Pow((1 + Math.Exp(-net / target)), -1);// sigmoid activation function
            activFunc = fp * (1 - fp);
            return activFunc;
        }

        private void UpdateWeights(double net, double activFunc, int yIndex)
        {
            for (int x = 0; x < weights.GetLength(0); x++)//Updating weights for each input
            {
                weights[x, yIndex] = weights[x, yIndex] + 2 * (learnRate) * (target - net) * activFunc * inputs[x];
                //wi(t) = wi(t-1) + delta wi(t), where delta wi(t) = 2n(tp-fp)fp(1-fp)xip
            }
        }

        private double SSE(ArrayList tempList)
        {
            double SSE = 0;
            int yIndex = 0;
            for (int x = 0; x < tempList.Count; x++)
            {
                Surface roughSurf = (Surface)tempList[x];
                if (roughSurf.getSpeed() > 0.500)
                {
                    yIndex = 1;
                }
                SSE = SSE + (roughSurf.getRa() - Predict(roughSurf, yIndex)) *
                (roughSurf.getRa() - Predict(roughSurf, yIndex));
            }
            return SSE / tempList.Count;
        }
        private double TestSSE() // Returns the Sum Squared Error of the Test Set
        {
            return SSE(evalData);
        }

        private double TrainSSE() // Returns the Sum Squared Error of the Train Set
        {
            return SSE(optiData);
        }

        private void dispSurf(ArrayList List, StreamWriter SW)//Displays predicted roughness
        {
            int yIndex = 0;
            for (int x = 0; x < List.Count; x++)
            {
                Surface roughSurf = (Surface)List[x];
                if (roughSurf.getSpeed() > 0.500)
                {
                    yIndex = 1;
                }
                roughSurf.Display(SW);
                SW.WriteLine(" -> {0}", Math.Round((Predict(roughSurf, yIndex)), 2));

            }
        }

        private void DispTestSurf(StreamWriter SW)//Displays Evaluation Set predicted Roughness
        {
            SW.WriteLine("Predicted Roughness (Ra)- Evaluation Set:\n");
            dispSurf(evalData, SW);
            SW.WriteLine("Evaluation Set SSE: {0}", empError);
            SW.WriteLine("Train Set SSE: {0}\n", genError);
        }

        public void displayParams(StreamWriter SW)
        {
            SW.WriteLine("\nWeights:");
            for (int y = 0; y < weights.GetLength(1); y++)
            {
                for (int x = 0; x < weights.GetLength(0); x++)
                {
                    SW.Write("{0}   ", weights[x, y]);
                }
                SW.WriteLine();
            }

            SW.WriteLine("\n\nLearning Rate:");
            SW.WriteLine(learnRate);
            SW.WriteLine("\nESSE:");
            SW.WriteLine(empError);
            SW.WriteLine("\nGSSE:");
            SW.WriteLine(genError);
            SW.WriteLine();
        }

        public void Display(StreamWriter SW)
        {
            DispTestSurf(SW);
        }
    }
}
