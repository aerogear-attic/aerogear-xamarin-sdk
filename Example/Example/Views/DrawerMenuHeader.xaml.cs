using System;
using System.Collections.Generic;
using AeroGear.Mobile.Core;
using AeroGear.Mobile.Core.Logging;
using Example.Models;
using Example.Resources;
using Example.Utils;
using Xamarin.Forms;

namespace Example.Views
{
    public partial class DrawerMenuHeader : ContentView
    {
        public DrawerMenuHeader()
        {
            InitializeComponent();           
            BindingContext = new Header(StringResources.AeroGear,StringResources.AppName, ImageSource.FromResource("Example.Resources.aerogear_icon.png"));
        }

    }
}
