using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.ServicePatient;
using mouham_cWpfMedecin.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// ViewModel for patients
    /// </summary>
    public class PatientsViewModel : ModernViewModelBase
    {
        /// <summary>
        /// navigation service
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// patient service
        /// </summary>
        private readonly IServicePatient _servicePatient;

        private ObservableCollection<Patient> _patients;
        /// <summary>
        /// collection of patient
        /// </summary>
        public ObservableCollection<Patient> Patients
        {
            get { return _patients; }
            set { Set(ref _patients, value, "Patients"); }
        }

        private Patient _selectedPatient;
        /// <summary>
        /// selected patient
        /// </summary>
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { Set(ref _selectedPatient, value, "SelectedPatient"); }
        }

        /// <summary>
        /// command to see user profile
        /// </summary>
        public ICommand SeeUserProfileCommand { get; private set; }

        /// <summary>
        /// command to add patient
        /// </summary>
        public ICommand AddPatientCommand { get; private set; }

        /// <summary>
        /// command to delete patient
        /// </summary>
        public ICommand DeletePatientCommand { get; private set; }

        /// <summary>
        /// constructor of patients viewmodel
        /// </summary>
        /// <param name="modernNavigationService">navigation service</param>
        /// <param name="sessionService">session service</param>
        /// <param name="servicePatient">patient service</param>
        public PatientsViewModel(IModernNavigationService modernNavigationService, ISessionService sessionService, IServicePatient servicePatient)
        {
            this.Role = sessionService.Role;

            _modernNavigationService = modernNavigationService;
            _servicePatient = servicePatient;
                
            LoadedCommand = new RelayCommand(LoadData);
            SeeUserProfileCommand = new RelayCommand(SeeUserProfile);
            AddPatientCommand = new RelayCommand(AddPatient);
            DeletePatientCommand = new RelayCommand(DeletePatient);
        }

        /// <summary>
        /// load data logic
        /// </summary>
        async void LoadData()
        {
            try
            {
                Patients = new ObservableCollection<Patient>(await _servicePatient.GetListPatientAsync());
            }
            catch
            {
            }
        }

        /// <summary>
        /// add patient logic
        /// </summary>
        void AddPatient()
        {
            _modernNavigationService.NavigateTo(ViewModelLocator.AddPatientPageKey);
        }

        /// <summary>
        /// see user profile logic
        /// </summary>
        void SeeUserProfile()
        {
            if (SelectedPatient != null)
                _modernNavigationService.NavigateTo(ViewModelLocator.UserProfilePageKey, SelectedPatient);
        }

        /// <summary>
        /// delete patient logic
        /// </summary>
        async void DeletePatient()
        {
            if (SelectedPatient != null)
            {
                var dialog = new ModernDialog
                {
                    Title = "Supprimer patient",
                    Content = String.Format("Voulez-vous supprimer le patient {0} {1} ?", SelectedPatient.Name, SelectedPatient.Firstname)
                };

                Button cancel = dialog.CancelButton;
                cancel.Content = "Annuler";
                Button yes = dialog.YesButton;
                yes.Content = "Oui";
                dialog.Buttons = new Button[] { cancel, yes };
                dialog.ShowDialog();

                if (dialog.MessageBoxResult == MessageBoxResult.Yes && await _servicePatient.DeletePatientAsync(SelectedPatient.Id))
                    Patients.Remove(SelectedPatient);
            }
        }
    }
}
