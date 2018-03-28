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
            lstRoles.ItemsSource = new List<string> { "Role1", "Role2", "Role3" };;
        }

        void onLogoutClicked(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
