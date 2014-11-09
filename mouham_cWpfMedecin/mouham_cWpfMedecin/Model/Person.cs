using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mouham_cWpfMedecin.Model
{
   public class Person
   {
       #region variables
       private string _name;
       private string _firstname;
       private string _age;
       #endregion

       #region getter / setter
       /// <summary>
       /// nom de la personne
       /// </summary>
       public string Name
       {
           get { return _name; }
           set { _name = value; }
       }
       /// <summary>
       /// prénom de la personne
       /// </summary>
       public string Firstname
       {
           get { return _firstname; }
           set { _firstname = value; }
       }
       /// <summary>
       /// age de la personne
       /// </summary>
       public string Age
       {
           get { return _age; }
           set { _age = value; }
       }
       #endregion

       /// <summary>
       /// créer une peronne
       /// </summary>
       /// <param name="name">nom de la personne</param>
       /// <param name="firstname">prénom de la personne</param>
       /// <param name="age">age de la personne</param>
       /// <returns>une personne initialisé avec les paramètres</returns>
       public static Person CreatePerson(string name, string firstname, string age)
       {
           Person person = new Person();
           person.Name = name;
           person.Firstname = firstname;
           person.Age = age;
           return person;
       }

       /// <summary>
       /// constructeur
       /// </summary>
       public Person()
       {
           _name = "";
           _firstname = "";
           _age = "";
       }
   }
}
