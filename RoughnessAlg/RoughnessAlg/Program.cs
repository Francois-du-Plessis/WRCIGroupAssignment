using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RoughnessAlg
{
    class Program
    {
        /*Programmers: Bianca Liebenberg and Stithian Thomas
         */
        static int genes = 4; //# of genes in chromosome
        static double[] parent1 = new double[genes]; //chromosomes of parent 1
        static double[] parent2 = new double[genes];
        static double[] child1 = new double[genes]; //chromosomes of children 1
        static double[] child2 = new double[genes];

        //static double[] tournament1 = new double[3]; //tournament to contain 3 individuals (SSEs)
        //static double[] tournament2 = new double[3];
        static double[] tournament1 = new double[] { 0, 0, 0 };
        static double[] tournament2 = new double[] { 0, 0, 0 };


        static double crossoverProb = 0.6; //crossover probability (0.6)
        static SimpleRNG uniformGen = new SimpleRNG(); //uniform generator class to be used for crossover
        static int maxGen = 2000; //maximum generations (1250)
        static int curGen = 0; //generation count
        static double mutationProb = 0; //mutation probability (rate) - relatively low?
        static double mutationMag; //mutation magnitude
        static double indvMutRate; //calculate mutation rate per generation (gets smaller as generations progress)
        static double totalSSE = 9999999999999999999;
       // static double totalSSE = totalRoughnessSSE(SSE);
        static double bestSSE = 300000000000000;


        static void Main(string[] args)
        {
            /* Genetic algorithm to predict surface roughnessRa) using the following inputs: Speed(V) , Feed(F), Depth(D) and Grey level(Ga)
             * 81 rows of data, population = 81
             */
            double[,] IOArray = getInputFromTxt(81, 4); //training set array

            double[,] population = randomPop(); //create population 81 rows and 4 columns

            Console.WriteLine("Total SSE vs Generations");
            FileStream outputSSE = new FileStream("outputSSE.txt", FileMode.Create, FileAccess.Write);
            FileStream outputPopulation = new FileStream("outputPopulation.txt", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outputSSE);
            StreamWriter writerPop = new StreamWriter(outputPopulation);

            //|| (totalSSE > bestSSE)
            while ((curGen < maxGen))
            {
                double[] calcRoughness = roughnessCalculation(IOArray, population); //calculated roughness
                double[] SSE = roughnessSSE(calcRoughness, IOArray);
                totalSSE = 0;
                totalSSE = totalRoughnessSSE(SSE);
                population = GAroughness(SSE, population, IOArray);// GENETIC ALGORITHM!!!!
                curGen++;

                //write SSE to file
                //Console.Write("Output SSE of {0} generation", curGen);
                writer.WriteLine(totalSSE);
                //string text = Console.ReadLine();
                //writer.WriteLine(text);

            }
            for (int i = 0; i < population.GetUpperBound(0); i++)
            {
                for (int y = 0; y < population.GetUpperBound(1); y++)
                {
                    writerPop.Write("{0} \t", population[i, y].ToString("F2"));
                }
                Console.WriteLine("\n");
            }
            writer.Close();
            outputSSE.Close();
            writerPop.Close();
            outputPopulation.Close();
            Console.WriteLine("Genetic Algorithm completed!");
            display2dArray(population);
            Console.WriteLine("");
            Console.WriteLine("curGen: {0}", curGen);
            display1dArray(tournament1);
            Console.WriteLine("");
            display1dArray(tournament2);
            Console.ReadLine();
        }

        static double[,] getInputFromTxt(int numOfItems, int numOfParaPerItem)
        {
            const char DELIM = ' ';
            const string FileName = "TrainingSet.txt";
            FileStream inFile = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            // Console.WriteLine("");

            recordIn = reader.ReadLine();
            int Counter = 0;

            double[,] IOdata = new double[numOfItems, numOfParaPerItem];

            while (recordIn != null)
            {

                fields = recordIn.Split(DELIM);


                for (int i = 0; i < numOfParaPerItem; i++)
                {
                    IOdata[Counter, i] = double.Parse(fields[i]);
                }
                recordIn = reader.ReadLine();

                Counter++;

            }
            reader.Close();
            inFile.Close();

            return IOdata;
        }

        public static double[,] randomPop()
        {
            double [,] pop = new double[82, genes+1];
            Random randNum = new Random(); //use random genrator class to randomly generated numbers

            for (int i = 0; i < pop.GetUpperBound(0); i++) //row upper bound
            {
                for (int x = 0; x < pop.GetUpperBound(1); x++) // column upper bound
                {
                    pop[i, x] = randNum.Next(-1, 2); //(-1,3)
                    //Console.Write(pop[i, x]);
                    //Console.Write(" ");
                }
                //Console.WriteLine();
            }
            return pop;
        }

        public static double[] roughnessCalculation(double[,] arrIO, double[,] arrPop) //calculated roughness calculation
        {
            double[] roughnessArr = new double[81];

            for (int i = 0; i < roughnessArr.Length; i++) //roughness array loop (81)
            {
                roughnessArr[i] = arrPop[i, 0] + (arrPop[i, 1] * arrIO[i, 0]) + (arrPop[i, 2] * arrIO[i, 1]) + (arrPop[i, 3] * arrIO[i, 2]);
                //roughnessArr[i] = arrPop[i, 0] + (arrPop[i, 1] * arrIO[i, 0]) + (arrPop[i, 2] * arrIO[i, 1]) + (arrPop[i, 3] * arrIO[i, 2]) + (arrPop[i, 4] * arrIO[i, 0] * arrIO[i, 1]) +
                  //  (arrPop[i, 5] * arrIO[i, 1] * arrIO[i, 2]) + (arrPop[i, 6] * arrIO[i, 0] * arrIO[i, 2]) + (arrPop[i, 7] * Math.Pow(arrIO[i, 0], 2)) + (arrPop[i, 8] * Math.Pow(arrIO[i, 1], 2)) +
                    //(arrPop[i, 9] * Math.Pow(arrIO[i, 2], 2));
            }

            return roughnessArr;
        }

        public static void display1dArray(double[] onedArr) //display 1D array
        {
            Console.WriteLine(" ");
            for (int i = 0; i < onedArr.Length; i++)
            {
                Console.WriteLine(onedArr[i]);
            }
        }

        public static void display2dArray(double[,] twodArr) //display 2D array
        {

            for (int i = 0; i < twodArr.GetUpperBound(0); i++) //row upper bound
            {
                for (int x = 0; x < twodArr.GetUpperBound(1); x++) // column upper bound
                {
                    Console.Write(twodArr[i, x]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public static double[] roughnessSSE(double[] arrCalcRoughness, double[,] arrIO) //roughness SSE
        {
            double[] arrSSE = new double[81]; //SSE = ((actual - calculated)^2)/2
            for (int i = 0; i < arrSSE.Length; i++)
            {
                arrSSE[i] = (Math.Pow((arrIO[i, 3] - arrCalcRoughness[i]), 2))/2;
            }

            return arrSSE;
        }

        public static double totalRoughnessSSE(double[] arrSSE) //sum roughness SSE
        {
            double sumSSE = 0;
            for (int i = 0; i < arrSSE.Length; i++)
            {
                sumSSE += arrSSE[i];
            }
            return sumSSE;
        }

        public static double[,] GAroughness(double[] arrSSE, double[,] arrPop, double[,] arrIO)
        {
            //double[,] newPop = new double[81, genes]; //new population to be returned
            //double[] mask = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //crossover mask for parent 1
            double[] mask = new double[] { 0, 0, 0, 0}; //crossover mask for parent 1

            //parent 1 tournament selection
            Random randNum1 = new Random(); //use random generator class to randomly generated numbers (tournament 1 array index)
            int[] newRND1 = new int[3]; //create tournament (parent 1)
            for (int i = 0; i < newRND1.Length; i++) //populate array with random index values between 0 and 82
            {
                newRND1[i] = randNum1.Next(0,82);
            }
            for (int a = 0; a < tournament1.Length; a++) //populate tornament 1 with SSE
            {
                for (int y = 0; y < arrSSE.Length; y++)
                {
                    if (y == newRND1[a])
                    {
                        tournament1[a] = arrSSE[newRND1[a]];
                    }
                }
                //if (tournament1[a] == newRND1[a])
                //{
                    //tournament1[a] = arrSSE[newRND1[a]];
                //}
            }

            //////
            double minSSEP1 = 9999999999999999999;
            int minSSE1index = 0;
            for (int i = 0; i < tournament1.Length; i++) //find best individual in tournament 1 (i.e.  parent)
            {
                if (tournament1[i] < minSSEP1)
                {
                    minSSEP1 = tournament1[i];
                }
            }
            for (int y = 0; y < arrSSE.Length; y++) //find index of best individual in tournament from population
            {
                if (arrSSE[y] == minSSEP1)
                {
                    minSSE1index = y;
                }
            }
            for (int p = 0; p < parent1.Length; p++) //choose fittest (1) parent from tournament 1
            {
                parent1[p] = arrPop[minSSE1index, p];
            }

            //parent 2 tournament selection
            // Random randNum2 = new Random(); //use random genrator class to randomly generated numbers (tournament 2 array index)
            int[] newRND2 = new int[3];
            for (int i = 0; i < newRND2.Length; i++)
            {
                newRND2[i] = randNum1.Next(0, 82);
            }
            for (int a = 0; a < tournament2.Length; a++) //populate tornament 2
            {
                for (int y = 0; y < arrSSE.Length; y++)
                {
                    if (y == newRND2[a])
                    {
                        tournament2[a] = arrSSE[newRND2[a]];
                    }
                }
                //if (tournament2[a] == newRND2[a])
               // {
                    //tournament2[a] = arrSSE[newRND2[a]];
               // }
            }

            /////
            double minSSEP2 = 9999999999999999999;
            int minSSE2index = 0;
            for (int i = 0; i < tournament2.Length; i++) //find best individual in tournament 2 (i.e.  parent)
            {
                if (tournament2[i] < minSSEP2)
                {
                    minSSEP2 = tournament2[i];
                }
            }
            for (int y = 0; y < arrSSE.Length; y++) //find index of best individual in tournament from population
            {
                if (arrSSE[y] == minSSEP2)
                {
                    minSSE2index = y;
                }
            }
            for (int p = 0; p < parent2.Length; p++) //choose fittest (2) parent from tournament 2
            {
                parent2[p] = arrPop[minSSE2index, p];
            }
            
            //crossover- MASK!!
            for (int p1 = 0; p1 < parent1.Length; p1++)
            {
                if (uniformGen.GetUniform() < crossoverProb)
                {
                    mask[p1] = 1;
                }
            }
            Random rand1 = new Random(); //randomise (shuffle) mask 1 contents
            for (int r = 0; r < mask.Length - 1; r++)
            {
                double temp = mask[r];
                int e = rand1.Next(r, mask.Length);
                mask[r] = mask[e];
                mask[e] = temp;
            }

            //find best individual
            // Random randNum2 = new Random(); //use random genrator class to randomly generated numbers (tournament 2 array index)
            double minSSEBest = 9999999999999999999;
            int minSSEBestIndex = 0;
            for (int i = 0; i < arrSSE.Length; i++) //find best individual in population
            {
                if (arrSSE[i] < minSSEBest)
                {
                    minSSEBest = arrSSE[i];
                }
            }
            for (int y = 0; y < arrSSE.Length; y++) //find index of best individual in population
            {
                if (arrSSE[y] == minSSEBest)
                {
                    minSSEBestIndex = y;
                }
            }
            double[] bestChromo = new double[genes];
            for (int c = 0; c < arrPop.GetUpperBound(1); c++) //loop through columns of population
            {
                //for (int c = 0; c < arrPop.GetUpperBound(1); c++)//loop through columns of population
                //{
                    //if (i == minSSEBestIndex)
                    //{
                        bestChromo[c] = arrPop[minSSEBestIndex, c];//populate temporary best chromosome array
                    //}
                //}
            }

            //uniform crossover - produce offspring (children)
            // 1--> parent 1 (child 1); 1--> parent 2 (child 2)
            // 0--> parent 2 (child 1); 0--> parent 2 (child 2)
            for (int m = 0; m < mask.Length; m++)
            {
                if (mask[m] == 1)
                {
                    child1[m] = parent1[m];
                    //child2[m] = parent2[m];
                }
                else
                {
                    child1[m] = parent2[m];
                    //child2[m] = parent1[m];
                }
                //Console.WriteLine("{0} \t {1} \t {2} \t {3}", parent1[m], parent2[m], child1[m], child2[m]); //TEMP display parents and children
                //Console.WriteLine("{0} \t {1} \t {2}", parent1[m], parent2[m], child1[m]); //TEMP display parents and children
                //Console.WriteLine("");
            }
           // Console.ReadLine();

            //find worst individual(s) in population
            double worstSSE = -1; //very small value (corresponds to max SSE population)
            int worstSSEindex = 0;
            for (int i = 0; i < arrSSE.Length; i++) //find worst individual in population
            {
                if (arrSSE[i] > worstSSE)
                {
                    worstSSE = arrSSE[i];
                }
            }
            for (int y = 0; y < arrSSE.Length; y++) //find index of worst individual in population
            {
                if (arrSSE[y] == worstSSE)
                {
                    worstSSEindex = y;
                }
            }
            for (int g = 0; g < child1.Length; g++)//Replace worst individual with child!
            {
                arrPop[worstSSEindex, g] = child1[g];
            }
            //display2dArray(arrPop); //temp display
            //Console.WriteLine("");

            //Gaussian mutation (mutate ALL genes in population)
            double x = uniformGen.GetUniform(); //x value for gaussian distribution formula
            //mutationProb = -(1 / maxGen) * curGen + 0.5; //decay function for mutation probability (rate)
            mutationProb = (1 / 240) + (0.11375 / Math.Pow(2, curGen));
            indvMutRate = 1 - Math.Pow((1 - mutationProb), genes); //10 GENES IN CHROMOSOME! HARD CODED!!
            //if (mutationProb < 0.002)
            //{
            //    mutationProb = 0.002;
            //}
            mutationMag = (1 / Math.Sqrt(2 * Math.PI) * (Math.Exp(-(Math.Pow(x,2))/2))); //include Gaussian noise

            for (int r = 0; r < arrPop.GetUpperBound(0); r++) //rows of population
            {
                for (int c = 0; c < arrPop.GetUpperBound(1); c++) //columns of population
                {
                    if (uniformGen.GetUniform() <= indvMutRate)
                    {
                        arrPop[r, c] = arrPop[r, c] + mutationMag; //mutate gene!
                    }
                }
            }
            //display2dArray(arrPop); //temp display

            //insert best individual from previous (unmutated) individual into current population
            for (int r = 0; r < arrPop.GetUpperBound(0); r++)
            {
                for (int c = 0; c < arrPop.GetUpperBound(1); c++)
                {
                    if (r == minSSEBestIndex)
                    {
                        arrPop[r, c] = bestChromo[c];
                    }
                }
            }

            //update population
            return arrPop;
        }
    }
}
