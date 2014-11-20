using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.Services;
using mouham_cWpfMedecin.ServiceUser;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class AddUserViewModel : ModernViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// 
        /// </summary>
        private IServiceUser _userService;

        /// <summary>
        /// 
        /// </summary>
        private BackgroundWorker _connectWorker;

        private string _login;
        /// <summary>
        /// 
        /// </summary>
        public string Login
        {
            get { return _login; }
            set { Set(ref _login, value, "Login"); }
                }

        private string _pwd;
        /// <summary>
        /// 
        /// </summary>
        public string Pwd
        {
            get { return _pwd; }
            set { Set(ref _pwd, value, "Pwd"); }
                }

        private string _name;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value, "Name"); }
        }

        private string _firstname;
        /// <summary>
        /// 
        /// </summary>
        public string Firstname
        {
            get { return _firstname; }
            set { Set(ref _firstname, value, "Firstname"); }
        }

        private Byte[] _picture;
        /// <summary>
        /// 
        /// </summary>
        public Byte[] Picture
        {
            get { return _picture; }
            set { Set(ref _picture, value, "Picture"); }
        }

        private string _pictureFilename;
        /// <summary>
        /// 
        /// </summary>
        public string PictureFilename
        {
            get { return _pictureFilename; }
            set { Set(ref _pictureFilename, value, "PictureFilename"); }
        }

        private string _role;
        /// <summary>
        /// 
        /// </summary>
        public string Role
        {
            get { return _role; }
            set { Set(ref _role, value, "Role"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ComfirmCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand BrowseCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the AddUserViewModel class.
        /// </summary>
        /// <param name="modernNavigationService"></param>
        public AddUserViewModel(IModernNavigationService modernNavigationService)
        {
            _userService = new ServiceUserClient();
            _modernNavigationService = modernNavigationService;

            ComfirmCommand = new RelayCommand(() => AddUser());
            BrowseCommand = new RelayCommand(() => SelectFile());
            LoadedCommand = new RelayCommand(LoadData);
        }
        private void LoadData()
        {
            this.Role = "";
            this.Firstname = "";
            this.Name = "";
            this.Login = "";
            this.Pwd = "";
            this.PictureFilename = "";
        }

        private void SelectFile()
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Images (.jpg)|*.jpg";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                PictureFilename = dlg.FileName;
                Picture = System.IO.File.ReadAllBytes(_pictureFilename);
            }
        }

        private async void AddUser()
        {
            User user = new User();
            bool result = false;

            user.Login = _login;
            user.Pwd = _pwd;
            user.Name = _name;
            user.Firstname = _firstname;
            user.Picture = _picture;
            user.Role = _role;
            user.Connected = false;

            try
            {
               result = await _userService.AddUserAsync(user);
            }
            catch { }

            _modernNavigationService.NavigateTo(ViewModelLocator.UsersPageKey);
        }
    }
}