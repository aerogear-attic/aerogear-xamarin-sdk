using System;
using System.Collections.Generic;

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
        }

        public void OnAuthenticateClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new UserDetails());
        }
    }
}
