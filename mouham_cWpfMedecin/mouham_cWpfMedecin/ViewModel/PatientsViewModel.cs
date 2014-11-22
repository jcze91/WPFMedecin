﻿using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using mouham_cWpfMedecin.ServiceLive;
using mouham_cWpfMedecin.ServicePatient;
using mouham_cWpfMedecin.Services;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class PatientsViewModel : ModernViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IModernNavigationService _modernNavigationService;

        /// <summary>
        /// 
        /// </summary>
        private IServicePatient _servicePatient;

        private ObservableCollection<Patient> _patients;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Patient> Patients
        {
            get { return _patients; }
            set { Set(ref _patients, value, "Patients"); }
        }

        private Patient _selectedPatient;
        /// <summary>
        /// 
        /// </summary>
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { Set(ref _selectedPatient, value, "SelectedPatient"); }
        }

        public ICommand SeeUserProfileCommand { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommand AddPatientCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand DeletePatientCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modernNavigationService"></param>
        /// <param name="sessionService"></param>
        public PatientsViewModel(IModernNavigationService modernNavigationService, ISessionService sessionService, IServicePatient servicePatient)
        {
            this.Role = sessionService.Role;

            _modernNavigationService = modernNavigationService;
            _servicePatient = servicePatient;
                
            LoadedCommand = new RelayCommand(LoadData);
            SeeUserProfileCommand = new RelayCommand(SeeUserProfile);
            AddPatientCommand = new RelayCommand(AddPatient);
            DeletePatientCommand = new RelayCommand(DeletePatient);
        }

        /// <summary>
        /// 
        /// </summary>
        async void LoadData()
        {
            try
            {
                Patients = new ObservableCollection<Patient>(await _servicePatient.GetListPatientAsync());
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void AddPatient()
        {
            _modernNavigationService.NavigateTo(ViewModelLocator.AddPatientPageKey);
        }

        void SeeUserProfile()
        {
            if (SelectedPatient != null)
                _modernNavigationService.NavigateTo(ViewModelLocator.UserProfilePageKey, SelectedPatient);
        }

        /// <summary>
        /// 
        /// </summary>
        async void DeletePatient()
        {
            if (SelectedPatient != null)
            {
                var dialog = new ModernDialog
                {
                    Title = "Supprimer patient",
                    Content = String.Format("Voulez-vous supprimer le patient {0} {1} ?", SelectedPatient.Name, SelectedPatient.Firstname)
                };

                Button cancel = dialog.CancelButton;
                cancel.Content = "Annuler";
                Button yes = dialog.YesButton;
                yes.Content = "Oui";
                dialog.Buttons = new Button[] { cancel, yes };
                dialog.ShowDialog();

                if (dialog.MessageBoxResult == MessageBoxResult.Yes && await _servicePatient.DeletePatientAsync(SelectedPatient.Id))
                    Patients.Remove(SelectedPatient);
            }
        }
    }
}
