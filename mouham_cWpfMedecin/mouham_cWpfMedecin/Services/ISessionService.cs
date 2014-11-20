using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mouham_cWpfMedecin.Services
{
    public interface ISessionService
    {
        string Login { get; }
        string Role { get; }

        void RegisterSession(string login);
        Task FetchUserRole();
    }
}
