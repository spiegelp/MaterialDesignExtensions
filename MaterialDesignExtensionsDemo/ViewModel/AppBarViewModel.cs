using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class AppBarViewModel : ViewModel
    {
        public override string DocumentationUrl
        {
            get
            {
                return "https://github.com/spiegelp/MaterialDesignExtensions/wiki/App-bar";
            }
        }

        public AppBarViewModel() : base() { }
    }
}
