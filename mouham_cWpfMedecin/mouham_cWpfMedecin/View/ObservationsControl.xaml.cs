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

namespace mouham_cWpfMedecin.View
{
    /// <summary>
    /// Interaction logic for ObservationsControl.xaml
    /// </summary>
    public partial class ObservationsControl : UserControl
    {
        public ObservationsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            observations.SelectedIndex = 0;
        }
    }
}
