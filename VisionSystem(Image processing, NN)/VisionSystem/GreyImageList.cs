using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VisionSystem
{
    class GreyImageList
    {
        private ArrayList roughList; // roughness data image list
        private ArrayList evalList;
        private BitmapSource bitmapIn;
        private byte[] pixelArray;
        private byte Ga; // Grey level content of a greyscale image
        public GreyImageList(Image image) // Grey images list class
        {
            roughList = new ArrayList();
            evalList = new ArrayList();
            Ga = 0;
            pixelArray = null;
            LoadData(@"C:\Users\cc\Desktop\Machined Surfaces\Roughness Data", "RoughnessData.txt", roughList);
            LoadData(@"C:\Users\cc\Desktop\Machined Surfaces\Evaluation Data", "Evaluation.txt", evalList);
            // File directory of the images 
            Display(image, bitmapIn, roughList);
        }

        private void LoadData(string filesPath, string fileName, ArrayList List)
            // Load "image files" to List from filesPath folder and write details to a text file
        {
            string[] imageFiles = Directory.GetFiles(filesPath, "*.jpg");
            if (imageFiles == null)
            {
                return;
            }
            StreamWriter SW = new StreamWriter(fileName);
            SW.WriteLine("Speed (V), Feed (F), Depth (D), Grey Level (Ga), Roughness (Ra)");
            for (int x = 0; x < imageFiles.Length; x++)
            {
                bitmapIn = new BitmapImage(new Uri(imageFiles[x]));
                if (bitmapIn.Format != PixelFormats.Gray8) // Convert image format to greyscale
                {
                    bitmapIn = new FormatConvertedBitmap(bitmapIn, PixelFormats.Gray8, null, 0);
                }
                pixelArray = new byte[bitmapIn.PixelHeight * bitmapIn.PixelWidth];
                getPixels(bitmapIn); // Load image pixel data into an array
                Ga = meanGrey(); // compute mean grey level content of an image
                imageFiles[x] = FileName(imageFiles[x], filesPath+@"\"); // Modify file name
                GreyImage greyCopy = new GreyImage(bitmapIn, Ga, imageFiles[x]);
                List.Add(greyCopy); // Add grey image object to grey image ArrayList
                writeGaData(SW, imageFiles[x]); // Write Image pixel data in text file 
            }
            SW.Close();
        }

        private byte meanGrey() // Mean grey level content of the image (Ga)
        {
            long ga = 0;
            int dim = pixelArray.Length;
            for (int x = 0; x < dim; x++)
            {
                ga = ga + pixelArray[x];
            }
            return Convert.ToByte(ga / dim);
        }

        private void writeGaData(StreamWriter SW, string fileName)
        {
            string[] VFDRa = fileName.Split('_');
            SW.WriteLine("{0},{1},{2},{3},{4}", VFDRa[0], VFDRa[1], VFDRa[2], Ga, VFDRa[3]);
            //writeArray(pixelArray, SW);    
        }

        private void writeArray(byte[] array, StreamWriter SW)
        {
            for (int x = 0; x < array.Length; x++)
            {
                SW.Write(array[x] + " ");
            }
            SW.WriteLine("\n\n\n\n\n\n\n");
        }

        private void getPixels(BitmapSource bitmap)
        {
            bitmap.CopyPixels(pixelArray, bitmap.PixelWidth, 0); //Load pixel data into an array
        }

        private string FileName(string path, string mask)
        {
            string fileName = null;
            fileName = path.Replace(mask, "");
            fileName = fileName.Replace(".JPG", "");
            return fileName;
        }

        private WriteableBitmap Write(BitmapSource bitmap)
        {
            WriteableBitmap bitmapInGrey;
            bitmap.CopyPixels(pixelArray, bitmap.PixelWidth, 0); //Load pixel data into an array
            bitmapInGrey = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Gray8, null);
            bitmapInGrey.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixelArray, bitmap.PixelWidth, 0);
            return bitmapInGrey;
        }

        private void Display(Image image, BitmapSource temp, ArrayList List)
        {
            try
            {
                if (List == null)
                {
                    return;
                }
                WriteableBitmap bitmap = Write(temp);
                image.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error has occured");
            }
        }
    }
}
