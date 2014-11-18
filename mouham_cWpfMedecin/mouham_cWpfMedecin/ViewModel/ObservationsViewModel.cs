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
    public class ObservationsViewModel : ModernViewModelBase
    {
        private readonly IModernNavigationService _modernNavigationService;
        private PatientServiceReference.Patient _patient;
        private ObservationServiceReference.ServiceObservationClient _serviceObservationClient;

        public PatientServiceReference.Patient Patient
        {
            get { return _patient; }
            set
            {
                if (_patient != value)
                {
                    _patient = value;
                    RaisePropertyChanged("Patient");
                }
            }
        }
        public ICommand AddObservationCommand { get; set; }


        public ObservationsViewModel(IModernNavigationService modernNavigationService)
        {
            try
            {
                _modernNavigationService = modernNavigationService;
                _serviceObservationClient = new ObservationServiceReference.ServiceObservationClient();
                AddObservationCommand = new RelayCommand<Object>(c =>
                    {
                        //_modernNavigationService.NavigateTo(ViewModelLocator.AddObservationPageKey);
                    }, c => true);
                LoadedCommand = new RelayCommand(LoadData);
            }
            catch { }
        }

        private void LoadData()
        {
            this.Patient = _modernNavigationService.Parameter as PatientServiceReference.Patient;
        }

    }
}
