using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MaterialDesignExtensions.Controls
{
    public class PersistentSearch : SearchBase
    {
        static PersistentSearch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PersistentSearch), new FrameworkPropertyMetadata(typeof(PersistentSearch)));
        }

        public PersistentSearch() : base() { }
    }
}
