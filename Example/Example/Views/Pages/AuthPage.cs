using System;

using Xamarin.Forms;

namespace Example.Views.Pages
{
    public class AuthPage : ContentPage
    {
        public AuthPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

