﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mouham_cWpfMedecin.View;
using System.ComponentModel;
using mouham_cWpfMedecin.UserServiceReference;
using System.Windows.Controls;

namespace mouham_cWpfMedecin.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _login;
        private string _password;
        private RelayCommand _loginCommand;
        private bool _closeTrigger;
        private BackgroundWorker _connectWorker;
        private ServiceUserClient _serviceUserClient;

        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged("Login");
                }

            }
        }

        public bool CloseTrigger
        {
            get { return _closeTrigger; }
            set
            {
                if (_closeTrigger != value)
                {
                    _closeTrigger = value;
                    OnPropertyChanged("CloseTrigger");
                }
            }
        }

        public RelayCommand LoginCommand
        {
            get { return _loginCommand; }
            set { _loginCommand = value; }
        }


        public LoginViewModel()
        {
            Init();
        }

        private void Init()
        {
            this.Login = "";
            _loginCommand = new RelayCommand(c => 
                {
                    var passwordBox = c as PasswordBox;
                    var password = passwordBox.Password;
                    _password = password;
                    _connectWorker.RunWorkerAsync();
                }, c => true);

            _serviceUserClient = new ServiceUserClient();
            _connectWorker = new BackgroundWorker();
            _connectWorker.DoWork += new DoWorkEventHandler((s, e) =>
            {
                try
                {
                    e.Result = _serviceUserClient.Connect(Login, _password);
                }
                catch (Exception ex)
                { }
            });
            _connectWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((s, e) =>
            {
                try
                {
                    bool success = (bool)e.Result;
                    if (success)
                    {
                        PortalView view = new PortalView();
                        PortalViewModel viewModel = new PortalViewModel();
                        view.DataContext = viewModel;
                        view.Show();
                        this.CloseTrigger = true;
                    }
                    else
                    {
                        // TODO gestion erreur
                    }
                }
                catch (Exception ex)
                { }
            });
        }
    }
}
