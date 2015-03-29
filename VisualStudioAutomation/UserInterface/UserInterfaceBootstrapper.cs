using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInterface.ViewModels;

namespace UserInterface
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
