using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace VisionSystem
{
    class Optimisation
    {
        protected double[] inputs; //Optimisation Inputs
        protected ArrayList optiData; //Optimisation Data
        protected ArrayList evalData;
        protected double target; //Optimisation Target

        public Optimisation(SurfaceList List) //Loading Optimisation Data
        {
            optiData = List.getOptiData();
            evalData = List.getEvalData();
            inputs = new double[5];
        }

        protected virtual void OptiDataTray(Surface temp) //Optimisation Inputs temporary tray
        {
            inputs[0] = temp.getSpeed();
            inputs[1] = temp.getFeed();
            inputs[2] = temp.getDepth();
            inputs[3] = temp.getGa();
            target = temp.getRa();
        }

    }
}
