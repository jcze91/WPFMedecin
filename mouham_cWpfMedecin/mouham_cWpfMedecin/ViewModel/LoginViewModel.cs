using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.Services;
using mouham_cWpfMedecin.ServiceUser;
using mouham_cWpfMedecin.View;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        private ServiceUserClient _serviceUserClient;

        /// <summary>
        /// 
        /// </summary>
        private ISessionService _sessionService;

        private string _login;
        /// <summary>
        /// 
        /// </summary>
        public string Login
        {
            get { return _login; }
            set { Set(ref _login, value, "Login"); }
        }

        private bool _isConnecting;
        /// <summary>
        /// 
        /// </summary>
        public bool IsConnecting
        {
            get { return _isConnecting; }
            set { Set(ref _isConnecting, value, "IsConnecting"); }
        }

        private string _password;
        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value, "Password"); }
        }

        private bool _closeTrigger;
        /// <summary>
        /// 
        /// </summary>
        public bool CloseTrigger
        {
            get { return _closeTrigger; }
            set { Set(ref _closeTrigger, value, "CloseTrigger"); }
        }

        private string _errorText;
        /// <summary>
        /// 
        /// </summary>
        public string ErrorText
        {
            get { return _errorText; }
            set { Set(ref _errorText, value, "ErrorText"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionService"></param>
        public LoginViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
            Login = "";
            ErrorText = "";
            _serviceUserClient = new ServiceUserClient();
            IsConnecting = false;

            LoginCommand = new RelayCommand(LoginExecute, CanLoginExecute);
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanLoginExecute()
        {
            return !IsConnecting;
        }
    }
}
