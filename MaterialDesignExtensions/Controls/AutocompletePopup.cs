using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// Another implementation of <see cref="Popup" /> to add a better behavior while moving or resizing the window.
    /// </summary>
    public class AutocompletePopup : Popup
    {
        private Window m_window;

        /// <summary>
        /// Creates a new <see cref="AutocompletePopup" />.
        /// </summary>
        public AutocompletePopup()
            : base()
        {
            m_window = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        protected virtual void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_window = Window.GetWindow(this);

            if (m_window != null)
            {
                m_window.SizeChanged += WindowSizeChangedHandler;
                m_window.LocationChanged += WindowLocationChanged;
            }
        }

        protected virtual void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            if (m_window != null)
            {
                m_window.SizeChanged -= WindowSizeChangedHandler;
                m_window.LocationChanged -= WindowLocationChanged;
            }
        }

        private void WindowSizeChangedHandler(object sender, SizeChangedEventArgs args)
        {
            UpdatePosition();
        }

        private void WindowLocationChanged(object sender, EventArgs args)
        {
            UpdatePosition();
        }

        /// <summary>
        /// A <code>Popup</code> does not update its location if the <code>PlacementTarget</code> changes its location for any reason.
        /// Call this method to ensure that the <code>Popup</code> will appear at the correct location.
        /// </summary>
        public void UpdatePosition()
        {
            if (IsOpen)
            {
                // change the offset to trigger an update of the Popup user interface
                double offset = VerticalOffset;
                VerticalOffset = offset + 1;
                VerticalOffset = offset;
            }
        }
    }
}
