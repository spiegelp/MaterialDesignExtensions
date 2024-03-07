using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// Base class for a control with a text box and a button to open a file system dialog.
    /// </summary>
    public abstract class TextBoxFileSystemPath : Control
    {
        protected const string ShowDialogButtonName = "showButtonDialog";

        /// <summary>
        /// The dialog host ot show the dialog.
        /// </summary>
        public static readonly DependencyProperty DialogHostProperty = DependencyProperty.Register(
            nameof(DialogHost), typeof(DialogHost), typeof(TextBoxFileSystemPath));

        /// <summary>
        /// The dialog host ot show the dialog.
        /// </summary>
        public DialogHost DialogHost
        {
            get
            {
                return (DialogHost)GetValue(DialogHostProperty);
            }

            set
            {
                SetValue(DialogHostProperty, value);
            }
        }

        /// <summary>
        /// The name of the dialog host to show the dialog.
        /// </summary>
        public static readonly DependencyProperty DialogHostNameProperty = DependencyProperty.Register(
            nameof(DialogHostName), typeof(string), typeof(TextBoxFileSystemPath));

        /// <summary>
        /// The name of the dialog host to show the dialog.
        /// </summary>
        public string DialogHostName
        {
            get
            {
                return (string)GetValue(DialogHostNameProperty);
            }

            set
            {
                SetValue(DialogHostNameProperty, value);
            }
        }

        /// <summary>
        /// The <see cref="Style" /> for the <see cref="TextBox" /> inside the template.
        /// </summary>
        public static readonly DependencyProperty TextBoxStyleProperty = DependencyProperty.Register(
            nameof(TextBoxStyle), typeof(Style), typeof(TextBoxFileSystemPath));

        /// <summary>
        /// The <see cref="Style" /> for the <see cref="TextBox" /> inside the template.
        /// </summary>
        public Style TextBoxStyle
        {
            get
            {
                return (Style)GetValue(TextBoxStyleProperty);
            }

            set
            {
                SetValue(TextBoxStyleProperty, value);
            }
        }

        protected Button m_showButtonDialog;

        static TextBoxFileSystemPath()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxFileSystemPath), new FrameworkPropertyMetadata(typeof(TextBoxFileSystemPath)));
        }

        /// <summary>
        /// Creates a new <see cref="TextBoxFileSystemPath" />.
        /// </summary>
        public TextBoxFileSystemPath()
            : base()
        {
            m_showButtonDialog = null;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (m_showButtonDialog != null)
            {
                m_showButtonDialog.Click -= ShowDialogButtonClickHandler;
            }

            m_showButtonDialog = Template.FindName(ShowDialogButtonName, this) as Button;
            m_showButtonDialog.Click += ShowDialogButtonClickHandler;
        }

        private async void ShowDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            await ShowDialogAsync();
        }

        /// <summary>
        /// Shows the according dialog for the control.
        /// </summary>
        /// <returns></returns>
        protected abstract Task ShowDialogAsync();
    }
}
