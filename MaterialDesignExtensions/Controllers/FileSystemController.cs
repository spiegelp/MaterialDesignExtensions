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

        public FileSystemController()
        {
            m_currentDirectory = null;
            m_currentDirectoryPathParts = null;
            m_directories = null;
            m_files = null;
        }

        public void SelectDirectory(string directory)
        {
            SelectDirectory(new DirectoryInfo(directory));
        }

        public void SelectDirectory(DirectoryInfo directory)
        {
            if (directory != null && !string.IsNullOrWhiteSpace(directory.FullName))
            {
                CurrentDirectory = directory;
                Directories = CurrentDirectory.GetDirectories().ToList();
                Files = CurrentDirectory.GetFiles().ToList();
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
    }
}
