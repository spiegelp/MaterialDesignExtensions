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
    public partial class TabControlControl : UserControl
    {
        public TabControlControl()
        {
            InitializeComponent();
        }

        private void RemoveTabsButtonClickHandler(object sender, RoutedEventArgs args)
        {
            foreach (object item in m_tabControl.Items)
            {
                ((TabItem)item).Template = null;
            }

            m_tabControl.Items.Clear();
        }
    }
}
