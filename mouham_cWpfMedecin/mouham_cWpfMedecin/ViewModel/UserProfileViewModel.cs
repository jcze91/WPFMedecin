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
        /// 
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;
        private Patient _patient;
        private IServiceObservation _serviceObservation;
        private IServicePatient _servicePatient;
        private string _heart;
        private double _temperature;
        private DateTime _startTime;
        private ObservableCollection<IPlotterElement> _heartChart;
        private CompositeDataSource _heartData;
        private List<KeyValuePair<TimeSpan, double>> _heartValues;
        private bool _canRefreshHeart;

        public ObservableCollection<IPlotterElement> HeartChart
        {
            get { return _heartChart; }
            set { Set(ref _heartChart, value, "HeartChart"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Heart
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modernNavigationService"></param>
        /// <param name="sessionService"></param>
        /// <param name="serviceObservation"></param>
        public UserProfileViewModel(
            IModernNavigationService modernNavigationService,
            ISessionService sessionService,
            IServiceObservation serviceObservation,
            IServicePatient servicePatient)
        {
            this.Role = sessionService.Role;

            _modernNavigationService = modernNavigationService;
            _serviceObservation = serviceObservation;
            _servicePatient = servicePatient;

            AddObservationCommand = new RelayCommand(AddObservation);
            LoadedCommand = new RelayCommand(LoadData);
            _heartValues = new List<KeyValuePair<TimeSpan, double>>();
            HeartChart = new ObservableCollection<IPlotterElement>();
            _canRefreshHeart = false;
        }

        /// <summary>
        /// 
        /// </summary>
        async private void LoadData()
        {
            try
            {
                var p = _modernNavigationService.Parameter as Patient;
                if (p != null)
                {
                    Patient = await _servicePatient.GetPatientAsync(p.Id);
                }
                this.HeartChart.Clear();
                _heartValues.Clear();
                _canRefreshHeart = !_canRefreshHeart;
                _startTime = DateTime.Now;
            }
            catch { }
        }

        /// <summary>
        /// 
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
            _modernNavigationService.NavigateTo(ViewModelLocator.AddObservationPageKey, Patient);
        }
    }
}
