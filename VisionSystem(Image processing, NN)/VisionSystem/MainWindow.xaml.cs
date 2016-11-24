using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;
namespace VisionSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Image surfImage;
        private GreyImageList List;
        //private ArrayList meanArray3, dataArray3;
        public MainWindow()
        {
            InitializeComponent();
            //surfImage = new Image();           
            //imageFrame.Content = surfImage;
            //meanArray3 = new ArrayList();
            //dataArray3 = new ArrayList();
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void Train_Click(object sender, RoutedEventArgs e)
        {
            //List = new VisionSystem.GreyImageList(surfImage);
            //ComputeMean("Data3.txt", meanArray3, dataArray3);
            //WriteMean("Mean3.txt", "DataCopy3.txt", meanArray3, dataArray3);
            StreamWriter SW = new StreamWriter("Display.txt");
            SurfaceList List = new SurfaceList();
            NeuralNetwork NN = new NeuralNetwork(List, 2.5e-3, -1e-1);
            NN.displayParams(SW);
            NN.Train(300);
            NN.Display(SW);
            NN.displayParams(SW);
            SW.Close();
            checkBox.IsChecked = true;
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Compute_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ComputeMean(string fileName, ArrayList meanList, ArrayList dataList)
        {
            StreamReader SR = new StreamReader(fileName);
            string[] tray = null;
            double[] roughData = null;
            double sum;
            int dim;
            while (!SR.EndOfStream)
            {
                tray = SR.ReadLine().Split(',');
                dim = tray.Length;
                roughData = new double[dim];
                sum = 0;
                for (int x = 0; x < dim; x++)
                {
                    roughData[x] = Double.Parse(tray[x].Replace('.', ','));
                    sum = sum + roughData[x];
                }
                meanList.Add(Math.Round((sum / dim), 2));
                dataList.Add(roughData); // jagged array
            }
            SR.Close();
        }

        private void WriteMean(string fileName, string fileName2, ArrayList meanList, ArrayList dataList)
        {
            double[] tray = null;
            StreamWriter SW = new StreamWriter(fileName);
            StreamWriter SW2 = new StreamWriter(fileName2);
            for (int x = 0; x < dataList.Count; x++)
            {
                SW.WriteLine((double)meanList[x]);
                tray = (double[])dataList[x];
                for (int y = 0; y < tray.Length; y++)
                {
                    SW2.Write(tray[y] + " ");
                }
                SW2.WriteLine();
            }
            SW.Close();
            SW2.Close();
        }

    }
}
