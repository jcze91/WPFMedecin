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
using mouham_cWpfMedecin.ServiceLive;
using mouham_cWpfMedecin.ServiceObservation;
using mouham_cWpfMedecin.ServicePatient;
using mouham_cWpfMedecin.Services;
using mouham_cWpfMedecin.ServiceUser;
using System;
using System.ServiceModel;

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
        public const string UserProfilePageKey = "UserProfileView";
        public const string AddUserPageKey = "AddUserView";
        public const string AddPatientPageKey = "AddPatientView";
        public const string UsersPageKey = "UsersView";
        public const string PatientsPageKey = "PatientsView";
        public const string AddObservationPageKey = "AddObservationView";

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<PatientsViewModel>();
            SimpleIoc.Default.Register<UsersViewModel>();
            SimpleIoc.Default.Register<AddUserViewModel>();
            SimpleIoc.Default.Register<AddPatientViewModel>();
            SimpleIoc.Default.Register<UserProfileViewModel>();
            SimpleIoc.Default.Register<AddObservationViewModel>();

            var navigationService = new ModernNavigationService();
            var sessionService = new SessionService();
            var serviceUser = new ServiceUserClient();
            var serviceObservation = new ServiceObservationClient();
            var servicePatient = new ServicePatientClient();

            navigationService.Configure(ViewModelLocator.UserProfilePageKey, new Uri("View/UserProfileControl.xaml", UriKind.Relative));
            navigationService.Configure(ViewModelLocator.AddUserPageKey, new Uri("View/AddUserView.xaml", UriKind.Relative));
            navigationService.Configure(ViewModelLocator.AddPatientPageKey, new Uri("View/AddPatientView.xaml", UriKind.Relative));
            navigationService.Configure(ViewModelLocator.UsersPageKey, new Uri("View/UsersControl.xaml", UriKind.Relative));
            navigationService.Configure(ViewModelLocator.PatientsPageKey, new Uri("View/PatientsControl.xaml", UriKind.Relative));
            navigationService.Configure(ViewModelLocator.AddObservationPageKey, new Uri("View/AddObservationView.xaml", UriKind.Relative));

            SimpleIoc.Default.Register<IModernNavigationService>(() => navigationService);
            SimpleIoc.Default.Register<ISessionService>(() => sessionService);
            SimpleIoc.Default.Register<IServicePatient>(() => servicePatient);
            SimpleIoc.Default.Register<IServiceUser>(() => serviceUser);
            SimpleIoc.Default.Register<IServiceObservation>(() => serviceObservation);

            var serviceLive = new ServiceLiveClient(new InstanceContext(UserProfile), "WSDualHttpBinding_IServiceLive");
            serviceLive.Open();
            serviceLive.SubscribeAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        public LoginViewModel Login
        {
            get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public UsersViewModel Users
        {
            get { return ServiceLocator.Current.GetInstance<UsersViewModel>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public PatientsViewModel Patients
        {
            get { return ServiceLocator.Current.GetInstance<PatientsViewModel>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public AddUserViewModel AddUser
        {
            get { return ServiceLocator.Current.GetInstance<AddUserViewModel>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public AddPatientViewModel AddPatient
        {
            get { return ServiceLocator.Current.GetInstance<AddPatientViewModel>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public UserProfileViewModel UserProfile
        {
            get { return ServiceLocator.Current.GetInstance<UserProfileViewModel>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public AddObservationViewModel AddObservation
        {
            get { return ServiceLocator.Current.GetInstance<AddObservationViewModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}