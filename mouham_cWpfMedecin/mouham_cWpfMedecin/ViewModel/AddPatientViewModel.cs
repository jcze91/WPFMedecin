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
    /// View model of Patient insertion view
    /// </summary>
    public class AddPatientViewModel : ModernViewModelBase
    {
        /// <summary>
        /// Navigation service
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// Patient service WCF
        /// </summary>
        private IServicePatient _servicePatient;

        private string _name;
        /// <summary>
        /// Patient name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value, "Name"); }
        }

        private string _firstname;
        /// <summary>
        /// Patient firstname
        /// </summary>
        public string Firstname
        {
            get { return _firstname; }
            set { Set(ref _firstname, value, "Firstname"); }
        }

        private DateTime _birthday;
        /// <summary>
        /// Patient birthday
        /// </summary>
        public DateTime Birthday
        {
            get { return _birthday; }
            set { Set(ref _birthday, value, "Birthday"); }
        }

        /// <summary>
        /// Command to add patient
        /// </summary>
        public ICommand ComfirmCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the AddPatientViewModel class.
        /// </summary>
        /// <param name="modernNavigationService"></param>
        public AddPatientViewModel(IModernNavigationService modernNavigationService, IServicePatient servicePatient)
        {
            _servicePatient = servicePatient;
            _modernNavigationService = modernNavigationService;
            ComfirmCommand = new RelayCommand(AddPatient);
            LoadedCommand = new RelayCommand(LoadData);
        }
        private void LoadData()
        {
            this.Name = "";
            this.Firstname = "";
            this.Birthday = DateTime.Today;
        }

        /// <summary>
        /// Add patient method
        /// </summary>
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
                length = _servicePatient.GetListPatient().Length;
            }
            catch { }

            patient.Id = length;
            patient.Observations = new List<Observation>().ToArray();
            try
            {
                result = await _servicePatient.AddPatientAsync(patient);
            }
            catch { }

            Trace.WriteLine(result);
            _modernNavigationService.NavigateTo(ViewModelLocator.PatientsPageKey);
        }
    }
}