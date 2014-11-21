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
        /// <summary>
        /// 
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// 
        /// </summary>
        private IServicePatient _servicePatientClient;

        private string _name;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value, "Name"); }
        }

        private string _firstname;
        /// <summary>
        /// 
        /// </summary>
        public string Firstname
        {
            get { return _firstname; }
            set { Set(ref _firstname, value, "Firstname"); }
        }

        private DateTime _birthday;
        /// <summary>
        /// 
        /// </summary>
        public DateTime Birthday
        {
            get { return _birthday; }
            set { Set(ref _birthday, value, "Birthday"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ComfirmCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the AddPatientViewModel class.
        /// </summary>
        /// <param name="modernNavigationService"></param>
        public AddPatientViewModel(IModernNavigationService modernNavigationService)
        {
            _servicePatientClient = new ServicePatientClient();
            _modernNavigationService = modernNavigationService;
            ComfirmCommand = new RelayCommand(AddPatient);
            LoadedCommand = new RelayCommand(LoadData);
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            this.Name = "";
            this.Firstname = "";
            this.Birthday = DateTime.Today;
        }

        /// <summary>
        /// 
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