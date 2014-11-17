using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mouham_cWpfMedecin.View;
using mouham_cWpfMedecin.UserServiceReference;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace mouham_cWpfMedecin.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _login;
        private bool _isConnecting;
        private bool _closeTrigger;
        private string _errorText;
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
        public bool IsLoginButtonVisible
        {
            get { return !_isConnecting; }
        }
        public bool IsProgressRingActive
        {
            get { return _isConnecting; }
        }
        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                if (_errorText != value)
                {
                    _errorText = value;
                    RaisePropertyChanged("ErrorText");
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
            this.ErrorText = "";
            _serviceUserClient = new ServiceUserClient();
            _isConnecting = false;

            LoginCommand = new RelayCommand<Object>(async c => 
                {
                    this.ErrorText = "";
                    _isConnecting = true;
                    RaisePropertyChanged("IsLoginButtonVisible");
                    RaisePropertyChanged("IsProgressRingActive");

                    var passwordBox = c as PasswordBox;
                    var password = passwordBox.Password;
                 
                    if (await _serviceUserClient.ConnectAsync(Login, password))
                    {
                        PortalView view = new PortalView();
                        PortalViewModel viewModel = new PortalViewModel();
                        view.DataContext = viewModel;
                        view.Show();
                        this.CloseTrigger = true;
                    }
                    else
                    {
                        _isConnecting = false;
                        this.ErrorText = "Error. Retry to connect...";
                        RaisePropertyChanged("IsLoginButtonVisible");
                        RaisePropertyChanged("IsProgressRingActive");
                    }
                }, c => true);
        }
    }
}
