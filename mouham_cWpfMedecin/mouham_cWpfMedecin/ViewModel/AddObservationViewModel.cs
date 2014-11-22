using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using mouham_cWpfMedecin.ServiceObservation;
using mouham_cWpfMedecin.ServicePatient;
using mouham_cWpfMedecin.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class AddObservationViewModel : ModernViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// 
        /// </summary>
        private IServiceObservation _serviceObservation;

        /// <summary>
        /// 
        /// </summary>
        public Patient Patient { get; private set; }

        private DateTime _selectedDate = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { Set(ref _selectedDate, value, "SelectedDate"); }
        }

        private string _comment;
        /// <summary>
        /// 
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set { Set(ref _comment, value, "Comment"); }
        }

        private int _weight;
        /// <summary>
        /// 
        /// </summary>
        public int Weight
        {
            get { return _weight; }
            set { Set(ref _weight, value, "Weight"); }
        }

        private int _bloodPressure;
        /// <summary>
        /// 
        /// </summary>
        public int BloodPressure
        {
            get { return _bloodPressure; }
            set { Set(ref _bloodPressure, value, "BloodPressure"); }
        }

        private string _currentPrescription;
        /// <summary>
        /// 
        /// </summary>
        public string CurrentPrescription
        {
            get { return _currentPrescription; }
            set { Set(ref _currentPrescription, value, "CurrentPrescription"); }
        }

        private ObservableCollection<string> _prescriptions;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<string> Prescriptions
        {
            get { return _prescriptions; }
            set { Set(ref _prescriptions, value, "Prescriptions"); }
        }

        private ObservableCollection<Byte[]> _pictures;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Byte[]> Pictures
        {
            get { return _pictures; }
            set { Set(ref _pictures, value, "Pictures"); }
        }

        private string _pictureFilename;
        /// <summary>
        /// 
        /// </summary>
        public string PictureFilename
        {
            get { return _pictureFilename; }
            set { Set(ref _pictureFilename, value, "PictureFilename"); }
        }

        private readonly ICommand _addPrescriptionCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand AddPrescriptionCommand { get { return _addPrescriptionCommand; } }

        private readonly ICommand _browseCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand BrowseCommand { get { return _browseCommand; } }

        private readonly ICommand _addPictureCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand AddPictureCommand { get { return _addPictureCommand; } }

        private readonly ICommand _addObservationCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand AddObservationCommand { get { return _addObservationCommand; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modernNavigationService"></param>
        /// <param name="serviceObservation"></param>
        public AddObservationViewModel(IModernNavigationService modernNavigationService, IServiceObservation serviceObservation)
        {
            _modernNavigationService = modernNavigationService;
            _serviceObservation = serviceObservation;

            _prescriptions = new ObservableCollection<string>();
            _pictures = new ObservableCollection<byte[]>();

            LoadedCommand = new RelayCommand(LoadData);
            NavigatedFromCommand = new RelayCommand(Cleanup);
            _browseCommand = new RelayCommand(SelectFile);
            _addPrescriptionCommand = new RelayCommand(AddPrescription);
            _addPictureCommand = new RelayCommand(AddPicture);
            _addObservationCommand = new RelayCommand(AddObservation);
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            Patient = _modernNavigationService.Parameter as Patient;
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddPrescription()
        {
            if (!string.IsNullOrEmpty(CurrentPrescription))
            {
                Prescriptions.Add(CurrentPrescription);
                CurrentPrescription = string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SelectFile()
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Images (.jpg)|*.jpg";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                PictureFilename = dlg.FileName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddPicture()
        {
            try { Pictures.Add(System.IO.File.ReadAllBytes(PictureFilename)); }
            catch (Exception e)
            {
                var dialog = new ModernDialog
                {
                    Title = "Echec",
                    Content = String.Format(e.ToString())
                };

                dialog.Buttons = new Button[] { dialog.OkButton };
                dialog.ShowDialog();
            }
            finally { PictureFilename = string.Empty; }
        }

        /// <summary>
        /// 
        /// </summary>
        async private void AddObservation()
        {

            try
            {
                ServiceObservation.Observation obs = new ServiceObservation.Observation();

                obs.Date = SelectedDate;
                obs.BloodPressure = BloodPressure;
                obs.Comment = Comment;
                obs.Weight = Weight;
                obs.Pictures = Pictures.ToArray();
                obs.Prescription = Prescriptions.ToArray();

                if (await _serviceObservation.AddObservationAsync(Patient.Id, obs))
                {
                    Cleanup();
                    _modernNavigationService.GoBack();
                }
                else
                {
                    var dialog = new ModernDialog
                    {
                        Title = "Échec",
                        Content = String.Format("L'opération n'a pas pu être effectuée")
                    };

                    dialog.Buttons = new Button[] { dialog.OkButton };
                    dialog.ShowDialog();
                }
            }
            catch (Exception e)
            {
                var dialog = new ModernDialog
                {
                    Title = "Échec",
                    Content = String.Format(e.ToString())
                };

                dialog.Buttons = new Button[] { dialog.OkButton };
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();

            BloodPressure = 0;
            Comment = string.Empty;
            Weight = 0;
            Pictures.Clear();
            Prescriptions.Clear();
        }
    }
}
