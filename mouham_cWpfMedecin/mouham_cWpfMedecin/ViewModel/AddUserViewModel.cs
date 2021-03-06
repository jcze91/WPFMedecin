﻿using GalaSoft.MvvmLight.Command;
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
    /// View model for User insertion view
    /// </summary>
    public class AddUserViewModel : ModernViewModelBase
    {
        /// <summary>
        /// Navigation service
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// User service WCF
        /// </summary>
        private IServiceUser _serviceUser;

        private string _login;
        /// <summary>
        /// User login
        /// </summary>
        public string Login
        {
            get { return _login; }
            set { Set(ref _login, value, "Login"); }
        }

        private string _pwd;
        /// <summary>
        /// User password
        /// </summary>
        public string Pwd
        {
            get { return _pwd; }
            set { Set(ref _pwd, value, "Pwd"); }
        }

        private string _name;
        /// <summary>
        /// User name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value, "Name"); }
        }

        private string _firstname;
        /// <summary>
        /// User firstname
        /// </summary>
        public string Firstname
        {
            get { return _firstname; }
            set { Set(ref _firstname, value, "Firstname"); }
        }

        private Byte[] _picture;
        /// <summary>
        /// User picture
        /// </summary>
        public Byte[] Picture
        {
            get { return _picture; }
            set { Set(ref _picture, value, "Picture"); }
        }

        private string _pictureFilename;
        /// <summary>
        /// Picture filename
        /// </summary>
        public string PictureFilename
        {
            get { return _pictureFilename; }
            set { Set(ref _pictureFilename, value, "PictureFilename"); }
        }

        private string _role;
        /// <summary>
        /// User role
        /// </summary>
        public string Role
        {
            get { return _role; }
            set { Set(ref _role, value, "Role"); }
        }



        private readonly ICommand _comfirmCommand;
        /// <summary>
        /// Command to add user
        /// </summary>
        public ICommand ComfirmCommand { get { return _comfirmCommand; } }
        
        private readonly ICommand _browseCommand;
        /// <summary>
        /// Command to search an file for Picture
        /// </summary>
        public ICommand BrowseCommand { get { return _browseCommand; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modernNavigationService"></param>
        public AddUserViewModel(IModernNavigationService modernNavigationService, IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
            _modernNavigationService = modernNavigationService;

            _comfirmCommand = new RelayCommand(AddUser);
            _browseCommand = new RelayCommand(SelectFile);
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

        /// <summary>
        /// File picker method
        /// </summary>
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

                _picture = System.IO.File.ReadAllBytes(_pictureFilename);
            }
        }

        /// <summary>
        /// Add user method
        /// </summary>
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
                result = await _serviceUser.AddUserAsync(user);
            }
            catch { }

            _modernNavigationService.NavigateTo(ViewModelLocator.UsersPageKey);
        }
    }
}