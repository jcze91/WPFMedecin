using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    public class UsersViewModel : ModernViewModelBase
    {
        private ObservableCollection<UserServiceReference.User> _users;
        private UserServiceReference.ServiceUserClient _serviceUserClient;
        private readonly IModernNavigationService _modernNavigationService;

        public ObservableCollection<UserServiceReference.User> Users
        {
            get { return _users; }
            set
            {
                if (_users != value)
                {
                    _users = value;
                    RaisePropertyChanged("Users");
                }
            }
        }

        public ICommand AddUserCommand { get; set; }

        public UsersViewModel(IModernNavigationService modernNavigationService)
        {
            try
            {
                _modernNavigationService = modernNavigationService;
                LoadedCommand = new RelayCommand(LoadData);
                _serviceUserClient = new UserServiceReference.ServiceUserClient();
                AddUserCommand = new RelayCommand(() =>
                    {
                        _modernNavigationService.NavigateTo(ViewModelLocator.AddUserPageKey);
                    });
            }
            catch { }
        }

        private async void LoadData()
        {
            try
            {
                Users = new ObservableCollection<UserServiceReference.User>(await _serviceUserClient.GetListUserAsync());
            }
            catch { }
        }
    }
}
