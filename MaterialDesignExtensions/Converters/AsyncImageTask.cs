using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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
        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// The loaded image.
        /// </summary>
        public object Image { get; private set; }

        /// <summary>
        /// Creates a new AsyncImageTask to load the image with the specified width and height keeping its ratio.
        /// </summary>
        /// <param name="imageFilename"></param>
        /// <param name="targetWidth"></param>
        /// <param name="targetHeight"></param>
        public AsyncImageTask(string imageFilename, int targetWidth = 40, int targetHeight = 40)
        {
            Image = PackIconKind.FileImage;

            LoadImageAsync(imageFilename, targetWidth, targetHeight);
        }

        private async void LoadImageAsync(string imageFilename, int targetWidth, int targetHeight)
        {
            Image = await Task.Run(() => BitmapImageHelper.LoadImage(imageFilename, targetWidth, targetHeight));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
        }
    }
}
