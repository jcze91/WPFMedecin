using mouham_cWpfMedecin.ServiceUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mouham_cWpfMedecin.Services
{
    public class SessionService : ISessionService
    {
        private ServiceUserClient serviceUserClient;

        public string Login { get; private set; }
        public string Role { get; private set; }

        public void RegisterSession(string login)
        {
            this.Login = login;
            serviceUserClient = new ServiceUserClient();
        }

        public async Task FetchUserRole()
        {
            if (serviceUserClient != null) 
                this.Role = await serviceUserClient.GetRoleAsync(this.Login);
        }
    }
}
