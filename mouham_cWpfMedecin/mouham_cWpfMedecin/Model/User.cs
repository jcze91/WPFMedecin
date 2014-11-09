using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mouham_cWpfMedecin.Model
{
   public class User
   {
       #region variables
       private string _name;      
       private string _pwd;      
       #endregion

       #region getter/ setter
       /// <summary>
       /// login du user
       /// </summary>
       public string Name
       {
           get { return _name; }
           set { _name = value; }
       }

       /// <summary>
       /// mot de passe du user
       /// </summary>
       public string Pwd
       {
           get { return _pwd; }
           set { _pwd = value; }
       }
       #endregion
       
       /// <summary>
       /// crée un utilisateur
       /// </summary>
       /// <param name="name">nom du user</param>
       /// <param name="pwd">mot de passe du user</param>
       /// <returns>un user initialisé avec les paramétres</returns>
       public static User CreateUser(string name, string pwd)
       {
           User user = new User();
           user.Name = name;
           user.Pwd = pwd;
           return user;
       }
       
       /// <summary>
       /// constructeur
       /// </summary>
       public User()
       {
           _name = "";
           _pwd = "";
       }
   }
}
