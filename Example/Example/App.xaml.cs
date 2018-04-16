using System;

using Example.Views;
using Xamarin.Forms;
using AeroGear.Mobile.Core;

namespace Example
{
	public partial class App : Application
	{

		public App ()
		{
			InitializeComponent();

            MainPage = new RootPage();
        }

		protected override void OnStart ()
		{
            

		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
