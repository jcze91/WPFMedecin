using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.Services;
using mouham_cWpfMedecin.ServiceUser;
using mouham_cWpfMedecin.View;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private ServiceUserClient _serviceUserClient;
        private ISessionService _sessionService;

        private string _login;
        public string Login
        {
            get { return _login; }
            set { Set(ref _login, value, "Login"); }
        }

        private bool _isConnecting;
        public bool IsConnecting
        {
            get { return _isConnecting; }
            set { Set(ref _isConnecting, value, "IsConnecting"); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value, "Password"); }
        }

        private bool _closeTrigger;
        public bool CloseTrigger
        {
            get { return _closeTrigger; }
            set { Set(ref _closeTrigger, value, "CloseTrigger"); }
        }

        private string _errorText;
        public string ErrorText
        {
            get { return _errorText; }
            set { Set(ref _errorText, value, "ErrorText"); }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
            Login = "";
            ErrorText = "";
            _serviceUserClient = new ServiceUserClient();
            IsConnecting = false;

            LoginCommand = new RelayCommand(LoginExecute, CanLoginExecute);
        }

        async private void LoginExecute()
        {
            ErrorText = "";
            IsConnecting = true;

            if (await _serviceUserClient.ConnectAsync(Login, Password))
            {

                _sessionService.RegisterSession(Login);
                await _sessionService.FetchUserRole();
                PortalView view = new PortalView();
                view.Show();
                this.CloseTrigger = true;
                IsConnecting = false;
            }
            else
            {
                ErrorText = "Erreur. Réessayez de vous connecter...";
                IsConnecting = false;
            }
        }

        private bool CanLoginExecute()
        {
            return !IsConnecting;
        }
    }
}
