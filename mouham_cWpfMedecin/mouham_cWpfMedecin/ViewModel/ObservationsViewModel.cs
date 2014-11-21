using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.ServiceObservation;
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
    public class ObservationsViewModel : ModernViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// 
        /// </summary>
        private IServiceObservation _serviceObservationClient;

        /// <summary>
        /// 
        /// </summary>
        private Patient _patient;
        public Patient Patient
        {
            get { return _patient; }
            set { Set(ref _patient, value, "Patient"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand AddObservationCommand { get; set; }
        public ICommand OpenWideImageCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modernNavigationService"></param>
        /// <param name="sessionService"></param>
        public ObservationsViewModel(IModernNavigationService modernNavigationService, ISessionService sessionService)
        {
            try
            {
                this.Role = sessionService.Role;

                _modernNavigationService = modernNavigationService;
                _serviceObservationClient = new ServiceObservationClient();

                AddObservationCommand = new RelayCommand(AddObservation);
                OpenWideImageCommand = new RelayCommand(OpenWideImage);
                LoadedCommand = new RelayCommand(LoadData);
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            this.Patient = _modernNavigationService.Parameter as Patient;
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddObservation()
        {
            _modernNavigationService.NavigateTo(ViewModelLocator.AddObservationPageKey);
        }

        /// <summary>
        /// 
        /// </summary>
        void OpenWideImage()
        {
            throw new NotImplementedException();
        }

    }
}
