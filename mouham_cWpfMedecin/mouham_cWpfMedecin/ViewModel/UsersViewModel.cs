using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.ServiceUser;
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

        public UsersViewModel()
        {
            try
            {
                LoadedCommand = new RelayCommand(LoadData);
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
