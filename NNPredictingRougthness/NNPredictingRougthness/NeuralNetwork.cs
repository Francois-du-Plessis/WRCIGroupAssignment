using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNPredictingRougthness
{
    class NeuralNetwork
    {
        private readonly int J = 5; //Number of neurons in hidden layer
        private readonly int I = 4 + 10000; //Number of inputs
        private readonly int K = 1; //Number of neurons in output layer

        private List<Double> InputN;
        private List<Double> HiddenN;
        private List<Double> OutputN;

        private double momentumConstant = 0.0001;

        private double[,] LearningConstantsV;
        private double[,] LearningConstatnsW;

        private double[,] V;
        private double[,] W;

        private double[,] dV;
        private double[,] dW;

        private double[,] prevdV;
        private double[,] prevdW;

        private Random random = new Random();

        public NeuralNetwork()
        {
            V = new double[J,I + 1];
            W = new double[K,J + 1];

            dV = new double[J,I + 1];
            dW = new double[K,J + 1];

            prevdV = new double[J,I + 1];
            prevdW = new double[K,J + 1];

            LearningConstantsV = new double[J,I + 1];
            LearningConstatnsW = new double[K,J + 1];

            InputN = new List<Double>();
            HiddenN = new List<Double>();
            OutputN = new List<Double>();

            initializeWeights();
            initializeNeurons();
        }

        private void initializeWeights()
        {
            for (int j = 0; j < J; j++)
            {
                for (int i = 0; i < I + 1; i++)
                {
                    V[j,i] = calcRandomWeight();
                    dV[j,i] = 0;
                    prevdV[j,i] = 0;
                    LearningConstantsV[j,i] = 0.01;
                }
            }

            for (int k = 0; k < K; k++)
            {
                for (int j = 0; j < J + 1; j++)
                {
                    W[k,j] = calcRandomWeight();
                    dW[k,j] = 0;
                    prevdW[k,j] = 0;
                    LearningConstatnsW[k,j] = 0.01;
                }
            }
        }

        public void SetWeights(double[,] newInputWeights, double[,] newOutputWeights)
        {
            V = newInputWeights;
            W = newOutputWeights;
        }

        private void initializeNeurons()
        {
            for (int j = 0; j < J; j++)
            {
                HiddenN.Add(0.0);
            }

            for (int k = 0; k < K; k++)
            {
                OutputN.Add(0.0);
            }
        }

        //    private double calcRandomWeight(){
        //        return (random.nextDouble()*2.0 - 1.0);
        //    }

        private double calcRandomWeight()
        {
            return (random.NextDouble() * (2.0 / Math.Sqrt(I + 1)) - (1.0 / Math.Sqrt(I + 1)));
        }

        public List<Double> Predict(GreyImage image)
        {
            InputN = new List<Double>();

            InputN.AddRange(image.surface.GetScaleInputs());
            InputN.AddRange(image.scaledPixelArray);

            CalculateHiddenLayer();
            CalculateOutputLayer();

            return OutputN;
        }

        private void CalculateHiddenLayer()
        {
            for (int j = 0; j < J; j++)
            {
                double total = 0;
                for (int i = 0; i < I; i++)
                {
                    total += V[j,i] * InputN[i];
                }
                total += V[j,I] * (-1); // hidden bias
                HiddenN[j] = HiddenActivationFunction(total);
            }
        }

        private void CalculateOutputLayer()
        {
            for (int k = 0; k < K; k++)
            {
                double total = 0;
                for (int j = 0; j < J; j++)
                {
                    total += W[k,j] * HiddenN[j];
                }
                total += W[k,J] * (-1); // output bias
                OutputN[k] =  OutputActivationFunction(total);
            }
        }

        private double HiddenActivationFunction(double net)
        {
            return SigmoidFunction(net);
        }

        private double OutputActivationFunction(double net)
        {
            return SigmoidFunction(net);
        }

        private double SigmoidFunction(double net)
        {
            return 1 / (1 + Math.Exp(-1 * net));
        }

        private double LinearFunction(double net)
        {
            return net;
        }

        public void TrainNeuron(double learningRate, List<Double> actualValues)
        {

            for (int j = 0; j < J; j++)
            {
                double Yj = HiddenN[j];
                for (int i = 0; i < I; i++)
                {
                    for (int k = 0; k < K; k++)
                    {
                        dV[j,i] += LearningConstantsV[j,i] * (actualValues[k] - OutputN[k]) * OutputN[k] * (1 - OutputN[k]) * W[k,j] * Yj * (1 - Yj) * InputN[i];
                    }
                }

                for (int k = 0; k < K; k++)
                {
                    dV[j,I] += LearningConstantsV[j,I] * (actualValues[k] - OutputN[k]) * OutputN[k] * (1 - OutputN[k]) * W[k,j] * Yj * (1 - Yj) * (-1.0); //output bias
                }
            }

            for (int k = 0; k < K; k++)
            {
                for (int j = 0; j < J; j++)
                {
                    dW[k,j] += LearningConstatnsW[k,j] * (actualValues[k] - OutputN[k]) * OutputN[k] * (1 - OutputN[k]) * HiddenN[j];
                }
                dW[k,J] += LearningConstatnsW[k,J] * (actualValues[k] - OutputN[k]) * OutputN[k] * (1 - OutputN[k]) * (-1.0);  //Hidden bias
            }

            LearnNeuron();
        }

        public void LearnNeuron()
        {
            for (int j = 0; j < J; j++)
            {
                for (int i = 0; i < I + 1; i++)
                {
                    V[j,i] += (dV[j,i] + momentumConstant * prevdV[j,i]);
                    if (dV[j,i] * prevdV[j,i] == Math.Abs(dV[j,i] * prevdV[j,i]))
                    {
                        LearningConstantsV[j,i] += 0.00001;
                    }
                    else {
                        LearningConstantsV[j,i] -= 0.00001;
                    }
                    prevdV[j,i] = dV[j,i];
                    dV[j,i] = 0;
                }
            }

            for (int k = 0; k < K; k++)
            {
                for (int j = 0; j < J + 1; j++)
                {
                    W[k,j] += (dW[k,j] + momentumConstant * prevdW[k,j]);
                    if (dW[k,j] * prevdW[k,j] == Math.Abs(dW[k,j] * prevdW[k,j]))
                    {
                        LearningConstatnsW[k,j] += 0.000001;
                    }
                    else {
                        LearningConstatnsW[k,j] -= 0.000001;
                    }
                    prevdW[k,j] = dW[k,j];
                    dW[k,j] = 0;
                }
            }
        }
    }
}
