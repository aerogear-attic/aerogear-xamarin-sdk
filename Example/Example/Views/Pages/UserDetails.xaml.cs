using System;
using System.Collections.Generic;
using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Core;
using Xamarin.Forms;

namespace Example.Views.Pages
{
    public partial class UserDetails : ContentPage
    {
        public UserDetails()
        {
            InitializeComponent();
            InitListView();
        }

        private void InitListView() {
            var user = MobileCore.Instance.GetInstance<IAuthService>().CurrentUser();
            var roleItems = new List<string> { };
            foreach(var role in user.getRoles())
            {
                roleItems.Add(role.ToString());
            }

            lstRoles.ItemsSource = roleItems;
            username.Text = user.Username;
            email.Text = user.Email;
        }

        void onLogoutClicked(object sender, System.EventArgs e)
        {
            var authService = MobileCore.Instance.GetInstance<IAuthService>();
            authService.Logout(authService.CurrentUser()).ContinueWith(result =>
            {
                Device.BeginInvokeOnMainThread(() => Navigation.PushAsync(new AuthPage()));
            });
        }
    }
}
