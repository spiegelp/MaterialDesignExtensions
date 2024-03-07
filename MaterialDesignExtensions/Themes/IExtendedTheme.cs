﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Themes
{
    public interface IExtendedTheme : ITheme
    {
        Color NavigationItemIcon { get; set; }

        Color NavigationItemText { get; set; }

        Color NavigationItemSubheader { get; set; }

        Color StepperInactiveStep { get; set; }

        Color StepperActiveStep { get; set; }

        Color StepperSeparator { get; set; }
    }
}
