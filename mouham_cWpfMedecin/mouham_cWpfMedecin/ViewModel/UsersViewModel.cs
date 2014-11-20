using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.Services;
using mouham_cWpfMedecin.ServiceUser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    public class UsersViewModel : ModernViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;
        private ServiceUserClient _serviceUserClient;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { Set(ref _users, value, "Users"); }
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { Set(ref _selectedUser, value, "SelectedUser"); }
        }

        public ICommand AddUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }

        public UsersViewModel(IModernNavigationService modernNavigationService, ISessionService sessionService)
        {
            try
            {
                this.Role = sessionService.Role;

                _modernNavigationService = modernNavigationService;
                _serviceUserClient = new ServiceUserClient();

                LoadedCommand = new RelayCommand(LoadData);
                AddUserCommand = new RelayCommand(AddUser);
                DeleteUserCommand = new RelayCommand(DeleteUser);

            }
            catch { }
        }

        void AddUser()
        {
            _modernNavigationService.NavigateTo(ViewModelLocator.AddUserPageKey);
        }

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

                if (dialog.MessageBoxResult == MessageBoxResult.Yes && await _serviceUserClient.DeleteUserAsync(SelectedUser.Login))
                    Users.Remove(SelectedUser);
            }
        }

        async void LoadData()
        {
            try
            {
                Users = new ObservableCollection<User>(await _serviceUserClient.GetListUserAsync());
            }
            catch { }
        }
    }
}
