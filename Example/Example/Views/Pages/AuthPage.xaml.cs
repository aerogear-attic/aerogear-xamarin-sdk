﻿using System;
using System.Collections.Generic;
using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Core;
using Example.Auth;
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
            if (service.CurrentUser() != null)
            {
                Navigation.PushAsync(new UserDetails());
            }
        }

        public void OnAuthenticateClicked(object sender, EventArgs args)
        {
            IAuthService service = MobileCore.Instance.GetInstance<IAuthService>();
            var authOptions = DependencyService.Get<IAuthenticateOptionsProvider>().GetOptions();
            service.Authenticate(authOptions).ContinueWith(result =>
            {
                Device.BeginInvokeOnMainThread(() => Navigation.PushAsync(new UserDetails()));
            });
        }
    }
}
