using GalaSoft.MvvmLight.Command;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace mouham_cWpfMedecin.ViewModel
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false)]
    public class UserProfileViewModel : ModernViewModelBase, IServiceLiveCallback
    {
        /// <summary>
        /// navigation service
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// session service
        /// </summary>
        private readonly ISessionService _sessionService;

        /// <summary>
        /// observation service
        /// </summary>
        private readonly IServiceObservation _serviceObservation;

        /// <summary>
        /// patient service
        /// </summary>
        private readonly IServicePatient _servicePatient;


        private Patient _patient;
        private string _heart;
        private double _temperature;
        private DateTime _startTime;
        private ObservableCollection<IPlotterElement> _heartChart;
        private CompositeDataSource _heartData;
        private List<KeyValuePair<TimeSpan, double>> _heartValues;
        private bool _canRefreshHeart;

        /// <summary>
        /// heart chart
        /// </summary>
        public ObservableCollection<IPlotterElement> HeartChart
        {
            get { return _heartChart; }
            set { Set(ref _heartChart, value, "HeartChart"); }
        }

        /// <summary>
        /// heart value
        /// </summary>
        public string Heart
        {
            get { return _heart; }
            set { Set(ref _heart, value, "Heart"); }
        }

        /// <summary>
        /// temparature value
        /// </summary>
        public double Temperature
        {
            get { return _temperature; }
            set { Set(ref _temperature, value, "Temperature"); }
        }

        /// <summary>
        /// current patient
        /// </summary>
        public Patient Patient
        {
            get { return _patient; }
            set { Set(ref _patient, value, "Patient"); }
        }

        /// <summary>
        /// command to add observation
        /// </summary>
        public ICommand AddObservationCommand { get; set; }

        /// <summary>
        /// constructor of user profile viewmodel
        /// </summary>
        /// <param name="modernNavigationService">navigation service</param>
        /// <param name="sessionService">session service</param>
        /// <param name="serviceObservation">observation service</param>
        /// <param name="servicePatient">patient service</param>
        public UserProfileViewModel(
            IModernNavigationService modernNavigationService,
            ISessionService sessionService,
            IServiceObservation serviceObservation,
            IServicePatient servicePatient)
        {
            _modernNavigationService = modernNavigationService;
            _serviceObservation = serviceObservation;
            _servicePatient = servicePatient;
            _sessionService = sessionService;

            AddObservationCommand = new RelayCommand(AddObservation);
            LoadedCommand = new RelayCommand(LoadData);
            _heartValues = new List<KeyValuePair<TimeSpan, double>>();
            HeartChart = new ObservableCollection<IPlotterElement>();
            _canRefreshHeart = false;
        }

        /// <summary>
        /// load data logic
        /// </summary>
        async private void LoadData()
        {
            try
            {
                this.Role = _sessionService.Role;
                var p = _modernNavigationService.Parameter as Patient;
                if (p == null)
                    p = _modernNavigationService.LastParameter as Patient;
                if (p != null)
                    Patient = await _servicePatient.GetPatientAsync(p.Id);
                this.HeartChart.Clear();
                _heartValues.Clear();
                _canRefreshHeart = !_canRefreshHeart;
                _startTime = DateTime.Now;
            }
            catch { }
        }

        /// <summary>
        /// receive heart push data and add to chart
        /// </summary>
        public void PushDataHeart(double requestData)
        {
            try
            {
                if (_canRefreshHeart)
                {
                    if (_heartValues.Count > 30)
                        _heartValues = _heartValues.Skip(1).ToList();

                    _heartValues.Add(new KeyValuePair<TimeSpan, double>(DateTime.Now.Subtract(_startTime), requestData));
                    var xData = new EnumerableDataSource<double>(_heartValues.Select(c => c.Key.TotalMilliseconds));
                    xData.SetXMapping(x => x);
                    var yData = new EnumerableDataSource<double>(_heartValues.Select(c => c.Value));
                    yData.SetYMapping(y => y);
                    _heartData = xData.Join(yData);
                    _canRefreshHeart = false;

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        this.HeartChart = new ObservableCollection<IPlotterElement>();
                        LineGraph lg = new LineGraph(_heartData);
                        lg.Stroke = new SolidColorBrush(Colors.DodgerBlue);
                        lg.Description.LegendItem.Visibility = Visibility.Collapsed;
                        this.HeartChart.Add(lg);

                        _canRefreshHeart = true;
                    });

                    Heart = requestData.ToString("0.000");
                }
            }
            catch { }
        }

        /// <summary>
        /// receive temperature push data
        /// </summary>
        public void PushDataTemp(double requestData)
        {
            Temperature = requestData;
        }

        /// <summary>
        /// add observatoin logic
        /// </summary>
        private void AddObservation()
        {
            _modernNavigationService.NavigateTo(ViewModelLocator.AddObservationPageKey, Patient);
        }
    }
}
