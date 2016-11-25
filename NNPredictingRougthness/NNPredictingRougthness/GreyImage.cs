using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NNPredictingRougthness
{
    class GreyImage
    {
        private int width; // Width of image
        private int height; // Height of image       
        private byte Ga; // Mean grey level content of image
        private string pathName; // File path of image

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
