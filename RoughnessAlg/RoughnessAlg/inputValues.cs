using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RoughnessAlg
{
    class inputValues
    {
        public List<double[]> TrainingInp = new List<double[]>();
        public List<double[]> TrainingOut = new List<double[]>();

        public inputValues(string locationInp, string locationOut)
        {
            double[] tempInp;
            var readerInp = new StreamReader(File.OpenRead(locationInp)); //C:\Users\S.H.J. Thomas\Desktop\RoughnessAlg
            while (!readerInp.EndOfStream)
            {
                var lineInp = readerInp.ReadLine();
                var dataInp = lineInp.Split(',');

                tempInp =  new double[dataInp.Length];

                for (int i = 0; i < dataInp.Length; i++)
                {
                    string temp2;
                    temp2 = dataInp[i];

                    if (temp2 != "")
                    {
                        tempInp[i] = double.Parse(temp2);
                    }

                    else
                    {
                        tempInp[i] = 0;
                    }
                }
                TrainingInp.Add(tempInp); 
            }


            double[] tempOut;
            var readerOut = new StreamReader(File.OpenRead(locationOut)); //C:\Users\S.H.J. Thomas\Desktop\RoughnessAlg
            while (!readerOut.EndOfStream)
            {
                var lineOut = readerOut.ReadLine();
                var dataOut = lineOut.Split(',');

                tempOut = new double[dataOut.Length];

                for (int i = 0; i < dataOut.Length; i++)
                {
                    string temp3;
                    temp3 = dataOut[i];
                    if (temp3 != "")
                    {
                        tempOut[i] = double.Parse(temp3);
                    }

                    else
                    {
                        tempOut[i] = 0;
                    }
                }
                TrainingOut.Add(tempOut);
            }
        }
    }
}
