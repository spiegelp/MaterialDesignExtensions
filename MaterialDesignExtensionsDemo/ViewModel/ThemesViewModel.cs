using System.Windows;

using MaterialDesignExtensions.Themes;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class ThemesViewModel : ViewModel
    {
        private PaletteHelper m_paletteHelper;

        private bool m_isDarkTheme;

        public bool IsDarkTheme
        {
            get
            {
                return m_isDarkTheme;
            }

            set
            {
                if (m_isDarkTheme != value)
                {
                    m_isDarkTheme = value;

                    OnPropertyChanged(nameof(IsDarkTheme));

                    SetTheme(m_isDarkTheme);
                }
            }
        }

        public ThemesViewModel()
            : base()
        {
            m_paletteHelper = new PaletteHelper();

            m_isDarkTheme = m_paletteHelper.IsDarkTheme();
        }

        private void SetTheme(bool isDarkTheme)
        {
            m_paletteHelper.SetLightDark(m_isDarkTheme);

            ((MainWindow)Application.Current.MainWindow).DialogHost.DialogTheme = isDarkTheme ? MaterialDesignThemes.Wpf.BaseTheme.Dark : MaterialDesignThemes.Wpf.BaseTheme.Light;
        }
    }
}
