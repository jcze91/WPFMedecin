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
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    public class PatientsViewModel : ModernViewModelBase
    {

        private ObservableCollection<Patient> _patients;
        private ServicePatientClient _servicePatientClient;
        private readonly IModernNavigationService _modernNavigationService;

        public ObservableCollection<Patient> Patients
        {
            get { return _patients; }
            set { Set(ref _patients, value, "Patients"); }
        }

        public ICommand SeeObservationsCommand { get; set; }
        public ICommand AddPatientCommand { get; set; }

        public PatientsViewModel(IModernNavigationService modernNavigationService)
        {
            try
            {
                _modernNavigationService = modernNavigationService;
                LoadedCommand = new RelayCommand(LoadData);
                _servicePatientClient = new ServicePatientClient();
                SeeObservationsCommand = new RelayCommand<object>(c =>
                    {
                        Patient p = c as Patient;
                        _modernNavigationService.NavigateTo(ViewModelLocator.ObservationsPageKey, p);
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
                Patients = new ObservableCollection<Patient>(await _servicePatientClient.GetListPatientAsync());
            }
            catch
            {
            }
        }
    }
}
