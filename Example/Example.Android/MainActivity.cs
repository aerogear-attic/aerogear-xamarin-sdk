
using Acr.UserDialogs;
using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Core;
using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using ImageCircle.Forms.Plugin.Droid;
using Android.Content;
using Example.Android.Auth;
using Xamarin.Forms;
using Example.Views.Pages;
using Xamarin.Forms.Xaml;
using AeroGear.Mobile.Auth.Config;

namespace Example.Android
{
    [Activity(Label = "AeroGear Xamarin", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
            CachedImageRenderer.Init(true);
            ImageCircleRenderer.Init();
            UserDialogs.Init(this);
            Instance = this;//This is not a terrible hack
            var app = new App();
            MobileCoreAndroid.Init(app.GetType().Assembly,ApplicationContext);
            IAuthService service = MobileCore.Instance.GetInstance<IAuthService>();
            var authConfig = AuthenticationConfig.Builder.RedirectUri("org.aerogear.mobile.example:/callback").Build();
            service.Configure(authConfig);
            LoadApplication(app);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == AuthenticateOptionsProvider.RequestCode) {
                ((AuthService)MobileCore.Instance.GetInstance<IAuthService>()).HandleAuthResult(data)
                    .ContinueWith(result=> {  });
            }
        }
    }
}

