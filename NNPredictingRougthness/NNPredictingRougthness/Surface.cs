using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NNPredictingRougthness
{
    class Surface
    {
        private double speed;
        private double feed;
        private double depth;
        private double Ga;
        private double Ra;
        public List<Double> inputList;

        public Surface(double spd, double fd, double dpt, double ga, double ra)
        {
            speed = spd;
            feed = fd;
            depth = dpt;
            Ga = ga;
            Ra = ra;
            inputList = GetInputs();
        }

        private List<Double> GetInputs()
        {
            List<Double> inputList = new List<double>();
            inputList.Add(speed);
            inputList.Add(feed);
            inputList.Add(depth);
            inputList.Add(Ga);
            return inputList;
        }

        public List<Double> GetScaleInputs()
        {
            List<Double> inputList = new List<double>();
            inputList.Add(GreyImageList.scaleSpeed(speed));
            inputList.Add(GreyImageList.scaleFeed(feed));
            inputList.Add(GreyImageList.scaleDepth(depth));
            inputList.Add(GreyImageList.scaleGa(Ga));
            return inputList;
        }

        public double getSpeed()
        {
            return speed;
        }

        public double getFeed()
        {
            return feed;
        }

        public double getDepth()
        {
            return depth;
        }

        public double getGa()
        {
            return Ga;
        }

        public double getRa()
        {
            return Ra;
        }

        public double getScaledRa()
        {
            return SurfaceList.scaleRa(Ra);
        }

        public void Display(StreamWriter SW)
        {
            SW.Write("{0}  {1}  {2}  {3}  |  {4}", speed, feed, depth, Ga, Ra);
        }

    }
}
