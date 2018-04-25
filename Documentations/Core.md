# AeroGear Xamarin Core SDK

Core SDK provides numbers of helpers for individual SDK's
Core can be also used to extract common implementations from SDK's.

Main responsibilities:

1. Configuration parser - loads configuration from JSON file and configures AeroGear modules
1. Network library - provides HTTP(s) access for the modules 
1. Default logger shared between SDK - provides logging for the modules

Underlying SDK architecture

## Adding NuGet


Core platfom-independent module:

```
TBD
```

Core platform module:


```
TBD Android, iOS
```


## Usage

Core SDK has to be initialized in your `Activity` (Android) or `AppDelegate` (iOS) before use:

If you use Xamarin.Forms on Android:

```C# 
var app = new App();
MobileCoreAndroid.Init(app.GetType().Assembly,ApplicationContext);
LoadApplication(app);
```

If you use Xamarin.Forms on iOS:

```C# 
var xamApp = new App();
MobileCoreIOS.Init(xamApp.GetType().Assembly);
LoadApplication(xamApp);
```

Legacy Xamarin for Android:

```C#
MobileCoreAndroid.Init(ApplicationContext);
```

Legacy Xamarin for iOS:
```C#
MobileCoreIOS.Init();
```


## Core API

Instance of the Core is accessible by 

```C#
MobileCore.Instance
```


### Logger

Core provides by default a platform-specific implementation of the logger. On Android it's standard logcat on iOS it's a standard system log.

You can log message with specific log tag:
```C#
MobileCore.Instance.Logger.Info("MyTag", "my log message");
```

Logger can be changed by providing your own implementation in initialization [Options](./Core/Core/Options.cs):
```C#
Options options = Options.Builder.Logger(new NullLogger()).Build();
MobileCoreInitializer.Init(app, options);
```


### HTTP module

HTTP layer provides common wrapper for making any networking requests. By default it uses .NET `System.Net` API.
It uses `async/await` paradigm.

#### Usage

```C#
public async Task<List<User>> GetUsers()  {
   var httpLayer = MobileCore.Instance.HttpLayer;
   var request = httpLayer.NewRequest();
   var response = await request.Get("https://jsonplaceholder.typicode.com/users").Execute();
   if (response.Successful) {
       return JsonConvert.DeserializeObject<List<User>>(response.Body);
   } else {
       MobileCore.Instance.Logger.Error($"failed to get users from {url}", response.Error);
       return null;
   }
}
```



