using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.ServicePatient;
using mouham_cWpfMedecin.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddPatientViewModel : ModernViewModelBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        private string _firstname;

        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (_firstname != value)
                {
                    _firstname = value;
                    RaisePropertyChanged("Firstname");
                }
            }
        }
        private DateTime _birthday;

        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                if (_birthday != value)
                {
                    _birthday = value;
                    RaisePropertyChanged("Birthday");
                }
            }
        }

        public ICommand ComfirmCommand { get; set; }
        private readonly IModernNavigationService _modernNavigationService;
        private ServicePatientClient _servicePatientClient;

        /// <summary>
        /// Initializes a new instance of the AddPatientViewModel1 class.
        /// </summary>
        public AddPatientViewModel(IModernNavigationService modernNavigationService)
        {
            _servicePatientClient = new ServicePatientClient();
            _modernNavigationService = modernNavigationService;
            ComfirmCommand = new RelayCommand(() => AddPatient());
            LoadedCommand = new RelayCommand(LoadData);
        }
        private void LoadData()
        {
            this.Name = "";
            this.Firstname = "";
            this.Birthday = DateTime.Today;
        }

        private async void AddPatient()
        {
            Patient patient = new Patient();
            bool result = false;

            patient.Firstname = _firstname;
            patient.Name = _name;
            patient.Birthday = _birthday;

            int length = 0;
            try
            {
                length = _servicePatientClient.GetListPatient().Length;
            }
            catch { }

            patient.Id = length;
            patient.Observations = new List<Observation>().ToArray();
            try
            {
                result = await _servicePatientClient.AddPatientAsync(patient);
            }
            catch { }

            Trace.WriteLine(result);
            _modernNavigationService.NavigateTo(ViewModelLocator.PatientsPageKey);
        }
    }
}