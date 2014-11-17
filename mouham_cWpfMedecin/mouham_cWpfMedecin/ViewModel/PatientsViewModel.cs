using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mouham_cWpfMedecin.ViewModel
{
    public class PatientsViewModel : ModernViewModelBase
    {

        private ObservableCollection<PatientServiceReference.Patient> _Patients;
        private PatientServiceReference.ServicePatientClient _servicePatientClient;

        public ObservableCollection<PatientServiceReference.Patient> Patients
        {
            get { return _Patients; }
            set
            {
                if (_Patients != value)
                {
                    _Patients = value;
                    RaisePropertyChanged("Patients");
                }
            }
        }

        public PatientsViewModel()
        {
            LoadedCommand = new RelayCommand(LoadData);
            _servicePatientClient = new PatientServiceReference.ServicePatientClient();
        }
       
        private async void LoadData()
        {
            Patients = new ObservableCollection<PatientServiceReference.Patient>(await _servicePatientClient.GetListPatientAsync());
        }

    }
}
