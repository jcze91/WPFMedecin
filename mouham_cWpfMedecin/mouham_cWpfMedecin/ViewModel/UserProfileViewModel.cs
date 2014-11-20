using GalaSoft.MvvmLight.Command;
using mouham_cWpfMedecin.ServiceLive;
using mouham_cWpfMedecin.ServiceObservation;
using mouham_cWpfMedecin.ServicePatient;
using mouham_cWpfMedecin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext=false)]
    public class UserProfileViewModel : ModernViewModelBase, IServiceLiveCallback
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;
        private Patient _patient;
        private ServiceObservationClient _serviceObservationClient;
        private ServiceLiveClient _serviceLiveClient;
        private double _heart;
        private double _temperature;

        /// <summary>
        /// 
        /// </summary>
        public double Heart
        {
            get { return _heart; }
            set { Set(ref _heart, value, "Heart"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public double Temperature
        {
            get { return _temperature; }
            set { Set(ref _temperature, value, "Temperature"); }
        }

        /// <summary>
        /// 
        /// </summary>
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
        public UserProfileViewModel(IModernNavigationService modernNavigationService, ISessionService sessionService)
        {
            try
            {
                this.Role = sessionService.Role;

                _modernNavigationService = modernNavigationService;
                _serviceObservationClient = new ServiceObservationClient();

                AddObservationCommand = new RelayCommand(AddObservation);
                OpenWideImageCommand = new RelayCommand(OpenWideImage);
                LoadedCommand = new RelayCommand(LoadData);
                _serviceLiveClient = new ServiceLiveClient(new InstanceContext(this), "WSDualHttpBinding_IServiceLive");
                _serviceLiveClient.Open();
                _serviceLiveClient.SubscribeAsync();
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
        public void PushDataHeart(double requestData)
        {
            Heart = requestData;
        }

        /// <summary>
        /// 
        /// </summary>
        public void PushDataTemp(double requestData)
        {
            Temperature = requestData;
        }
        /// <summary>
        /// 
        /// </summary>
        private void AddObservation()
        {
            throw new NotImplementedException();
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
