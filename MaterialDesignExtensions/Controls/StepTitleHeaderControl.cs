using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MaterialDesignExtensions.Controls
{
    public class StepTitleHeaderControl : Control
    {
        /// <summary>
        /// The text of the first level title.
        /// A value of null will hide this title.
        /// </summary>
        public static readonly DependencyProperty FirstLevelTitleProperty = DependencyProperty.Register(
            nameof(FirstLevelTitle), typeof(string), typeof(StepTitleHeaderControl), new PropertyMetadata(null, null));

        /// <summary>
        /// The text of the first level title.
        /// A value of null will hide this title.
        /// </summary>
        public string FirstLevelTitle
        {
            get
            {
                return (string)GetValue(FirstLevelTitleProperty);
            }

            set
            {
                SetValue(FirstLevelTitleProperty, value);
            }
        }


        /// <summary>
        /// The text of the second level title.
        /// A value of null will hide this title.
        /// </summary>
        public static readonly DependencyProperty SecondLevelTitleProperty = DependencyProperty.Register(
            nameof(SecondLevelTitle), typeof(string), typeof(StepTitleHeaderControl), new PropertyMetadata(null, null));

        /// <summary>
        /// The text of the second level title.
        /// A value of null will hide this title.
        /// </summary>
        public string SecondLevelTitle
        {
            get
            {
                return (string)GetValue(SecondLevelTitleProperty);
            }

            set
            {
                SetValue(SecondLevelTitleProperty, value);
            }
        }

        /// <summary>
        /// Creates a new <see cref="StepTitleHeaderControl" />.
        /// </summary>
        public StepTitleHeaderControl() : base() { }
    }
}
