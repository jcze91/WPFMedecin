using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.Services;
using mouham_cWpfMedecin.ServiceUser;
using mouham_cWpfMedecin.View;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// View model for login view
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// User service WCF
        /// </summary>
        private IServiceUser _serviceUser;

        /// <summary>
        /// Session service
        /// </summary>
        private ISessionService _sessionService;

        private string _login;
        /// <summary>
        /// User login
        /// </summary>
        public string Login
        {
            get { return _login; }
            set { Set(ref _login, value, "Login"); }
        }

        private bool _isConnecting;
        /// <summary>
        /// Button on/off
        /// </summary>
        public bool IsConnecting
        {
            get { return _isConnecting; }
            set { Set(ref _isConnecting, value, "IsConnecting"); }
        }

        private string _password;
        /// <summary>
        /// User password
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value, "Password"); }
        }

        private bool _closeTrigger;
        /// <summary>
        /// Attribute for closing the window
        /// </summary>
        public bool CloseTrigger
        {
            get { return _closeTrigger; }
            set { Set(ref _closeTrigger, value, "CloseTrigger"); }
        }

        private string _errorText;
        /// <summary>
        /// Connection error text
        /// </summary>
        public string ErrorText
        {
            get { return _errorText; }
            set { Set(ref _errorText, value, "ErrorText"); }
        }

        /// <summary>
        /// Command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sessionService"></param>
        /// <param name="serviceUser"></param>
        public LoginViewModel(ISessionService sessionService, IServiceUser serviceUser)
        {
            _sessionService = sessionService;
            _serviceUser = serviceUser;

            Login = "";
            ErrorText = "";
            IsConnecting = false;

            LoginCommand = new RelayCommand(LoginExecute, CanLoginExecute);
        }

        /// <summary>
        /// Login method
        /// </summary>
        async private void LoginExecute()
        {
            ErrorText = "";
            IsConnecting = true;

            if (await _serviceUser.ConnectAsync(Login, Password))
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
        /// Button on/off method
        /// </summary>
        /// <returns></returns>
        private bool CanLoginExecute()
        {
            return !IsConnecting;
        }
    }
}
