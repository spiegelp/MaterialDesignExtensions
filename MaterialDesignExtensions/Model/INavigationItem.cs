using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public interface INavigationItem : INotifyPropertyChanged
    {
        bool IsSelectable { get; set; }

        bool IsSelected { get; set; }

        NavigationItemSelectedCallback NavigationItemSelectedCallback { get; set; }
    }

    public delegate object NavigationItemSelectedCallback(INavigationItem navigationItem);
}
