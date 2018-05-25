using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Logging;
using AeroGear.Mobile.Core.Utils;
using AeroGear.Mobile.Security;
using Example.Models;
using Example.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Example.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SecurityPage : ContentPage
	{
        List<ISecurityCheck> checks = new List<ISecurityCheck>();
        private CancellationTokenSource cts;
        private readonly SecurityCheckVM securityCheckVM;
        private ILogger LOGGER = MobileCore.Instance.Logger;

        public SecurityPage()
		{
            var securityFactory = ServiceFinder.Resolve<ISecurityCheckFactory>();


            if (Device.RuntimePlatform == Device.iOS)
            {
                checks.Add(securityFactory.create("NonJailbrokenCheck"));
                checks.Add(securityFactory.create("DeviceLockCheck"));
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                checks.Add(securityFactory.create("NonRootedCheck"));
                checks.Add(securityFactory.create("DeveloperModeDisabledCheck"));
                checks.Add(securityFactory.create("ScreenLockCheck"));
                checks.Add(securityFactory.create("BackupDisallowedCheck"));
                checks.Add(securityFactory.create("EncryptionCheck"));
            }

            checks.Add(securityFactory.create("NotInEmulatorCheck"));
            checks.Add(securityFactory.create("NoDebuggerCheck"));

            securityCheckVM = new SecurityCheckVM(checks);
            BindingContext = securityCheckVM;

            InitializeComponent();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cts = new CancellationTokenSource();
            Task.Run(async () => await securityCheckVM.Fetch());
        }

        protected override void OnDisappearing()
        {
            cts.Cancel();
            base.OnDisappearing();
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            LOGGER.Info("SecurityPage", ((CheckModel)e.Item).name);
        }

    }
}