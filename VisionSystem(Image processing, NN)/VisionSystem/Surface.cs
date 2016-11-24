using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VisionSystem
{
    class Surface
    {
        private double speed;
        private double feed;
        private double depth;
        private double Ga;
        private double Ra;

        public Surface(double spd, double fd, double dpt, double ga, double ra)
        {
            speed = spd / 1000;
            feed = fd;
            depth = dpt / 5;
            Ga = ga / 50;
            Ra = ra;
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

        public void Display(StreamWriter SW)
        {
            SW.Write("{0}  {1}  {2}  {3}  |  {4}", speed, feed, depth, Ga, Ra);
        }

    }
}
