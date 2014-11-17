using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mouham_cWpfMedecin.View;
using System.ComponentModel;
using mouham_cWpfMedecin.UserServiceReference;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace mouham_cWpfMedecin.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _login;
        private string _password;
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
                    RaisePropertyChanged("Login");
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
                    RaisePropertyChanged("CloseTrigger");
                }
            }
        }

        public ICommand LoginCommand { get;  set; }

        public LoginViewModel()
        {
            Init();
        }

        private void Init()
        {
            this.Login = "";
            LoginCommand = new RelayCommand<Object>(c => 
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
