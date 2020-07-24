using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class WindowStyleViewModel : ViewModel
    {
        private WindowStyle m_selectedWindowStyle;

        public List<WindowStyle> WindowStyles
        {
            get
            {
                return new List<WindowStyle>
                {
                    WindowStyle.SingleBorderWindow, WindowStyle.ThreeDBorderWindow, WindowStyle.ToolWindow, WindowStyle.None
                };
            }
        }

        public WindowStyle SelectedWindowStyle
        {
            get
            {
                return m_selectedWindowStyle;
            }

            set
            {
                m_selectedWindowStyle = value;

                OnPropertyChanged(nameof(SelectedWindowStyle));
            }
        }

        public WindowStyleViewModel()
            : base()
        {
            m_selectedWindowStyle = WindowStyle.SingleBorderWindow;
        }
    }
}
