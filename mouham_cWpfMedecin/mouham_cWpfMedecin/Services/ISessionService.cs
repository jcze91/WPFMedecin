using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mouham_cWpfMedecin.Services
{
    /// <summary>
    ///  Session service interface
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Gets the login
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        string Login { get; }
        /// <summary>
        /// Gets the user role
        /// </summary>
        /// <value>
        /// The user role
        /// </value>
        string Role { get; }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
       
        /// <summary>
        /// Register the session login
        /// </summary>
        /// <param name="login">The login</param>
        void RegisterSession(string login);

        /// <summary>
        /// Call service to set the role according to user login
        /// </summary>
        Task FetchUserRole();
    }
}
