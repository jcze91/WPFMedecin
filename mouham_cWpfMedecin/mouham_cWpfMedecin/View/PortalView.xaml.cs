﻿using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mouham_cWpfMedecin.View
{
    /// <summary>
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    public partial class PortalView
    {
        public PortalView()
        {
            InitializeComponent();
            AppearanceManager.Current.AccentColor = Colors.DodgerBlue;
            ContentSource = MenuLinkGroups.First().Links.First().Source;
        }
    }
}
