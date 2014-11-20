/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:mouham_cWpfMedecin"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using mouham_cWpfMedecin.Services;
using System;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// The resource page key.
        /// </summary>
        public const string ObservationsPageKey = "ObservationsView";
        public const string AddUserPageKey = "AddUserView";
        public const string AddPatientPageKey = "AddPatientView";
        public const string UserPageKey = "UserView";

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<PortalViewModel>();
            SimpleIoc.Default.Register<PatientsViewModel>();
            SimpleIoc.Default.Register<UsersViewModel>();
            SimpleIoc.Default.Register<AddUserViewModel>();
            SimpleIoc.Default.Register<AddPatientViewModel>();
            SimpleIoc.Default.Register<ObservationsViewModel>();

            var navigationService = new ModernNavigationService();
            var sessionService = new SessionService();
            navigationService.Configure(ViewModelLocator.ObservationsPageKey, new Uri("View/ObservationsControl.xaml", UriKind.Relative));
            navigationService.Configure(ViewModelLocator.AddUserPageKey, new Uri("View/AddUserView.xaml", UriKind.Relative));
            navigationService.Configure(ViewModelLocator.AddPatientPageKey, new Uri("View/AddPatientView.xaml", UriKind.Relative));
            navigationService.Configure(ViewModelLocator.UserPageKey, new Uri("View/UsersControl.xaml", UriKind.Relative));

            SimpleIoc.Default.Register<IModernNavigationService>(() => navigationService);
            SimpleIoc.Default.Register<ISessionService>(() => sessionService);
        }

        public LoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        public PortalViewModel Portal
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PortalViewModel>();
            }
        }
        public UsersViewModel Users
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UsersViewModel>();
            }
        }
        public PatientsViewModel Patients
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PatientsViewModel>();
            }
        }
        public AddUserViewModel AddUser
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddUserViewModel>();
            }
        }
        public AddPatientViewModel AddPatient
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddPatientViewModel>();
            }
        }
        public ObservationsViewModel Observations
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ObservationsViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}