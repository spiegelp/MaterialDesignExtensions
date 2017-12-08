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
    public class AsyncImageTask : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public object Image { get; private set; }

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
