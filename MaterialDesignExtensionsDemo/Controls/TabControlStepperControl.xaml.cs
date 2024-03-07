﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class TabControlStepperControl : UserControl
    {
        public TabControlStepperControl()
        {
            InitializeComponent();

            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            // demo for setting a step via SelectedIndex property
            //m_stepper.SelectedIndex = 1;
        }
    }
}
