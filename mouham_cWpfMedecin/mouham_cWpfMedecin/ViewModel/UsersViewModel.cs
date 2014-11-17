using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mouham_cWpfMedecin.ViewModel
{
    public class UsersViewModel : ModernViewModelBase
    {
        private ObservableCollection<UserServiceReference.User> _users;
        private UserServiceReference.ServiceUserClient _serviceUserClient;

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

        public UsersViewModel()
        {
            LoadedCommand = new RelayCommand(LoadData);
            _serviceUserClient = new UserServiceReference.ServiceUserClient();
        }

        private async void LoadData()
        {
            Users = new ObservableCollection<UserServiceReference.User>(await _serviceUserClient.GetListUserAsync());
        }

    }
}
