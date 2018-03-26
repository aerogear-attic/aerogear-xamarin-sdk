using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading;
using FFImageLoading.Forms.Droid;
using ImageCircle.Forms.Plugin.Droid;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Platform.Android;

namespace Example.Droid
{
    [Activity(Label = "AeroGear Xamarin", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            MobileCore.Init(new AndroidPlatformInjector());
            CachedImageRenderer.Init(true);
            ImageCircleRenderer.Init();
            LoadApplication(new App());
        }
    }
}

