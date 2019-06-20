using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Controllers;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converts a file extension to a content related icon.
    /// </summary>
    public class FileSystemInfoIconConverter : IValueConverter
    {
        private ISet<string> m_imageFileExtensions;
        private IDictionary<string, object> m_contentForFileExtension;

        /// <summary>
        /// The content mode to control the type of icons or thumbnails for image file types.
        /// </summary>
        public FileSystemInfoIconConverterImageMode ImageMode { get; set; }

        /// <summary>
        /// The width of a loaded thumbnail image.
        /// </summary>
        public int ImageTargetWidth { get; set; }

        /// <summary>
        /// The height of a loaded thumbnail image.
        /// </summary>
        public int ImageTargetHeight { get; set; }

        /// <summary>
        /// True to use a cache for loaded images.
        /// </summary>
        public bool UseCache { get; set; }

        /// <summary>
        /// Creates a new <see cref="FileSystemInfoIconConverter" />.
        /// </summary>
        public FileSystemInfoIconConverter()
        {
            ImageMode = FileSystemInfoIconConverterImageMode.Icon;
            ImageTargetWidth = 40;
            ImageTargetHeight = 40;
            UseCache = false;

            m_imageFileExtensions = new HashSet<string>()
            {
                "jpg",
                "jpeg",
                "png",
                "gif",
                "bmp",
                "tiff"
            };

            m_contentForFileExtension = new Dictionary<string, object>()
            {
                // code
                ["cs"] = PackIconKind.LanguageCsharp,
                ["xaml"] = PackIconKind.Xml,
                ["ts"] = PackIconKind.LanguageTypescript,
                ["js"] = PackIconKind.LanguageJavascript,
                ["java"] = PackIconKind.LanguageJava,
                ["c"] = PackIconKind.LanguageC,
                ["cpp"] = PackIconKind.LanguageCpp,
                ["h"] = PackIconKind.CodeBraces,
                ["py"] = PackIconKind.LanguagePython,
                ["php"] = PackIconKind.LanguagePhp,
                ["r"] = PackIconKind.LanguageR,

                // compiled code and executables
                ["exe"] = PackIconKind.Settings,
                ["dll"] = PackIconKind.Settings,
                ["class"] = PackIconKind.Settings,
                ["jar"] = PackIconKind.Settings,
                ["lib"] = PackIconKind.Settings,
                ["so"] = PackIconKind.Settings,
                ["a"] = PackIconKind.Settings,
                ["bat"] = PackIconKind.Settings,
                ["sh"] = PackIconKind.Settings,

                // web, xml, json, sql
                ["html"] = PackIconKind.Xml,
                ["htm"] = PackIconKind.Xml,
                ["css"] = PackIconKind.CodeTags,
                ["xml"] = PackIconKind.Xml,
                ["json"] = PackIconKind.Json,
                ["xsl"] = PackIconKind.Xml,
                ["xslt"] = PackIconKind.Xml,
                ["sql"] = PackIconKind.Database,

                // databases
                ["db"] = PackIconKind.Database,
                ["sqlite"] = PackIconKind.Database,
                ["sqlite2"] = PackIconKind.Database,
                ["sqlite3"] = PackIconKind.Database,
                ["sqlite4"] = PackIconKind.Database,
                ["litedb"] = PackIconKind.Database,
                ["litedb2"] = PackIconKind.Database,
                ["litedb3"] = PackIconKind.Database,
                ["litedb4"] = PackIconKind.Database,
                ["fdb"] = PackIconKind.Database,
                ["mdf"] = PackIconKind.Database,

                // music
                ["mp3"] = PackIconKind.FileMusic,
                ["wma"] = PackIconKind.FileMusic,
                ["wav"] = PackIconKind.FileMusic,
                ["aac"] = PackIconKind.FileMusic,
                ["aa"] = PackIconKind.FileMusic,
                ["aiff"] = PackIconKind.FileMusic,

                // video
                ["mp4"] = PackIconKind.FileVideo,
                ["mpeg"] = PackIconKind.FileVideo,
                ["mpg"] = PackIconKind.FileVideo,
                ["wmv"] = PackIconKind.FileVideo,
                ["flv"] = PackIconKind.FileVideo,
                ["avi"] = PackIconKind.FileVideo,
                ["flv"] = PackIconKind.FileVideo,
                ["mov"] = PackIconKind.FileVideo,
                ["webm"] = PackIconKind.FileVideo,
                ["mkv"] = PackIconKind.FileVideo,
                ["rm"] = PackIconKind.FileVideo,

                // compressed
                ["zip"] = PackIconKind.ZipBox,
                ["7z"] = PackIconKind.ZipBox,
                ["rar"] = PackIconKind.ZipBox,

                // common document types
                ["docx"] = PackIconKind.FileWord,
                ["doc"] = PackIconKind.FileWord,
                ["xlsx"] = PackIconKind.FileExcel,
                ["xls"] = PackIconKind.FileExcel,
                ["pptx"] = PackIconKind.FilePowerpoint,
                ["ppt"] = PackIconKind.FilePowerpoint,
                ["pps"] = PackIconKind.FilePowerpoint,
                ["pdf"] = PackIconKind.FilePdf,
                ["txt"] = PackIconKind.FileDocument,
                ["rtf"] = PackIconKind.FileDocument
            };

            foreach (string fileExtension in m_imageFileExtensions)
            {
                m_contentForFileExtension[fileExtension] = PackIconKind.FileImage;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DirectoryInfo directory)
            {
                return PackIconKind.Folder;
            }
            else if (value is FileInfo file)
            {
                return GetContentForFile(file.FullName);
            }
            else if (value is string path)
            {
                if (Directory.Exists(path))
                {
                    return PackIconKind.Folder;
                }
                else if (File.Exists(path))
                {
                    return GetContentForFile(path);
                }
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        private object GetContentForFile(string filename)
        {
            string fileExtension = string.Empty;
            int dotIndex = filename.LastIndexOf('.');

            if (dotIndex > -1)
            {
                fileExtension = filename.Substring(dotIndex + 1).ToLower();
            }

            if (m_imageFileExtensions.Contains(fileExtension))
            {
                if (ImageMode == FileSystemInfoIconConverterImageMode.Image)
                {
                    return BitmapImageHelper.LoadImage(filename, ImageTargetWidth, ImageTargetHeight, UseCache);
                }
                else if (ImageMode == FileSystemInfoIconConverterImageMode.AsyncImageTask)
                {
                    return new AsyncImageTask(filename, ImageTargetWidth, ImageTargetHeight, UseCache);
                }
            }

            if (!m_contentForFileExtension.TryGetValue(fileExtension, out object content))
            {
                content = PackIconKind.File;
            }

            return content;
        }
    }

    /// <summary>
    /// The content mode to control the type of icons or thumbnails for image file types.
    /// </summary>
    public enum FileSystemInfoIconConverterImageMode : byte
    {
        /// <summary>
        /// Only use icons
        /// </summary>
        Icon,

        /// <summary>
        /// Use thumbnails for images
        /// </summary>
        Image,

        /// <summary>
        /// Use async task to load thumbnail images
        /// </summary>
        AsyncImageTask
    }
}
