using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MaterialDesignExtensions.Controllers
{
    public abstract class BitmapImageHelper
    {
        private BitmapImageHelper() { }

        public static BitmapImage LoadImage(string imageFilename, int targetWidth=40, int targetHeight = 40)
        {
            try
            {
                using (FileStream fileStream = new FileStream(imageFilename, FileMode.Open))
                {
                    BitmapDecoder bitmapDecoder = BitmapDecoder.Create(fileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);
                    int width = bitmapDecoder.Frames[0].PixelWidth;
                    int height = bitmapDecoder.Frames[0].PixelHeight;

                    fileStream.Position = 0;

                    BitmapImage image = new BitmapImage();
                    image.BeginInit();

                    double targetRatio = ((double)targetWidth) / targetHeight;
                    double ratio = ((double)width) / height;

                    if (targetRatio > ratio)
                    {
                        image.DecodePixelWidth = targetWidth;
                    }
                    else
                    {
                        image.DecodePixelHeight = targetHeight;
                    }

                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = fileStream;

                    image.EndInit();
                    image.Freeze();

                    return image;
                }
            }
            catch (Exception exc)
            {
                if (exc is IOException || exc is UnauthorizedAccessException)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
