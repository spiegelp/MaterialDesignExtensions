using System.Windows.Controls;
using MaterialDesignExtensions.Controls;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class SearchControl : UserControl
    {
        public SearchControl()
        {
            InitializeComponent();
        }

        private void SearchHandler1(object sender, SearchEventArgs args)
        {
            searchResultTextBlock1.Text = "Your are looking for '" + args.SearchTerm + "'.";
        }
    }
}
