using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GAPredictingRougthness
{
    class GreyImage
    {
        private int width; // Width of image
        private int height; // Height of image       
        private byte Ga; // Mean grey level content of image
        private string pathName; // File path of image
        public List<Double> scaledPixelArray;
        public Surface surface;

        public GreyImage(BitmapSource bitmap, byte ga, string nem)
        {
            if (bitmap.Format != PixelFormats.Gray8) // Convert image format to greyscale
            {
                bitmap = new FormatConvertedBitmap(bitmap, PixelFormats.Gray8, null, 0);
            }
            width = bitmap.PixelWidth; // Width of image
            height = bitmap.PixelHeight; // height of image      
            pathName = nem; // File path of image
            Ga = ga; // mean grey level content of image
        }

        public GreyImage(BitmapSource bitmap, byte ga, string nem,Surface surface)
        {
            if (bitmap.Format != PixelFormats.Gray8) // Convert image format to greyscale
            {
                bitmap = new FormatConvertedBitmap(bitmap, PixelFormats.Gray8, null, 0);
            }
            byte[] bytePixelArray = new byte[bitmap.PixelHeight * bitmap.PixelWidth];
            bitmap.CopyPixels(bytePixelArray, bitmap.PixelWidth, 0);

            scaledPixelArray = new List<double>();
            foreach (byte curByte in bytePixelArray)
            {
                scaledPixelArray.Add(System.Convert.ToDouble(curByte)/255.0);
            }

            width = bitmap.PixelWidth; // Width of image
            height = bitmap.PixelHeight; // height of image      
            pathName = nem; // File path of image
            Ga = ga; // mean grey level content of image
            this.surface = surface;
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public byte getGa()
        {
            return Ga;
        }

    }
}
