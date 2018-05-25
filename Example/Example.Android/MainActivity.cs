
using Acr.UserDialogs;
using AeroGear.Mobile.Auth;
using AeroGear.Mobile.Auth.Config;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Security;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Example.Android.Auth;
using FFImageLoading.Forms.Droid;
using ImageCircle.Forms.Plugin.Droid;

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
            Instance = this;
            var app = new App();
            var ignore = SecurityChecks.BACKUP_DISALLOWED;
            MobileCoreAndroid.Init(app.GetType().Assembly,ApplicationContext);
            var authService = AuthService.InitializeService();
            var authConfig = AuthenticationConfig.Builder.RedirectUri("org.aerogear.mobile.example:/callback").Build();
            authService.Configure(authConfig);

            SecurityService.InitializeService();
            LoadApplication(app);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == AuthenticateOptionsProvider.RequestCode)
            {
                ((AuthService)MobileCore.Instance.GetService<IAuthService>()).HandleAuthResult(data);
            }		
        }
    }
}

