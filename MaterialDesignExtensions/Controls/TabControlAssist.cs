using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// Contains attached properties for <code>TabControl</code>.
    /// </summary>
    public static class TabControlAssist
    {
        /// <summary>
        /// The alignment of the horizontal tab headers in the <code>TabControl</code>.
        /// Obsolete: This property is going to be replaced by <see cref="TabHeaderHorizontalAlignment" /> in a future release
        /// </summary>
        [Obsolete]
        public static readonly DependencyProperty TabHeaderAlignmentProperty = DependencyProperty.RegisterAttached(
            "TabHeaderAlignment",
            typeof(HorizontalAlignment),
            typeof(TabControlAssist),
            new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.Inherits, TabHeaderAlignmentPropertyChangedHandler)
        );

        /// <summary>
        /// Gets the alignment of the horizontal tab headers in the <code>TabControl</code>.
        /// Obsolete: This property is going to be replaced by <see cref="TabHeaderHorizontalAlignment" /> in a future release
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        [Obsolete]
        public static HorizontalAlignment GetTabHeaderAlignment(DependencyObject element)
        {
            return (HorizontalAlignment)element.GetValue(TabHeaderAlignmentProperty);
        }

        /// <summary>
        /// Sets the alignment of the horizontal tab headers in the <code>TabControl</code>.
        /// Obsolete: This property is going to be replaced by <see cref="TabHeaderHorizontalAlignment" /> in a future release
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        [Obsolete]
        public static void SetTabHeaderAlignment(DependencyObject element, HorizontalAlignment value)
        {
            element.SetValue(TabHeaderAlignmentProperty, value);
        }

        private static void TabHeaderAlignmentPropertyChangedHandler(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            if ((HorizontalAlignment)args.NewValue != GetTabHeaderHorizontalAlignment(element))
            {
                SetTabHeaderHorizontalAlignment(element, (HorizontalAlignment)args.NewValue);
            }
        }

        /// <summary>
        /// The alignment of the horizontal tab headers in the <code>TabControl</code>.
        /// </summary>
        public static readonly DependencyProperty TabHeaderHorizontalAlignmentProperty = DependencyProperty.RegisterAttached(
            "TabHeaderHorizontalAlignment",
            typeof(HorizontalAlignment),
            typeof(TabControlAssist),
            new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.Inherits, TabHeaderHorizontalAlignmentPropertyChangedHandler)
        );

        /// <summary>
        /// Gets the alignment of the horizontal tab headers in the <code>TabControl</code>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static HorizontalAlignment GetTabHeaderHorizontalAlignment(DependencyObject element)
        {
            return (HorizontalAlignment)element.GetValue(TabHeaderHorizontalAlignmentProperty);
        }

        /// <summary>
        /// Sets the alignment of the horizontal tab headers in the <code>TabControl</code>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTabHeaderHorizontalAlignment(DependencyObject element, HorizontalAlignment value)
        {
            element.SetValue(TabHeaderHorizontalAlignmentProperty, value);
        }

        private static void TabHeaderHorizontalAlignmentPropertyChangedHandler(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            // hack to provide compatibility in the time before the breaking change of removing TabHeaderAlignmentProperty will be released
            if ((HorizontalAlignment)args.NewValue != GetTabHeaderAlignment(element))
            {
                SetTabHeaderAlignment(element, (HorizontalAlignment)args.NewValue);
            }
        }

        /// <summary>
        /// The alignment of the vertical tab headers in the <code>TabControl</code>.
        /// </summary>
        public static readonly DependencyProperty TabHeaderVerticalAlignmentProperty = DependencyProperty.RegisterAttached(
            "TabHeaderVerticalAlignment",
            typeof(VerticalAlignment),
            typeof(TabControlAssist),
            new FrameworkPropertyMetadata(VerticalAlignment.Top, FrameworkPropertyMetadataOptions.Inherits, null)
        );

        /// <summary>
        /// Gets the alignment of the vertical tab headers in the <code>TabControl</code>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static VerticalAlignment GetTabHeaderVerticalAlignment(DependencyObject element)
        {
            return (VerticalAlignment)element.GetValue(TabHeaderVerticalAlignmentProperty);
        }

        /// <summary>
        /// Sets the alignment of the vertical tab headers in the <code>TabControl</code>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTabHeaderVerticalAlignment(DependencyObject element, VerticalAlignment value)
        {
            element.SetValue(TabHeaderVerticalAlignmentProperty, value);
        }

        /// <summary>
        /// The brush for not selected tab headers.
        /// </summary>
        public static readonly DependencyProperty TabHeaderInactiveBrushProperty = DependencyProperty.RegisterAttached(
            "TabHeaderInactiveBrush",
            typeof(Brush),
            typeof(TabControlAssist),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, null)
        );

        /// <summary>
        /// Gets the brush for not selected tab headers.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetTabHeaderInactiveBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(TabHeaderInactiveBrushProperty);
        }

        /// <summary>
        /// Sets the brush for not selected tab headers.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTabHeaderInactiveBrush(DependencyObject element, Brush value)
        {
            element.SetValue(TabHeaderInactiveBrushProperty, value);
        }

        /// <summary>
        /// The opacity for not selected tab headers.
        /// </summary>
        public static readonly DependencyProperty TabHeaderInactiveOpacityProperty = DependencyProperty.RegisterAttached(
            "TabHeaderInactiveOpacity",
            typeof(double),
            typeof(TabControlAssist),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.Inherits, null)
        );

        /// <summary>
        /// Gets the opacity for not selected tab headers.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static double GetTabHeaderInactiveOpacity(DependencyObject element)
        {
            return (double)element.GetValue(TabHeaderInactiveOpacityProperty);
        }

        /// <summary>
        /// Sets the ppacity for not selected tab headers.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTabHeaderInactiveOpacity(DependencyObject element, double value)
        {
            element.SetValue(TabHeaderInactiveOpacityProperty, value);
        }

        /// <summary>
        /// The highlight color of the selected tab item header.
        /// </summary>
        public static readonly DependencyProperty TabHeaderHighlightBrushProperty = DependencyProperty.RegisterAttached(
            "TabHeaderHighlightBrush",
            typeof(Brush),
            typeof(TabControlAssist),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, null)
        );

        /// <summary>
        /// Gets the highlight color of the selected tab item header.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetTabHeaderHighlightBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(TabHeaderHighlightBrushProperty);
        }

        /// <summary>
        /// Sets the highlight color of the selected tab item header.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTabHeaderHighlightBrush(DependencyObject element, Brush value)
        {
            element.SetValue(TabHeaderHighlightBrushProperty, value);
        }

        /// <summary>
        /// The current color of the tab item header. Intended to be read-only.
        /// </summary>
        public static readonly DependencyProperty TabHeaderForegroundProperty = DependencyProperty.RegisterAttached(
            "TabHeaderForeground",
            typeof(Brush),
            typeof(TabControlAssist),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, null)
        );

        /// <summary>
        /// Gets the current color of the tab item header.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetTabHeaderForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(TabHeaderForegroundProperty);
        }

        /// <summary>
        /// Sets the current color of the tab item header.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTabHeaderForeground(DependencyObject element, Brush value)
        {
            element.SetValue(TabHeaderForegroundProperty, value);
        }
    }
}
