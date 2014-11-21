using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mouham_cWpfMedecin.ViewModel
{
    /// <summary>
    /// The modern view model base.
    /// </summary>
    public abstract class ModernViewModelBase : ViewModelBase
    {
        private string _role;
        /// <summary>
        /// Gets or sets the navigating from command.
        /// </summary>
        /// <value>The navigating from command.</value>
        public ICommand NavigatingFromCommand { get; set; }

        /// <summary>
        /// Gets or sets the navigated from command.
        /// </summary>
        /// <value>The navigated from command.</value>
        public ICommand NavigatedFromCommand { get; set; }

        /// <summary>
        /// Gets or sets the navigated to command.
        /// </summary>
        /// <value>The navigated to command.</value>
        public ICommand NavigatedToCommand { get; set; }

        /// <summary>
        /// Gets or sets the fragment navigation command.
        /// </summary>
        /// <value>The fragment navigation command.</value>
        public ICommand FragmentNavigationCommand { get; set; }

        /// <summary>
        /// Gets or sets the loaded command.
        /// </summary>
        /// <value>The loaded command.</value>
        public ICommand LoadedCommand { get; set; }

        /// <summary>
        /// Gets or sets the is visible changed command.
        /// </summary>
        /// <value>The is visible changed command.</value>
        public ICommand IsVisibleChangedCommand { get; set; }

        /// <summary>
        /// Get or sets the role of current user.
        /// </summary>
        /// <value>The user role</value>
        public string Role
        {
            get { return _role; }
            set { Set(ref _role, value, "Role"); }
        }
    }
}
