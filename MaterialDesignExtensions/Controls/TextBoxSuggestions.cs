using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

using MaterialDesignExtensions.Commands.Internal;
using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A decorator control to add some kind of autocomplete features to a default TextBox.
    /// </summary>
    [ContentProperty(nameof(TextBox))]
    public class TextBoxSuggestions : ControlWithAutocompletePopup
    {
        private static readonly string SuggestionItemsControlName = "suggestionItemsControl";
        private static readonly string SuggestionItemsPopupName = "suggestionItemsPopup";

        /// <summary>
        /// True to keep the focus on the text box after selecting a suggestion.
        /// </summary>
        public static readonly DependencyProperty KeepFocusOnSelectionProperty = DependencyProperty.Register(
            nameof(KeepFocusOnSelection), typeof(bool), typeof(TextBoxSuggestions), new PropertyMetadata(false));

        /// <summary>
        /// True to keep the focus on the text box after selecting a suggestion.
        /// </summary>
        public bool KeepFocusOnSelection
        {
            get
            {
                return (bool)GetValue(KeepFocusOnSelectionProperty);
            }

            set
            {
                SetValue(KeepFocusOnSelectionProperty, value);
            }
        }

        /// <summary>
        /// The TextBox to decorate.
        /// </summary>
        public static readonly DependencyProperty TextBoxProperty = DependencyProperty.Register(
            nameof(TextBox), typeof(TextBox), typeof(TextBoxSuggestions), new PropertyMetadata(null, TextBoxChangedHandler));

        /// <summary>
        /// The TextBox to decorate.
        /// </summary>
        public TextBox TextBox
        {
            get
            {
                return (TextBox)GetValue(TextBoxProperty);
            }

            set
            {
                SetValue(TextBoxProperty, value);
            }
        }

        /// <summary>
        /// A source for providing the suggestions.
        /// </summary>
        public static readonly DependencyProperty TextBoxSuggestionsSourceProperty = DependencyProperty.Register(
            nameof(TextBoxSuggestionsSource), typeof(ITextBoxSuggestionsSource), typeof(TextBoxSuggestions), new PropertyMetadata(null, TextBoxSuggestionsSourceChangedHandler));

        /// <summary>
        /// A source for providing the suggestions.
        /// </summary>
        public ITextBoxSuggestionsSource TextBoxSuggestionsSource
        {
            get
            {
                return (ITextBoxSuggestionsSource)GetValue(TextBoxSuggestionsSourceProperty);
            }

            set
            {
                SetValue(TextBoxSuggestionsSourceProperty, value);
            }
        }

        private ItemsControl m_suggestionItemsControl;

        private AutocompleteController m_autocompleteController;

        static TextBoxSuggestions()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxSuggestions), new FrameworkPropertyMetadata(typeof(TextBoxSuggestions)));
        }

        /// <summary>
        /// Creates a new <see cref="TextBoxSuggestions" />.
        /// </summary>
        public TextBoxSuggestions()
            : base()
        {
            m_suggestionItemsControl = null;

            m_autocompleteController = new AutocompleteController() { AutocompleteSource = TextBoxSuggestionsSource };

            CommandBindings.Add(new CommandBinding(TextBoxSuggestionsCommands.SelectSuggestionItemCommand, SelectSuggestionItemCommandHandler));

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_popup = Template.FindName(SuggestionItemsPopupName, this) as AutocompletePopup;

            m_suggestionItemsControl = Template.FindName(SuggestionItemsControlName, this) as ItemsControl;
        }

        protected override void LoadedHandler(object sender, RoutedEventArgs args)
        {
            base.LoadedHandler(sender, args);

            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteItemsChanged += AutocompleteItemsChangedHandler;
            }

            if (TextBox != null)
            {
                // first remove the event handler to prevent multiple registrations
                TextBox.TextChanged -= TextBoxTextChangedHandler;
                TextBox.KeyUp -= TextBoxKeyUpHandler;

                // and then set the event handler
                TextBox.TextChanged += TextBoxTextChangedHandler;
                TextBox.KeyUp += TextBoxKeyUpHandler;
            }
        }

        protected override void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            base.UnloadedHandler(sender, args);

            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteItemsChanged -= AutocompleteItemsChangedHandler;
            }

            if (TextBox != null)
            {
                TextBox.TextChanged -= TextBoxTextChangedHandler;
                TextBox.KeyUp -= TextBoxKeyUpHandler;
            }
        }

        private void SelectSuggestionItemCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (TextBox != null)
            {
                TextBox.Text = args.Parameter as string ?? string.Empty;

                if (KeepFocusOnSelection)
                {
                    Keyboard.Focus(TextBox);
                    TextBox.CaretIndex = TextBox.Text.Length;
                }
                else
                {
                    Keyboard.Focus(null);
                }
            }
        }

        private static void TextBoxChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as TextBoxSuggestions)?.TextBoxChangedHandler(args.OldValue as TextBox, args.NewValue as TextBox);
        }

        private void TextBoxChangedHandler(TextBox oldTextBox, TextBox newTextBox)
        {
            if (oldTextBox != null)
            {
                oldTextBox.TextChanged -= TextBoxTextChangedHandler;
                newTextBox.KeyUp -= TextBoxKeyUpHandler;
            }

            if (newTextBox != null)
            {
                newTextBox.TextChanged += TextBoxTextChangedHandler;
                newTextBox.KeyUp += TextBoxKeyUpHandler;
            }
        }

        private void TextBoxTextChangedHandler(object sender, TextChangedEventArgs args)
        {
            if (sender == TextBox && IsEnabled && IsLoaded && TextBox.IsLoaded && TextBox.IsFocused)
            {
                m_autocompleteController?.Search(TextBox.Text);
            }
        }

        private void TextBoxKeyUpHandler(object sender, KeyEventArgs args)
        {
            if (sender == TextBox && args.Key == Key.Down)
            {
                m_suggestionItemsControl.Focus();
            }
        }

        private static void TextBoxSuggestionsSourceChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as TextBoxSuggestions)?.TextBoxSuggestionsSourceChangedHandler(args.NewValue as ITextBoxSuggestionsSource);
        }

        private void TextBoxSuggestionsSourceChangedHandler(ITextBoxSuggestionsSource textBoxSuggestionsSource)
        {
            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteSource = textBoxSuggestionsSource;
            }
        }

        private void AutocompleteItemsChangedHandler(object sender, AutocompleteItemsChangedEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                SetSuggestionItems(args.Items);
            });
        }

        private void SetSuggestionItems(IEnumerable suggestionItems)
        {
            if (m_suggestionItemsControl != null)
            {
                if (suggestionItems != null)
                {
                    m_suggestionItemsControl.ItemsSource = suggestionItems;
                }
                else
                {
                    m_suggestionItemsControl.ItemsSource = null;
                }
            }
        }
    }
}
