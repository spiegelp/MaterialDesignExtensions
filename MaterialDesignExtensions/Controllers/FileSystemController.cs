using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Model;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using FileSystemInfo = Pri.LongPath.FileSystemInfo;
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.Controllers
{
    /// <summary>
    /// Controller behind the <see cref="Controls.OpenDirectoryControl" />, <see cref="Controls.OpenFileControl" /> and <see cref="Controls.SaveFileControl" />.
    /// </summary>
    public class FileSystemController : INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private DirectoryInfo m_currentDirectory;
        private FileInfo m_currentFile;
        private string m_currentFileFullName;
        private List<DirectoryInfo> m_currentDirectoryPathParts;
        private List<DirectoryInfo> m_directories;
        private List<FileInfo> m_files;
        private bool m_showHiddenFilesAndDirectories;
        private bool m_showSystemFilesAndDirectories;
        private IList<IFileFilter> m_fileFilters;
        private IFileFilter m_fileFilterToApply;
        private bool m_forceFileExtensionOfFileFilter;

        /// <summary>
        /// The current directory shown in the control.
        /// </summary>
        public DirectoryInfo CurrentDirectory
        {
            get
            {
                return m_currentDirectory;
            }

            set
            {
                if (!AreObjectsEqual(m_currentDirectory, value))
                {
                    m_currentDirectory = value;

                    OnPropertyChanged(nameof(CurrentDirectory));
                }
            }
        }

        /// <summary>
        /// The list of sub directories to <see cref="CurrentDirectory" />.
        /// </summary>
        public List<DirectoryInfo> CurrentDirectoryPathParts
        {
            get
            {
                return m_currentDirectoryPathParts;
            }

            set
            {
                if (!AreObjectsEqual(m_currentDirectoryPathParts, value))
                {
                    m_currentDirectoryPathParts = value;

                    OnPropertyChanged(nameof(CurrentDirectoryPathParts));
                }
            }
        }

        /// <summary>
        /// The selected file of the control.
        /// </summary>
        public FileInfo CurrentFile
        {
            get
            {
                return m_currentFile;
            }

            set
            {
                if (!AreObjectsEqual(m_currentFile, value))
                {
                    m_currentFile = value;

                    OnPropertyChanged(nameof(CurrentFile));
                }
            }
        }

        /// <summary>
        /// The full filename (full path and name) of the selected file of the control.
        /// </summary>
        public string CurrentFileFullName
        {
            get
            {
                return m_currentFileFullName;
            }

            set
            {
                if (!AreObjectsEqual(m_currentFileFullName, value))
                {
                    m_currentFileFullName = value;

                    OnPropertyChanged(nameof(CurrentFileFullName));
                }
            }
        }

        /// <summary>
        /// The directories inside <see cref="CurrentDirectory" />.
        /// </summary>
        public List<DirectoryInfo> Directories
        {
            get
            {
                return m_directories;
            }

            set
            {
                if (!AreObjectsEqual(m_directories, value))
                {
                    m_directories = value;

                    OnPropertyChanged(nameof(Directories));
                }
            }
        }

        /// <summary>
        /// The directories and files inside <see cref="CurrentDirectory" />.
        /// </summary>
        public List<FileSystemInfo> DirectoriesAndFiles
        {
            get
            {
                List<FileSystemInfo> directoriesAndFiles = new List<FileSystemInfo>();

                if (m_directories != null)
                {
                    directoriesAndFiles.AddRange(m_directories);
                }

                if (m_files != null)
                {
                    directoriesAndFiles.AddRange(m_files);
                }

                return directoriesAndFiles;
            }
        }

        /// <summary>
        /// The system's drives.
        /// </summary>
        public List<SpecialDrive> Drives
        {
            get
            {
                DriveType[] supportedDriveTypes = { DriveType.CDRom, DriveType.Fixed, DriveType.Network, DriveType.Removable };

                return DriveInfo.GetDrives()
                    .Where(driveInfo => supportedDriveTypes.Contains(driveInfo.DriveType))
                    .Select(driveInfo =>
                    {
                        PackIconKind icon = PackIconKind.Harddisk;

                        if (driveInfo.DriveType == DriveType.CDRom)
                        {
                            icon = PackIconKind.Disc;
                        }
                        else if (driveInfo.DriveType == DriveType.Removable)
                        {
                            icon = PackIconKind.Usb;
                        }
                        else if (driveInfo.DriveType == DriveType.Network)
                        {
                            icon = PackIconKind.ServerNetwork;
                        }

                        string label = driveInfo.Name;

                        if (label.EndsWith(@"\"))
                        {
                            label = label.Substring(0, label.Length - 1);
                        }

                        string volumeLabel = driveInfo.IsReady ? driveInfo.VolumeLabel : null;

                        if (string.IsNullOrWhiteSpace(volumeLabel) && driveInfo.DriveType == DriveType.Fixed)
                        {
                            volumeLabel = Localization.Strings.LocalDrive;
                        }

                        if (!string.IsNullOrWhiteSpace(volumeLabel))
                        {
                            label = volumeLabel + " (" + label + ")";
                        }

                        return new SpecialDrive() { Info = driveInfo, Icon = icon, Label = label };
                    })
                    .ToList();
            }
        }

        /// <summary>
        /// The files inside <see cref="CurrentDirectory" />.
        /// </summary>
        public List<FileInfo> Files
        {
            get
            {
                return m_files;
            }

            set
            {
                if (!AreObjectsEqual(m_files, value))
                {
                    m_files = value;

                    OnPropertyChanged(nameof(Files));
                }
            }
        }

        /// <summary>
        /// The possible file filters to select from for applying to the files inside the current directory.
        /// </summary>
        public IList<IFileFilter> FileFilters
        {
            get
            {
                return m_fileFilters;
            }

            set
            {
                if (m_fileFilters != value)
                {
                    m_fileFilters = value;

                    OnPropertyChanged(nameof(FileFilters));
                }
            }
        }

        /// <summary>
        /// The file filter to apply to the files inside the current directory.
        /// </summary>
        public IFileFilter FileFilterToApply
        {
            get
            {
                return m_fileFilterToApply;
            }

            set
            {
                if (m_fileFilterToApply != value)
                {
                    m_fileFilterToApply = value;

                    OnPropertyChanged(nameof(FileFilterToApply));
                }
            }
        }

        /// <summary>
        /// Forces the possible file extension of the selected file filter for new filenames.
        /// </summary>
        public bool ForceFileExtensionOfFileFilter
        {
            get
            {
                return m_forceFileExtensionOfFileFilter;
            }

            set
            {
                if (m_forceFileExtensionOfFileFilter != value)
                {
                    m_forceFileExtensionOfFileFilter = value;

                    OnPropertyChanged(nameof(ForceFileExtensionOfFileFilter));
                }
            }
        }

        /// <summary>
        /// The special directories (e.g. music directory) of the user.
        /// </summary>
        public List<SpecialDirectory> SpecialDirectories
        {
            get
            {
                return new List<SpecialDirectory>()
                {
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)),
                        Icon = PackIconKind.Account },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),
                        Label = Localization.Strings.Documents, Icon = PackIconKind.FileDocument },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)),
                        Label = Localization.Strings.Pictures, Icon = PackIconKind.FileImage },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)),
                        Label = Localization.Strings.Music, Icon = PackIconKind.FileMusic },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)),
                        Label = Localization.Strings.Videos, Icon = PackIconKind.FileVideo },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)),
                        Label = Localization.Strings.Desktop, Icon = PackIconKind.Monitor }
                };
            }
        }

        /// <summary>
        /// Specifies whether hidden files and directories will be shown or not.
        /// </summary>
        public bool ShowHiddenFilesAndDirectories
        {
            get
            {
                return m_showHiddenFilesAndDirectories;
            }

            set
            {
                if (!m_showHiddenFilesAndDirectories != value)
                {
                    m_showHiddenFilesAndDirectories = value;

                    OnPropertyChanged(nameof(ShowHiddenFilesAndDirectories));
                }
            }
        }

        /// <summary>
        /// Specifies whether protected system files and directories will be shown or not.
        /// </summary>
        public bool ShowSystemFilesAndDirectories
        {
            get
            {
                return m_showSystemFilesAndDirectories;
            }

            set
            {
                if (m_showSystemFilesAndDirectories != value)
                {
                    m_showSystemFilesAndDirectories = value;

                    OnPropertyChanged(nameof(ShowSystemFilesAndDirectories));
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="FileSystemController" />.
        /// </summary>
        public FileSystemController()
        {
            m_currentDirectory = null;
            m_currentFile = null;
            m_currentFileFullName = null;
            m_currentDirectoryPathParts = null;
            m_directories = null;
            m_files = null;
            m_showHiddenFilesAndDirectories = false;
            m_showSystemFilesAndDirectories = false;
            m_fileFilters = null;
            m_fileFilterToApply = null;
            m_forceFileExtensionOfFileFilter = false;
        }

        /// <summary>
        /// Selects a new current directory.
        /// </summary>
        /// <param name="directory"></param>
        public void SelectDirectory(string directory)
        {
            SelectDirectory(new DirectoryInfo(directory));
        }

        /// <summary>
        /// Selects a new current directory.
        /// </summary>
        /// <param name="directory"></param>
        public void SelectDirectory(DirectoryInfo directory)
        {
            bool ShowFileSystemInfo(FileSystemInfo fileSystemInfo)
            {
                if ((ShowHiddenFilesAndDirectories || !fileSystemInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    && (ShowSystemFilesAndDirectories || !fileSystemInfo.Attributes.HasFlag(FileAttributes.System)))
                {
                    if (fileSystemInfo is FileInfo fileInfo && m_fileFilterToApply != null)
                    {
                        return m_fileFilterToApply.IsMatch(fileInfo);
                    }
                    else
                    {
                        return true;
                    }
                }

                return false;
            };

            if (directory != null && !string.IsNullOrWhiteSpace(directory.FullName))
            {
                if (!directory.Exists)
                {
                    throw new FileNotFoundException(string.Format(Localization.Strings.DirectoryXNotFound, directory.Name));
                }

                try
                {
                    // try to access the directory before assigning it as a kind of access control check
                    //     if the user is not allowed to access the directory, the controller does not change the current directory
                    List<DirectoryInfo> directories = directory.GetDirectories()
                        .Where(directoryInfo => ShowFileSystemInfo(directoryInfo))
                        .ToList();

                    List<FileInfo> files = directory.GetFiles()
                        .Where(fileInfo => ShowFileSystemInfo(fileInfo))
                        .ToList();

                    CurrentDirectory = directory;
                    Directories = directories;
                    Files = files;

                    OnPropertyChanged(nameof(DirectoriesAndFiles));
                }
                catch (UnauthorizedAccessException exc)
                {
                    throw new UnauthorizedAccessException(string.Format(Localization.Strings.AccessToDirectoryXDenied, directory.Name), exc);
                }
            }
            else
            {
                CurrentDirectory = null;
                Directories = null;
                Files = null;

                OnPropertyChanged(nameof(DirectoriesAndFiles));
            }

            UpdateCurrentDirectoryPathParts();
        }

        /// <summary>
        /// Selects a file.
        /// </summary>
        /// <param name="file"></param>
        public void SelectFile(string file)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {
                CurrentFile = new FileInfo(file);
            }
            else
            {
                CurrentFile = null;
            }

            // assign the string at the end, because we might get a PathTooLongException
            CurrentFileFullName = file;
        }

        /// <summary>
        /// Selects a file.
        /// </summary>
        /// <param name="file"></param>
        public void SelectFile(FileInfo file)
        {
            CurrentFile = file;

            if (!AreObjectsEqual(m_currentFile, file?.FullName))
            {
                CurrentFileFullName = file?.FullName;
            }
        }

        private void UpdateCurrentDirectoryPathParts()
        {
            List<DirectoryInfo> currentDirectoryPathParts = null;

            if (m_currentDirectory != null)
            {
                currentDirectoryPathParts = new List<DirectoryInfo>();
                DirectoryInfo directoryInfo = m_currentDirectory;

                while (directoryInfo != null)
                {
                    currentDirectoryPathParts.Add(directoryInfo);
                    directoryInfo = directoryInfo.Parent;
                }
                
                currentDirectoryPathParts.Sort((directoryInfo1, directoryInfo2) => directoryInfo1.FullName.CompareTo(directoryInfo2.FullName));
            }

            CurrentDirectoryPathParts = currentDirectoryPathParts;
        }

        /// <summary>
        /// Setter intended for internal use.
        /// </summary>
        /// <param name="showHiddenFilesAndDirectories"></param>
        public void SetShowHiddenFilesAndDirectories(bool showHiddenFilesAndDirectories)
        {
            ShowHiddenFilesAndDirectories = showHiddenFilesAndDirectories;

            SelectDirectory(m_currentDirectory);
        }

        /// <summary>
        /// Setter intended for internal use.
        /// </summary>
        /// <param name="showSystemFilesAndDirectories"></param>
        public void SetShowSystemFilesAndDirectories(bool showSystemFilesAndDirectories)
        {
            ShowSystemFilesAndDirectories = showSystemFilesAndDirectories;

            SelectDirectory(m_currentDirectory);
        }

        /// <summary>
        /// Sets a new file filter to apply to the files inside the current directory.
        /// </summary>
        /// <param name="fileFilters"></param>
        /// <param name="fileFilterToApply"></param>
        public void SetFileFilter(IList<IFileFilter> fileFilters, IFileFilter fileFilterToApply)
        {
            if (m_fileFilters != fileFilters)
            {
                FileFilters = fileFilters;
            }

            if (fileFilterToApply != null)
            {
                if (m_fileFilters != null && m_fileFilters.Any(fileFilter => fileFilter == fileFilterToApply))
                {
                    FileFilterToApply = fileFilterToApply;
                }
                else
                {
                    fileFilterToApply = null;
                }

                SelectDirectory(m_currentDirectory);
            }
            else
            {
                fileFilterToApply = null;

                SelectDirectory(m_currentDirectory);
            }
        }

        /// <summary>
        /// Creates a new directory with the specified name.
        /// </summary>
        /// <param name="newDirectoryName">The name of the new directory</param>
        public void CreateNewDirectory(string newDirectoryName)
        {
            if (string.IsNullOrWhiteSpace(newDirectoryName))
            {
                throw new ArgumentException(Localization.Strings.TheDirectoryNameMustNotBeEmpty);
            }

            if (!FileNameHelper.CheckFileName(newDirectoryName))
            {
                throw new ArgumentException(Localization.Strings.TheDirectoryNameIsInvalid);
            }

            string fullDirectoryName = CurrentDirectory.FullName + @"\" + newDirectoryName;
            DirectoryInfo newDirectory = new DirectoryInfo(fullDirectoryName);

            if (newDirectory.Exists)
            {
                throw new ArgumentException(string.Format(Localization.Strings.TheDirectoryXAlreadyExists, newDirectoryName));
            }
            
            // create directory and select it
            newDirectory.Create();

            // important: create a new DirectoryInfo instance, because the Exists property will not be updated by the Create() method
            SelectDirectory(new DirectoryInfo(fullDirectoryName));
        }

        /// <summary>
        /// Builds a full filename for the specified filename inside the current directory.
        /// </summary>
        /// <param name="newFilename">The filename to append to the current directory</param>
        /// <returns></returns>
        public string BuildFullFileNameForInCurrentDirectory(string newFilename)
        {
            string filename = null;

            if (!string.IsNullOrWhiteSpace(newFilename) && m_currentDirectory != null)
            {
                string directory = m_currentDirectory.FullName;

                if (directory != null && !directory.EndsWith(@"\") && !directory.EndsWith("/"))
                {
                    directory = $@"{directory}\";
                }

                filename = directory + newFilename.Trim();

                // ensure that the full filename has a file extension out of the selected file filter
                if (m_forceFileExtensionOfFileFilter && m_fileFilterToApply != null && !m_fileFilterToApply.IsMatch(filename))
                {
                    IEnumerable<string> fileExtensions = FileFilterHelper.GetFileExtensionsFromFilter(m_fileFilterToApply);

                    if (fileExtensions != null && fileExtensions.Any())
                    {
                        fileExtensions = fileExtensions.Select(fileExtension => fileExtension.ToLower());
                        string lowerCaseFilename = filename.ToLower();

                        bool hasWrongFileExtension = !fileExtensions.Any(fileExtension => lowerCaseFilename.EndsWith($".{fileExtension}"));

                        if (hasWrongFileExtension)
                        {
                            if (!filename.EndsWith("."))
                            {
                                filename = $"{filename}.";
                            }

                            filename = filename + fileExtensions.First();
                        }
                    }
                }
            }

            return filename;
        }

        private bool AreObjectsEqual(object o1, object o2)
        {
            if (o1 == o2)
            {
                return true;
            }

            if (o1 != null)
            {
                return o1.Equals(o2);
            }

            return false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
