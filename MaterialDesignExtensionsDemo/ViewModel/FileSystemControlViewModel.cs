﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public abstract class FileSystemControlViewModel : ViewModel
    {
        private string m_selectedAction;
        private bool m_showHiddenFilesAndDirectories;
        private bool m_showSystemFilesAndDirectories;
        private bool m_createNewDirectoryEnabled;
        private bool m_switchPathPartsAsButtonsEnabled;

        public override string DocumentationUrl
        {
            get
            {
                return "https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols";
            }
        }

        public bool CreateNewDirectoryEnabled
        {
            get
            {
                return m_createNewDirectoryEnabled;
            }

            set
            {
                m_createNewDirectoryEnabled = value;

                OnPropertyChanged(nameof(CreateNewDirectoryEnabled));
            }
        }

        public string SelectedAction
        {
            get
            {
                return m_selectedAction;
            }

            set
            {
                m_selectedAction = value;

                OnPropertyChanged(nameof(SelectedAction));
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

        public bool SwitchPathPartsAsButtonsEnabled
        {
            get
            {
                return m_switchPathPartsAsButtonsEnabled;
            }

            set
            {
                m_switchPathPartsAsButtonsEnabled = value;

                OnPropertyChanged(nameof(SwitchPathPartsAsButtonsEnabled));
            }
        }

        public FileSystemControlViewModel()
            : base()
        {
            m_selectedAction = null;
            m_showHiddenFilesAndDirectories = false;
            m_showSystemFilesAndDirectories = false;
            m_createNewDirectoryEnabled = false;
            m_switchPathPartsAsButtonsEnabled = false;
        }
    }
}
