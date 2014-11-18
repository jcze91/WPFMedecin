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

using GalaSoft.MvvmLight;
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
            SimpleIoc.Default.Register<ObservationsViewModel>();

            var navigationService = new ModernNavigationService();
            navigationService.Configure(ViewModelLocator.ObservationsPageKey, new Uri("View/ObservationsControl.xaml", UriKind.Relative));

            SimpleIoc.Default.Register<IModernNavigationService>(() => navigationService);
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