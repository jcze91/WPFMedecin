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
    /// 
    /// </summary>
    public class PatientsViewModel : ModernViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// 
        /// </summary>
        private IServicePatient _servicePatientClient;

        private ObservableCollection<Patient> _patients;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Patient> Patients
        {
            get { return _patients; }
            set { Set(ref _patients, value, "Patients"); }
        }

        private Patient _selectedPatient;
        /// <summary>
        /// 
        /// </summary>
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { Set(ref _selectedPatient, value, "SelectedPatient"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand SeeObservationsCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand AddPatientCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand DeletePatientCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modernNavigationService"></param>
        /// <param name="sessionService"></param>
        public PatientsViewModel(IModernNavigationService modernNavigationService, ISessionService sessionService)
        {
            try
            {
                this.Role = sessionService.Role;

                _modernNavigationService = modernNavigationService;
                _servicePatientClient = new ServicePatientClient();

                LoadedCommand = new RelayCommand(LoadData);
                SeeObservationsCommand = new RelayCommand(SeeObservations);
                AddPatientCommand = new RelayCommand(AddPatient);
                DeletePatientCommand = new RelayCommand(DeletePatient);
            }
            catch (Exception e) { }
        }

        /// <summary>
        /// 
        /// </summary>
        async void LoadData()
        {
            try
            {
                Patients = new ObservableCollection<Patient>(await _servicePatientClient.GetListPatientAsync());
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void AddPatient()
        {
            _modernNavigationService.NavigateTo(ViewModelLocator.AddPatientPageKey);
        }

        /// <summary>
        /// 
        /// </summary>
        void SeeObservations()
        {
            if (SelectedPatient != null)
                _modernNavigationService.NavigateTo(ViewModelLocator.ObservationsPageKey, SelectedPatient);
        }

        /// <summary>
        /// 
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

                dialog.Buttons = new Button[] { dialog.CancelButton, dialog.YesButton };
                dialog.ShowDialog();

                if (dialog.MessageBoxResult == MessageBoxResult.Yes && await _servicePatientClient.DeletePatientAsync(SelectedPatient.Id))
                    Patients.Remove(SelectedPatient);
            }
        }
    }
}
