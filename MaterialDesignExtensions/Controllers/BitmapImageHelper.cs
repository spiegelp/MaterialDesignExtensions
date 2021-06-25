using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MaterialDesignExtensions.Controllers
{
    /// <summary>
    /// Helper class to load images.
    /// </summary>
    public abstract class BitmapImageHelper
    {
        private static readonly object m_staticLockObject = new object();

        private static readonly int BufferSize = 1024 * 64;

        private static readonly Dictionary<string, BitmapImage> Cache = new Dictionary<string, BitmapImage>();

        private BitmapImageHelper() { }

        /// <summary>
        /// Loads the image from the file with the specified width and height keeping its ratio.
        /// </summary>
        /// <param name="imageFilename"></param>
        /// <param name="targetWidth"></param>
        /// <param name="targetHeight"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public static BitmapImage LoadImage(string imageFilename, int targetWidth = 40, int targetHeight = 40, bool useCache = false)
        {
            string key = GetKey(imageFilename, targetWidth, targetHeight);

            BitmapImage image = null;

            if (useCache)
            {
                lock (m_staticLockObject)
                {
                    Cache.TryGetValue(key, out image);
                }

                if (image != null)
                {
                    return image;
                }
            }

            try
            {
                using (MemoryStream imageMemoryStream = new MemoryStream())
                {
                    using (BufferedStream fileStream = new BufferedStream(new FileStream(imageFilename, FileMode.Open), BufferSize))
                    {
                        fileStream.CopyTo(imageMemoryStream);
                    }

                    return GetAsBitmapImage(imageMemoryStream, targetWidth, targetHeight, useCache, key);
                }
            }
            catch (Exception exc)
            {
                if (exc is IOException || exc is UnauthorizedAccessException || exc is PathTooLongException || exc is NotSupportedException)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Loads the image from the file with the specified width and height keeping its ratio.
        /// </summary>
        /// <param name="imageFilename"></param>
        /// <param name="targetWidth"></param>
        /// <param name="targetHeight"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public static async Task<BitmapImage> LoadImageAsync(string imageFilename, int targetWidth = 40, int targetHeight = 40, bool useCache = false)
        {
            string key = GetKey(imageFilename, targetWidth, targetHeight);

            BitmapImage image = null;

            if (useCache)
            {
                lock (m_staticLockObject)
                {
                    Cache.TryGetValue(key, out image);
                }

                if (image != null)
                {
                    return image;
                }
            }

            try
            {
                using (MemoryStream imageMemoryStream = new MemoryStream())
                {
                    using (BufferedStream fileStream = new BufferedStream(new FileStream(imageFilename, FileMode.Open), BufferSize))
                    {
                        await fileStream.CopyToAsync(imageMemoryStream).ConfigureAwait(false);
                    }

                    return GetAsBitmapImage(imageMemoryStream, targetWidth, targetHeight, useCache, key);
                }
            }
            catch (Exception exc)
            {
                if (exc is IOException || exc is UnauthorizedAccessException || exc is PathTooLongException || exc is NotSupportedException)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        private static string GetKey(string imageFilename, int targetWidth, int targetHeight)
        {
            return string.Format("{0}_{1}_file://{2}", targetWidth, targetHeight, imageFilename);
        }

        private static BitmapImage GetAsBitmapImage(MemoryStream imageMemoryStream, int targetWidth, int targetHeight, bool useCache, string key)
        {
            imageMemoryStream.Position = 0;

            BitmapDecoder bitmapDecoder = BitmapDecoder.Create(imageMemoryStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);
            int width = bitmapDecoder.Frames[0].PixelWidth;
            int height = bitmapDecoder.Frames[0].PixelHeight;

            imageMemoryStream.Position = 0;

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
            image.StreamSource = imageMemoryStream;

            image.EndInit();
            image.StreamSource = null;
            image.Freeze();

            if (useCache)
            {
                lock (m_staticLockObject)
                {
                    Cache[key] = image;
                }
            }

            return image;
        }

        /// <summary>
        /// Clears the cache of loaded images.
        /// </summary>
        public static void ClearCache()
        {
            lock (m_staticLockObject)
            {
                Cache.Clear();
            }
        }
    }
}
