using Example.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Example.Models;
using Acr.UserDialogs;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Logging;

namespace Example.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HttpPage : ContentPage
    {
        private readonly PlaceholderUsersVM placeholderUsersVM;
        private CancellationTokenSource cts;
        private ILogger LOGGER = MobileCore.Instance.Logger;

        public HttpPage()
        {
            InitializeComponent();
            placeholderUsersVM = new PlaceholderUsersVM();
            BindingContext = placeholderUsersVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cts = new CancellationTokenSource();
            Task.Run(async () => await placeholderUsersVM.Fetch());
        }

        protected override void OnDisappearing()
        {
            cts.Cancel();
            base.OnDisappearing();
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            PlaceholderUser user = (PlaceholderUser)e.Item;
            var message = $"{user.name}\n{user.email}";
            var toastConfig = new ToastConfig(message);
            toastConfig.Duration = TimeSpan.FromMilliseconds(3000);
            toastConfig.BackgroundColor = System.Drawing.Color.FromArgb(12, 131, 193);
            UserDialogs.Instance.Toast(toastConfig);
            LOGGER.Info("HttpPage", message);
        }
    }
}
