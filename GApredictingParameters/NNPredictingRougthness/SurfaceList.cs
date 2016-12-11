using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace NNPredictingRougthness
{
    class SurfaceList
    {
        private List<Surface> optiData; 
        private List<Surface> evalData;

        static double maxSpeed = double.MinValue;
        static double maxFeed = double.MinValue;
        static double maxDepth = double.MinValue;
        static double maxGa = double.MinValue;
        static  double maxRa = double.MinValue;

        static double minSpeed = double.MaxValue;
        static double minFeed = double.MaxValue;
        static double minDepth = double.MaxValue;
        static double minGa = double.MaxValue;
        static double minRa = double.MaxValue;

        public SurfaceList()
        {
            optiData = new List<Surface>();
            evalData = new List<Surface>();
            ReadData("RoughnessData.txt", "Evaluation.txt");
            getMinMaxValues();
        }

        private void getMinMaxValues()
        {
            foreach(Surface curSurface in optiData)
            {
                if(curSurface.getSpeed() > maxSpeed)
                {
                    maxSpeed = curSurface.getSpeed();
                }

                if (curSurface.getSpeed() < minSpeed)
                {
                    minSpeed = curSurface.getSpeed();
                }

                if (curSurface.getFeed() > maxFeed)
                {
                    maxFeed = curSurface.getFeed();
                }

                if (curSurface.getFeed() < minFeed)
                {
                    minFeed = curSurface.getFeed();
                }

                if (curSurface.getDepth() > maxDepth)
                {
                    maxDepth = curSurface.getDepth();
                }

                if (curSurface.getDepth() < minDepth)
                {
                    minDepth = curSurface.getDepth();
                }

                if (curSurface.getGa() > maxGa)
                {
                    maxGa = curSurface.getGa();
                }

                if (curSurface.getGa() < minGa)
                {
                    minGa = curSurface.getGa();
                }

                if (curSurface.getRa() > maxRa)
                {
                    maxRa = curSurface.getRa();
                }

                if (curSurface.getRa() < minRa)
                {
                    minRa = curSurface.getRa();
                }
            }
        }

        public void ReadData(string fileName1, string fileName2) //Reading optimisation and evaluation Data from a file
        {
            StreamReader SR1 = new StreamReader(fileName1);
            string[] dataTray;
            SR1.ReadLine(); //Skip Title Row 
            while (!SR1.EndOfStream)
            {
                dataTray = SR1.ReadLine().Split(',');
                double spd = Double.Parse(dataTray[0]);
                double fd = Double.Parse(dataTray[1]);
                double dpt = Double.Parse(dataTray[2]);
                double ga = Double.Parse(dataTray[3]);
                double ra = Double.Parse(dataTray[4]);
                Surface temp1 = new Surface(spd, fd, dpt, ga, ra);
                optiData.Add(temp1);
            }
            SR1.Close();

            StreamReader SR2 = new StreamReader(fileName2);
            SR2.ReadLine(); //Skip Title Row 
            while (!SR2.EndOfStream)
            {
                dataTray = SR2.ReadLine().Split(',');
                double spd = Double.Parse(dataTray[0]);
                double fd = Double.Parse(dataTray[1]);
                double dpt = Double.Parse(dataTray[2]);
                double ga = Double.Parse(dataTray[3]);
                double ra = Double.Parse(dataTray[4]);
                Surface temp2 = new Surface(spd, fd, dpt, ga, ra);
                evalData.Add(temp2);
            }
            SR2.Close();

        }

        public List<Surface> getOptiData()
        {
            return optiData;
        }

        public List<Surface> getEvalData()
        {
            return evalData;
        }

        public static double scaleValue(double actualValue, double actualMin, double actualMax, double scaleMin, double scaleMax)
        {
            return (actualValue - actualMin) / (actualMax - actualMin) * (scaleMax - scaleMin) + scaleMin;
        }

        public static double descaleValue(double scaleValue, double actualMin, double actualMax, double scaleMin, double scaleMax)
        {
            return ((scaleValue - scaleMin) * (actualMax - actualMin)) / (scaleMax - scaleMin) + actualMin;
        }

        public static double scaleSpeed(double speed)
        {
            return scaleValue(speed, minSpeed, maxSpeed, -0.5, 0.5);
        }

        public static double scaleFeed(double feed)
        {
            return scaleValue(feed, minFeed, maxFeed, -0.5, 0.5);
        }

        public static double scaleDepth(double depth)
        {
            return scaleValue(depth, minDepth, maxDepth, -0.5, 0.5);
        }

        public static double scaleGa(double Ga)
        {
            return scaleValue(Ga, minGa, maxGa, -0.5, 0.5);
        }

        public static double scaleRa(double Ra)
        {
            return scaleValue(Ra, minRa, maxRa, 0, 1);
        }

        public static double descaleRa(double scaleRa)
        {
            return descaleValue(scaleRa, minRa, maxRa, 0, 1);
        }

    }
}
