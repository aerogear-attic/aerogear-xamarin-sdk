using Aerogear.Mobile.Auth.User;
using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Core;
using System;
using System.Collections.Generic;

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
            User user = MobileCore.Instance.GetInstance<IAuthService>().CurrentUser();

            var roleItems = new List<string> { };

            foreach (var role in user.getRoles()) {
                roleItems.Add(role.ToString());
            }

            lstRoles.ItemsSource = roleItems;
            email.Text = user.Email;
            username.Text = user.Username;
        }

        void onLogoutClicked(object sender, System.EventArgs e)
        {
            MobileCore.Instance.GetInstance<IAuthService>().Logout(
                MobileCore.Instance.GetInstance<IAuthService>().CurrentUser()
            );
            
        }
    }
}
