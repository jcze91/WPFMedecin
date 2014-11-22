using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.Services;
using mouham_cWpfMedecin.ServiceUser;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersViewModel : ModernViewModelBase
    {
        /// <summary>
        /// navigation service
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// user service
        /// </summary>
        private IServiceUser _serviceUser;

        private ObservableCollection<User> _users;
        /// <summary>
        /// collection of user
        /// </summary>
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { Set(ref _users, value, "Users"); }
        }

        private User _selectedUser;
        /// <summary>
        /// selected user
        /// </summary>
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { Set(ref _selectedUser, value, "SelectedUser"); }
        }

        /// <summary>
        /// command to add user
        /// </summary>
        public ICommand AddUserCommand { get; private set; }

        /// <summary>
        /// command to delete user
        /// </summary>
        public ICommand DeleteUserCommand { get; private set; }

        /// <summary>
        /// constructor os users viewmodel
        /// </summary>
        /// <param name="modernNavigationService">navigation service</param>
        /// <param name="sessionService">session service</param>
        /// <param name="serviceUser">user service</param>
        public UsersViewModel(IModernNavigationService modernNavigationService, ISessionService sessionService, IServiceUser serviceUser)
        {
            this.Role = sessionService.Role;

            _modernNavigationService = modernNavigationService;
            _serviceUser = serviceUser;

            LoadedCommand = new RelayCommand(LoadData);
            AddUserCommand = new RelayCommand(AddUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
        }

        /// <summary>
        /// add user logic
        /// </summary>
        void AddUser()
        {
            _modernNavigationService.NavigateTo(ViewModelLocator.AddUserPageKey);
        }

        /// <summary>
        /// delete user logic
        /// </summary>
        async void DeleteUser()
        {
            if (SelectedUser != null)
            {
                var dialog = new ModernDialog
                {
                    Title = "Supprimer utilisateur",
                    Content = String.Format("Voulez-vous supprimer l'utilisateur {0} {1} ?", SelectedUser.Name, SelectedUser.Firstname)
                };

                Button cancel = dialog.CancelButton;
                cancel.Content = "Annuler";
                Button yes = dialog.YesButton;
                yes.Content = "Oui";
                dialog.Buttons = new Button[] { cancel, yes };
                dialog.ShowDialog();

                if (dialog.MessageBoxResult == MessageBoxResult.Yes && await _serviceUser.DeleteUserAsync(SelectedUser.Login))
                    Users.Remove(SelectedUser);
            }
        }

        /// <summary>
        /// load data logic
        /// </summary>
        async void LoadData()
        {
            try
            {
                Users = new ObservableCollection<User>(await _serviceUser.GetListUserAsync());
            }
            catch { }
        }
    }
}
