using Caliburn.Micro;
using GroundRunning.GUI.ViewModels;

namespace GroundRunning.GUI
{
    public class UserInterfaceBootstrapper : BootstrapperBase
    {
        public UserInterfaceBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }

    }
}
