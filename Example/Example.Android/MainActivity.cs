
using Acr.UserDialogs;
using AeroGear.Mobile.Core;
using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using ImageCircle.Forms.Plugin.Droid;

namespace Example.Android
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
            
            CachedImageRenderer.Init(true);
            ImageCircleRenderer.Init();
            UserDialogs.Init(this);
            var app = new App();
            MobileCoreAndroid.Init(app.GetType().Assembly,ApplicationContext);
            LoadApplication(app);
        }
    }
}

