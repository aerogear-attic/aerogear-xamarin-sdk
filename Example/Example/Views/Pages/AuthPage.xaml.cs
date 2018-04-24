using System;
using System.Collections.Generic;
using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Core;
using Example.Android.Events;
using Xamarin.Forms;

using Xamarin.Forms.Xaml;
namespace Example.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        public AuthPage()
        {
            InitializeComponent();
            IAuthService service = MobileCore.Instance.GetInstance<IAuthService>();
            if (service.CurrentUser() != null) {
                Navigation.PushAsync(new UserDetails());
            }
        }

        public void OnAuthenticateClicked(object sender, EventArgs args)
        {
            
            
            var authOptions = DependencyService.Get<IAuthenticateOptionsProvider>().GetOptions();
            IAuthService service = MobileCore.Instance.GetInstance<IAuthService>();
            
            Console.WriteLine("=== WOOP ===");
            Console.WriteLine(authOptions);

            service.Authenticate(authOptions);
            Navigation.PopToRootAsync();


        }
    }
}
