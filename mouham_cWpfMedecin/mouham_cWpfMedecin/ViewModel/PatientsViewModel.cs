using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    public class PatientsViewModel : ModernViewModelBase
    {

        private ObservableCollection<PatientServiceReference.Patient> _patients;
        private PatientServiceReference.ServicePatientClient _servicePatientClient;
        private readonly IModernNavigationService _modernNavigationService;
        private PatientServiceReference.Patient _selectedPatient;

        public ObservableCollection<PatientServiceReference.Patient> Patients
        {
            get { return _patients; }
            set
            {
                if (_patients != value)
                {
                    _patients = value;
                    RaisePropertyChanged("Patients");
                }
            }
        }
        public ICommand SeeObservationsCommand { get; set; }
        public ICommand AddPatientCommand { get; set; }

        public PatientsViewModel(IModernNavigationService modernNavigationService)
        {
            try
            {
                _modernNavigationService = modernNavigationService;
                LoadedCommand = new RelayCommand(LoadData);
                _servicePatientClient = new PatientServiceReference.ServicePatientClient();
                SeeObservationsCommand = new RelayCommand<Object>(c =>
                    {
                        _modernNavigationService.NavigateTo(ViewModelLocator.ObservationsPageKey);
                    }, c => true);
                AddPatientCommand = new RelayCommand(() =>
                {
                    _modernNavigationService.NavigateTo(ViewModelLocator.AddPatientPageKey);
                });
            }
            catch { }
        }

        private async void LoadData()
        {
            try
            {
                Patients = new ObservableCollection<PatientServiceReference.Patient>(await _servicePatientClient.GetListPatientAsync());
            }
            catch
            {
            }
        }
    }
}
