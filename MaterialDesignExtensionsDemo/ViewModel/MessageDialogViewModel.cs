using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class MessageDialogViewModel : ViewModel
    {
        public MessageDialogViewModel() : base() { }
    }

    public class SportsSelectionViewModel : ViewModel
    {
        private bool m_isValid;

        public bool IsValid
        {
            get
            {
                return m_isValid;
            }

            set
            {
                m_isValid = value;

                OnPropertyChanged(nameof(IsValid));
            }
        }

        public List<SportsItem> SportsItems { get; private set; }

        public SportsSelectionViewModel()
            : base()
        {
            m_isValid = true;

            SportsItems = new List<SportsItem>
            {
                new SportsItem("Football", PackIconKind.Soccer),
                new SportsItem("Handball", PackIconKind.Handball),
                new SportsItem("Hockey", PackIconKind.HockeySticks),
                new SportsItem("Racing", PackIconKind.RacingHelmet),
                new SportsItem("Tennis", PackIconKind.TennisRacket),
                new SportsItem("Volleyball", PackIconKind.Volleyball)
            };
        }

        public void ValidationHandler(object sender, InputDialogValidationEventArgs args)
        {
            IsValid = SportsItems.Any(sportsItem => sportsItem.IsSelected);

            args.CancelConfirmation = !IsValid;
        }
    }

    public class SportsItem : ViewModel
    {
        private string m_label;
        private PackIconKind m_icon;
        private bool m_isSelected;

        public PackIconKind Icon
        {
            get
            {
                return m_icon;
            }

            set
            {
                m_icon = value;

                OnPropertyChanged(nameof(Icon));
            }
        }

        public bool IsSelected
        {
            get
            {
                return m_isSelected;
            }

            set
            {
                m_isSelected = value;

                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public string Label
        {
            get
            {
                return m_label;
            }

            set
            {
                m_label = value;

                OnPropertyChanged(nameof(Label));
            }
        }

        public SportsItem(string label, PackIconKind icon)
        {
            m_label = label;
            m_icon = icon;
            m_isSelected = false;
        }
    }
}
