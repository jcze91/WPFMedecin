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
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    public class UsersViewModel : ModernViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;
        private ObservableCollection<User> _users;
        private ServiceUserClient _serviceUserClient;

        public ObservableCollection<User> Users
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

        public UsersViewModel(IModernNavigationService modernNavigationService, ISessionService sessionService)
        {
            try
            {
                this.Role = sessionService.Role;
                _modernNavigationService = modernNavigationService;
                LoadedCommand = new RelayCommand(LoadData);
                AddUserCommand = new RelayCommand(() =>
                    {
                        _modernNavigationService.NavigateTo(ViewModelLocator.AddUserPageKey);
                    });

                _serviceUserClient = new ServiceUserClient();
            }
            catch { }
        }

        private async void LoadData()
        {
            try
            {
                Users = new ObservableCollection<User>(await _serviceUserClient.GetListUserAsync());
            }
            catch { }
        }
    }
}
