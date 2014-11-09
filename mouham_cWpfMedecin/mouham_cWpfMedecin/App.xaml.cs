﻿using mouham_cWpfMedecin.View;
using mouham_cWpfMedecin.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace mouham_cWpfMedecin
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LoginView window = new LoginView();
            LoginViewModel vm = new LoginViewModel();
            window.DataContext = vm;
            window.Show();
        }
    }
}
