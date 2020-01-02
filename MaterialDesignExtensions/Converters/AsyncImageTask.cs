using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Controllers;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// A task to load an image asynchronously.
    /// </summary>
    public class AsyncImageTask : INotifyPropertyChanged
    {
        private readonly object m_lockObject = new object();

        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private object m_image;

        /// <summary>
        /// The loaded image.
        /// </summary>
        public object Image
        {
            get
            {
                lock (m_lockObject)
                {
                    return m_image;
                }
            }

            private set
            {
                lock (m_lockObject)
                {
                    if (m_image != value)
                    {
                        m_image = value;

                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new AsyncImageTask to load the image with the specified width and height keeping its ratio.
        /// </summary>
        /// <param name="imageFilename">The full filename of the image</param>
        /// <param name="targetWidth">The target width of the image</param>
        /// <param name="targetHeight">The target height of the image</param>
        public AsyncImageTask(string imageFilename, int targetWidth = 40, int targetHeight = 40, bool useCache = false)
        {
            m_image = PackIconKind.FileImage;

            LoadImageAsync(imageFilename, targetWidth, targetHeight, useCache);
        }

        private async void LoadImageAsync(string imageFilename, int targetWidth, int targetHeight, bool useCache)
        {
            await Task.Run(async () =>
            {
#if DEBUG
                Console.WriteLine($"get image {imageFilename}");
#endif
                BitmapImage image = await BitmapImageHelper.LoadImageAsync(imageFilename, targetWidth, targetHeight, useCache).ConfigureAwait(false);

                if (image != null)
                {
                    Image = image;
                }
            }).ConfigureAwait(false);
        }
    }
}
