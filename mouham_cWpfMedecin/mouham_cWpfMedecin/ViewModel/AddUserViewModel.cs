using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mouham_cWpfMedecin.View;
using System.ComponentModel;
using mouham_cWpfMedecin.ServiceUser;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using mouham_cWpfMedecin.Services;
using System.Windows;
using System.IO;

namespace mouham_cWpfMedecin.ViewModel
{
    public class AddUserViewModel : ModernViewModelBase
    {
        private ServiceUserClient _userService;
        private Byte[] _picture;

        private string _login;
        public string Login
        {
            get { return _login; }
            set {
                if (_login != value)
                {
                    _login = value;
                    RaisePropertyChanged("Login");
                }
            }
        }

        private string _pwd;
        public string Pwd
        {
            get { return _pwd; }
            set {
                if (_pwd != value)
                {
                    _pwd = value;
                    RaisePropertyChanged("Pwd");
                }
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        private string _firstname;
        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (_firstname != value)
                {
                    _firstname = value;
                    RaisePropertyChanged("Firstname");
                }
            }
        }

        private string _pictureFilename;
        public string PictureFilename
        {
            get { return _pictureFilename; }
            set {
                if (_pictureFilename != value)
                {
                    _pictureFilename = value;
                    RaisePropertyChanged("PictureFilename");
                }    
            }
        }

        private string _role;
        public string Role
        {
            get { return _role; }
            set {
                if (_role != value)
                {
                    _role = value;
                    RaisePropertyChanged("Role");
                }
            }
        }

        public ICommand ComfirmCommand { get; set; }
        public ICommand BrowseCommand { get; set; }
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// Initializes a new instance of the AddUserViewModel class.
        /// </summary>
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
                //Picture = System.IO.File.ReadAllBytes(_pictureFilename);
                StreamReader sr = new StreamReader(PictureFilename);
                BinaryReader read = new BinaryReader(sr.BaseStream);
                _picture = read.ReadBytes((int)sr.BaseStream.Length);
                read.Close();
                sr.Close();
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