using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace VisionSystem
{
    class SurfaceList
    {
        private ArrayList optiData; 
        private ArrayList evalData;

        public SurfaceList()
        {
            optiData = new ArrayList();
            evalData = new ArrayList();
            ReadData("RoughnessData.txt", "Evaluation.txt");
        }

        public void ReadData(string fileName1, string fileName2) //Reading optimisation and evaluation Data from a file
        {
            StreamReader SR1 = new StreamReader(fileName1);
            string[] dataTray;
            while (!SR1.EndOfStream)
            {
                dataTray = SR1.ReadLine().Split(',');
                double spd = Double.Parse(dataTray[0]);
                double fd = Double.Parse(dataTray[1]);
                double dpt = Double.Parse(dataTray[2].Replace('.', ','));
                double ga = Double.Parse(dataTray[3]);
                double ra = Double.Parse(dataTray[4].Replace('.', ','));
                Surface temp1 = new Surface(spd, fd, dpt, ga, ra);
                optiData.Add(temp1);
            }
            SR1.Close();

            StreamReader SR2 = new StreamReader(fileName2);
            while (!SR2.EndOfStream)
            {
                dataTray = SR2.ReadLine().Split(',');
                double spd = Double.Parse(dataTray[0]);
                double fd = Double.Parse(dataTray[1]);
                double dpt = Double.Parse(dataTray[2].Replace('.', ','));
                double ga = Double.Parse(dataTray[3]);
                double ra = Double.Parse(dataTray[4].Replace('.', ','));
                Surface temp2 = new Surface(spd, fd, dpt, ga, ra);
                evalData.Add(temp2);
            }
            SR2.Close();

        }

        public ArrayList getOptiData()
        {
            return optiData;
        }

        public ArrayList getEvalData()
        {
            return evalData;
        }
    }
}
