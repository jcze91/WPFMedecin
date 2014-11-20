using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.ServicePatient;
using mouham_cWpfMedecin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    public class PatientsViewModel : ModernViewModelBase
    {

        private IServicePatient _servicePatientClient;
        private readonly IModernNavigationService _modernNavigationService;

        private ObservableCollection<Patient> _patients;
        public ObservableCollection<Patient> Patients
        {
            get { return _patients; }
            set { Set(ref _patients, value, "Patients"); }
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { Set(ref _selectedPatient, value, "SelectedPatient"); }
        }

        public ICommand SeeObservationsCommand { get; private set; }
        public ICommand AddPatientCommand { get; private set; }
        public ICommand DeletePatientCommand { get; private set; }

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

        void AddPatient()
        {
            _modernNavigationService.NavigateTo(ViewModelLocator.AddPatientPageKey);
        }

        void SeeObservations()
        {
            if (SelectedPatient != null)
                _modernNavigationService.NavigateTo(ViewModelLocator.ObservationsPageKey, SelectedPatient);
        }

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
    }
}
