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
        /// The alignment of the tab headers in the <code>TabControl</code>.
        /// </summary>
        public static readonly DependencyProperty TabHeaderAlignmentProperty = DependencyProperty.RegisterAttached(
            "TabHeaderAlignment",
            typeof(HorizontalAlignment),
            typeof(TabControlAssist),
            new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.Inherits, null)
        );

        /// <summary>
        /// Gets the alignment of the tab headers in the <code>TabControl</code>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static HorizontalAlignment GetTabHeaderAlignment(DependencyObject element)
        {
            return (HorizontalAlignment)element.GetValue(TabHeaderAlignmentProperty);
        }

        /// <summary>
        /// Sets the alignment of the tab headers in the <code>TabControl</code>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTabHeaderAlignment(DependencyObject element, HorizontalAlignment value)
        {
            element.SetValue(TabHeaderAlignmentProperty, value);
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
    }
}
