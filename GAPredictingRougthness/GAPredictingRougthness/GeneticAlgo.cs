using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPredictingRougthness
{
    class GeneticAlgo
    {
        public List<RougthnessChromosone> population;

        public int populationSize = 100; //Defualt 100
        public static int TOURNAMENT_SIZE = 10; //Defualt 5

        public static Random random = new Random();

        public GeneticAlgo()
        {
            population = new List<RougthnessChromosone>();

            for (int x = 0; x < populationSize; x++)
            {
                population.Add(new RougthnessChromosone());
            }
        }


        public void NextGeneration()
        {
            population.Sort(delegate(RougthnessChromosone o1, RougthnessChromosone o2) 
            {
                if (o1.GetFitness() < o2.GetFitness())
                {
                    return -1;
                }
                else if (o1.GetFitness() > o2.GetFitness())
                {
                    return 1;
                }
                return 0;
            });

            List<RougthnessChromosone> newPopulation = new List<RougthnessChromosone>();
            newPopulation.Add(population[0]);

            while (newPopulation.Count < populationSize)
            {
                //newPopulation.add(TournamentSelection());
                newPopulation.Add(TournamentSelection2());
            }

            population = newPopulation;
        }

        public RougthnessChromosone TournamentSelection2()
        {
            List<RougthnessChromosone> tournament = new List<RougthnessChromosone>();

            for (int x = 0; x < TOURNAMENT_SIZE; x++)
            {
                tournament.Add(population[random.Next(populationSize)]);
            }

            tournament.Sort(delegate(RougthnessChromosone o1, RougthnessChromosone o2) 
            {
                if (o1.GetFitness() < o2.GetFitness())
                {
                    return -1;
                }
                else if (o1.GetFitness() > o2.GetFitness())
                {
                    return 1;
                }
                return 0;
            });

            RougthnessChromosone parent1 = tournament[0];
            tournament = new List<RougthnessChromosone>();

            for (int x = 0; x < TOURNAMENT_SIZE; x++)
            {
                tournament.Add(population[random.Next(populationSize)]);
            }

            tournament.Sort(delegate(RougthnessChromosone o1, RougthnessChromosone o2)
            {
                if (o1.GetFitness() < o2.GetFitness())
                {
                    return -1;
                }
                else if (o1.GetFitness() > o2.GetFitness())
                {
                    return 1;
                }
                return 0;
            });

            RougthnessChromosone parent2 = tournament[0];

            return new RougthnessChromosone((RougthnessChromosone)parent1, (RougthnessChromosone)parent2);
        }
    }
}
