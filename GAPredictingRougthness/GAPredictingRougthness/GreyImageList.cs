using System;
using System.IO;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GAPredictingRougthness
{
    class GreyImageList
    {
        private List<GreyImage> roughList; // roughness data image list
        private List<GreyImage> evalList;
        private BitmapSource bitmapIn;
        private byte[] pixelArray;
        private byte Ga; // Grey level content of a greyscale image

        static double maxSpeed = double.MinValue;
        static double maxFeed = double.MinValue;
        static double maxDepth = double.MinValue;
        static double maxGa = double.MinValue;
        static double maxRa = double.MinValue;

        static double minSpeed = double.MaxValue;
        static double minFeed = double.MaxValue;
        static double minDepth = double.MaxValue;
        static double minGa = double.MaxValue;
        static double minRa = double.MaxValue;

        public GreyImageList() // Grey images list class
        {
            roughList = new List<GreyImage>();
            evalList = new List<GreyImage>();
            Ga = 0;
            pixelArray = null;
            LoadData(@"..\..\..\..\Images\Roughness Data\SmallerImages", "RoughnessDataSmall.txt", roughList, false);
            LoadData(@"..\..\..\..\Images\Evaluation Data\SmallerImages", "EvaluationSmall.txt", evalList, false);
            // File directory of the images 
            //Display(image, bitmapIn, roughList);
        }

        private void LoadData(string filesPath, string fileName, List<GreyImage> List, bool writeToFile)
        // Load "image files" to List from filesPath folder and write details to a text file
        {
            string[] imageFiles = Directory.GetFiles(filesPath, "*.jpg");
            if (imageFiles == null)
            {
                return;
            }
            if (writeToFile)
            {
                StreamWriter SW = new StreamWriter(fileName);
                SW.WriteLine("Speed (V), Feed (F), Depth (D), Grey Level (Ga), Roughness (Ra)");
                for (int x = 0; x < imageFiles.Length; x++)
                {
                    bitmapIn = new BitmapImage(new Uri(Path.GetFullPath(imageFiles[x])));
                    if (bitmapIn.Format != PixelFormats.Gray8) // Convert image format to greyscale
                    {
                        bitmapIn = new FormatConvertedBitmap(bitmapIn, PixelFormats.Gray8, null, 0);
                    }
                    pixelArray = new byte[bitmapIn.PixelHeight * bitmapIn.PixelWidth];
                    getPixels(bitmapIn); // Load image pixel data into an array
                    Ga = meanGrey(); // compute mean grey level content of an image
                    imageFiles[x] = FileName(imageFiles[x], filesPath + @"\"); // Modify file name
                    GreyImage greyCopy = new GreyImage(bitmapIn, Ga, imageFiles[x]);
                    List.Add(greyCopy); // Add grey image object to grey image ArrayList
                    writeGaData(SW, imageFiles[x]); // Write Image pixel data in text file 
                }
                SW.Close();
            }
            else
            {
                for (int x = 0; x < imageFiles.Length; x++)
                {
                    bitmapIn = new BitmapImage(new Uri(Path.GetFullPath(imageFiles[x])));
                    if (bitmapIn.Format != PixelFormats.Gray8) // Convert image format to greyscale
                    {
                        bitmapIn = new FormatConvertedBitmap(bitmapIn, PixelFormats.Gray8, null, 0);
                    }
                    pixelArray = new byte[bitmapIn.PixelHeight * bitmapIn.PixelWidth];
                    getPixels(bitmapIn); // Load image pixel data into an array
                    Ga = meanGrey(); // compute mean grey level content of an image
                    imageFiles[x] = FileName(imageFiles[x], filesPath + @"\"); // Modify file name
                    GreyImage greyCopy = new GreyImage(bitmapIn, Ga, imageFiles[x], GetSurfaceFromFileName(imageFiles[x]));
                    List.Add(greyCopy); // Add grey image object to grey image ArrayList
                }
            }
        }

        private Surface GetSurfaceFromFileName(string fileName)
        {
            string[] VFDRa = fileName.Split('_');
            Surface newSurface = new Surface(double.Parse(VFDRa[0]), double.Parse(VFDRa[1]), double.Parse(VFDRa[2]), System.Convert.ToDouble(Ga), double.Parse(VFDRa[3]));
            if (newSurface.getSpeed() > maxSpeed)
            {
                maxSpeed = newSurface.getSpeed();
            }

            if (newSurface.getSpeed() < minSpeed)
            {
                minSpeed = newSurface.getSpeed();
            }

            if (newSurface.getFeed() > maxFeed)
            {
                maxFeed = newSurface.getFeed();
            }

            if (newSurface.getFeed() < minFeed)
            {
                minFeed = newSurface.getFeed();
            }

            if (newSurface.getDepth() > maxDepth)
            {
                maxDepth = newSurface.getDepth();
            }

            if (newSurface.getDepth() < minDepth)
            {
                minDepth = newSurface.getDepth();
            }

            if (newSurface.getGa() > maxGa)
            {
                maxGa = newSurface.getGa();
            }

            if (newSurface.getGa() < minGa)
            {
                minGa = newSurface.getGa();
            }

            if (newSurface.getRa() > maxRa)
            {
                maxRa = newSurface.getRa();
            }

            if (newSurface.getRa() < minRa)
            {
                minRa = newSurface.getRa();
            }
            return newSurface;
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

        public List<GreyImage> GetTestGreyImages()
        {
            return roughList;
        }

        public List<GreyImage> GetEvalGreyImages()
        {
            return evalList;
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
                Console.WriteLine("An error has occured | " + ex.Message);
            }
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

        public static double descaleSpeed(double speed)
        {
            return descaleValue(speed, minSpeed, maxSpeed, -0.5, 0.5);
        }

        public static double descaleFeed(double feed)
        {
            return descaleValue(feed, minFeed, maxFeed, -0.5, 0.5);
        }

        public static double descaleDepth(double depth)
        {
            return descaleValue(depth, minDepth, maxDepth, -0.5, 0.5);
        }

        public static double descaleGa(double Ga)
        {
            return descaleValue(Ga, minGa, maxGa, -0.5, 0.5);
        }

        public static double scaleRa(double Ra)
        {
            return scaleValue(Ra, minRa, maxRa, 0, 1);
        }

        public static double descaleRa(double scaleRa)
        {
            return descaleValue(scaleRa, minRa, maxRa, 0, 1);
        }

        public static double initeSpeed()
        {
            return descaleValue(GeneticAlgo.random.NextDouble() - 0.5, minSpeed, maxSpeed, -0.5, 0.5);
        }

        public static double initFeed()
        {
            return descaleValue(GeneticAlgo.random.NextDouble() - 0.5, minFeed, maxFeed, -0.5, 0.5);
        }

        public static double initDepth()
        {
            return descaleValue(GeneticAlgo.random.NextDouble() - 0.5, minDepth, maxDepth, -0.5, 0.5);
        }

        public static double initGa()
        {
            return descaleValue(GeneticAlgo.random.NextDouble() - 0.5, minGa, maxGa, -0.5, 0.5);
        }

        public static void DrawImage(byte[] pixelArray, String filename)
        {
            System.Drawing.Image xy = byteArrayToImage(pixelArray);
            xy.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            int size = (int)Math.Sqrt(byteArrayIn.Length); // Some bytes will not be used as we round down here

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);

            try
            {
                // Copy byteArrayIn to bitmapData row by row (to account for the case
                // where bitmapData.Stride != bitmap.Width)
                for (int rowIndex = 0; rowIndex < bitmapData.Height; ++rowIndex)
                    Marshal.Copy(byteArrayIn, rowIndex * bitmap.Width, bitmapData.Scan0 + rowIndex * bitmapData.Stride, bitmap.Width);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            var newPalette = bitmap.Palette;
            for (int index = 0; index < bitmap.Palette.Entries.Length; ++index)
            {
                var entry = bitmap.Palette.Entries[index];
                var gray = (int)(0.30 * entry.R + 0.59 * entry.G + 0.11 * entry.B);
                newPalette.Entries[index] = System.Drawing.Color.FromArgb(gray, gray, gray);
            }
            bitmap.Palette = newPalette;

            return bitmap;
        }
    }
}
