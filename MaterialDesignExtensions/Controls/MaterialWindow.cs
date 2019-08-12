using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MaterialDesignExtensions.Controls
{
    public class MaterialWindow : Window
    {
        private const string MinimizeButtonName = "minimizeButton";
        private const string MaximizeRestoreButtonName = "maximizeRestoreButton";
        private const string CloseButtonName = "closeButton";

        public static readonly DependencyProperty BorderBackgroundBrushProperty = DependencyProperty.Register(
            nameof(BorderBackgroundBrush),
            typeof(Brush),
            typeof(MaterialWindow),
            new FrameworkPropertyMetadata(null, null));

        public Brush BorderBackgroundBrush
        {
            get
            {
                return (Brush)GetValue(BorderBackgroundBrushProperty);
            }

            set
            {
                SetValue(BorderBackgroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty BorderForegroundBrushProperty = DependencyProperty.Register(
            nameof(BorderForegroundBrush),
            typeof(Brush),
            typeof(MaterialWindow),
            new FrameworkPropertyMetadata(null, null));

        public Brush BorderForegroundBrush
        {
            get
            {
                return (Brush)GetValue(BorderForegroundBrushProperty);
            }

            set
            {
                SetValue(BorderForegroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty FadeContentIfInactiveProperty = DependencyProperty.Register(
                nameof(FadeContentIfInactive),
                typeof(bool),
                typeof(MaterialWindow),
                new FrameworkPropertyMetadata(false)
        );

        public bool FadeContentIfInactive
        {
            get
            {
                return (bool)GetValue(FadeContentIfInactiveProperty);
            }

            set
            {
                SetValue(FadeContentIfInactiveProperty, value);
            }
        }

        private Button m_minimizeButton;
        private Button m_maximizeRestoreButton;
        private Button m_closeButton;

        static MaterialWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialWindow), new FrameworkPropertyMetadata(typeof(MaterialWindow)));
        }

        public MaterialWindow() : base() { }

        public override void OnApplyTemplate()
        {
            if (m_minimizeButton != null)
            {
                m_minimizeButton.Click -= MinimizeButtonClickHandler;
            }

            m_minimizeButton = GetTemplateChild(MinimizeButtonName) as Button;

            if (m_minimizeButton != null)
            {
                m_minimizeButton.Click += MinimizeButtonClickHandler;
            }

            if (m_maximizeRestoreButton != null)
            {
                m_maximizeRestoreButton.Click -= MaximizeRestoreButtonClickHandler;
            }

            m_maximizeRestoreButton = GetTemplateChild(MaximizeRestoreButtonName) as Button;

            if (m_maximizeRestoreButton != null)
            {
                m_maximizeRestoreButton.Click += MaximizeRestoreButtonClickHandler;
            }

            if (m_closeButton != null)
            {
                m_closeButton.Click -= CloseButtonClickHandler;
            }

            m_closeButton = GetTemplateChild(CloseButtonName) as Button;

            if (m_closeButton != null)
            {
                m_closeButton.Click += CloseButtonClickHandler;
            }

            base.OnApplyTemplate();
        }

        private void CloseButtonClickHandler(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void MaximizeRestoreButtonClickHandler(object sender, RoutedEventArgs args)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeButtonClickHandler(object sender, RoutedEventArgs args)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
