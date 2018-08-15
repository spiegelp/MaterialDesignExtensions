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
    /// A base class to provide common functionality for controls with a <code>Popup</code>.
    /// </summary>
    public abstract class ControlWithAutocompletePopup : Control
    {
        protected Window m_window;
        protected AutocompletePopup m_popup;

        /// <summary>
        /// Creates a new <code>ControlWithPopup</code>.
        /// </summary>
        public ControlWithAutocompletePopup()
            : base()
        {
            m_window = null;
            m_popup = null;

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

        protected virtual void WindowSizeChangedHandler(object sender, SizeChangedEventArgs args)
        {
            ClearFocus();
        }

        protected virtual void WindowLocationChanged(object sender, EventArgs args)
        {
            ClearFocus();
        }

        protected void ClearFocus()
        {
            if (IsKeyboardFocusWithin)
            {
                Keyboard.ClearFocus();
            }
        }

        /// <summary>
        /// A <code>Popup</code> does not update its location if the <code>PlacementTarget</code> changes its location for any reason.
        /// Call this method to ensure that the <code>Popup</code> will appear at the correct location.
        /// </summary>
        public void UpdatePopup()
        {
            m_popup?.UpdatePosition();
        }
    }
}
