using mouham_cWpfMedecin.ServiceObservation;
using mouham_cWpfMedecin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mouham_cWpfMedecin.ViewModel
{
    public class AddObservationViewModel : ModernViewModelBase
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
        /// <param name="modernNavigationService"></param>
        public AddObservationViewModel(IModernNavigationService modernNavigationService)
        {
            _modernNavigationService = modernNavigationService;
            _serviceObservationClient = new ServiceObservationClient();
        }


    }
}
