using System.Diagnostics;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;

namespace mouham_cWpfMedecin.Controls
{
    public class ModernUserControl : UserControl, IContent
    {
          public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            if (FragmentNavigation != null)
            {
                FragmentNavigation(this, e);
            }
        }
        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (NavigatedFrom != null)
            {
                NavigatedFrom(this, e);
            }
        }
        
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigatedTo != null)
            {
                NavigatedTo(this, e);
            }
        }
        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (NavigatingFrom != null)
            {
                NavigatingFrom(this, e);
            }
        }

        public event NavigatingCancelHandler NavigatingFrom;

        public event NavigationEventHandler NavigatedFrom;

        public event NavigationEventHandler NavigatedTo;

        public event FragmentNavigationHandler FragmentNavigation;
    }

    public delegate void NavigatingCancelHandler(object sender, NavigatingCancelEventArgs e);

    public delegate void NavigationEventHandler(object sender, NavigationEventArgs e);

    public delegate void FragmentNavigationHandler(object sender, FragmentNavigationEventArgs e);
}
