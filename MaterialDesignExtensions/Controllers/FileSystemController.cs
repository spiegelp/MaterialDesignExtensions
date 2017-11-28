using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controllers
{
    public class FileSystemController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DirectoryInfo m_currentDirectory;
        private List<DirectoryInfo> m_currentDirectoryPathParts;
        private List<DirectoryInfo> m_directories;
        private List<FileInfo> m_files;
        private bool m_showHiddenFilesAndDirectories;
        private bool m_showSystemFilesAndDirectories;

        public DirectoryInfo CurrentDirectory
        {
            get
            {
                return m_currentDirectory;
            }

            set
            {
                m_currentDirectory = value;

                OnPropertyChanged(nameof(CurrentDirectory));
            }
        }

        public List<DirectoryInfo> CurrentDirectoryPathParts
        {
            get
            {
                return m_currentDirectoryPathParts;
            }

            set
            {
                m_currentDirectoryPathParts = value;

                OnPropertyChanged(nameof(CurrentDirectoryPathParts));
            }
        }

        public List<DirectoryInfo> Directories
        {
            get
            {
                return m_directories;
            }

            set
            {
                m_directories = value;

                OnPropertyChanged(nameof(Directories));
            }
        }

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
                            icon = PackIconKind.Disk;
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

        public List<FileInfo> Files
        {
            get
            {
                return m_files;
            }

            set
            {
                m_files = value;

                OnPropertyChanged(nameof(Files));
            }
        }

        public List<SpecialDirectory> SpecialDirectories
        {
            get
            {
                return new List<SpecialDirectory>()
                {
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)), Icon = PackIconKind.Account },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), Icon = PackIconKind.FileDocument },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)), Icon = PackIconKind.FileImage },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)), Icon = PackIconKind.FileMusic },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)), Icon = PackIconKind.FileVideo },
                    new SpecialDirectory() { Info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)), Icon = PackIconKind.Monitor }
                };
            }
        }

        public bool ShowHiddenFilesAndDirectories
        {
            get
            {
                return m_showHiddenFilesAndDirectories;
            }

            set
            {
                m_showHiddenFilesAndDirectories = value;

                OnPropertyChanged(nameof(ShowHiddenFilesAndDirectories));
            }
        }

        public bool ShowSystemFilesAndDirectories
        {
            get
            {
                return m_showSystemFilesAndDirectories;
            }

            set
            {
                m_showSystemFilesAndDirectories = value;

                OnPropertyChanged(nameof(ShowSystemFilesAndDirectories));
            }
        }

        public FileSystemController()
        {
            m_currentDirectory = null;
            m_currentDirectoryPathParts = null;
            m_directories = null;
            m_files = null;
            m_showHiddenFilesAndDirectories = false;
            m_showSystemFilesAndDirectories = false;
        }

        public void SelectDirectory(string directory)
        {
            SelectDirectory(new DirectoryInfo(directory));
        }

        public void SelectDirectory(DirectoryInfo directory)
        {
            bool ShowFileSystemInfo(FileSystemInfo fileSystemInfo) => (ShowHiddenFilesAndDirectories || !fileSystemInfo.Attributes.HasFlag(FileAttributes.Hidden))
                                                                            && (ShowSystemFilesAndDirectories || !fileSystemInfo.Attributes.HasFlag(FileAttributes.System));

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
            }

            UpdateCurrentDirectoryPathParts();
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

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SetShowHiddenFilesAndDirectories(bool showHiddenFilesAndDirectories)
        {
            ShowHiddenFilesAndDirectories = showHiddenFilesAndDirectories;

            SelectDirectory(m_currentDirectory);
        }

        public void SetShowSystemFilesAndDirectories(bool showSystemFilesAndDirectories)
        {
            ShowSystemFilesAndDirectories = showSystemFilesAndDirectories;

            SelectDirectory(m_currentDirectory);
        }
    }
}
