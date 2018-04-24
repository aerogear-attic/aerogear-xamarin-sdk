﻿using System;
using System.Collections.Generic;
using AeroGear.Mobile.Auth;
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
        }

        public void OnAuthenticateClicked(object sender, EventArgs args)
        {
            IAuthService service = MobileCore.Instance.GetInstance<IAuthService>();
            var authOptions = DependencyService.Get<IAuthenticateOptionsProvider>().GetOptions();

            Console.WriteLine("=== WOOP ===");
            Console.WriteLine(authOptions);

            service.Authenticate(authOptions);

            Navigation.PushAsync(new UserDetails());
        }
    }
}
