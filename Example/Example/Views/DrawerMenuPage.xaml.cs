using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Example.Models;
using Example.Resources;
using Example.Utils;
using Example.ViewModels;
using Example.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Example.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenuPage : ContentPage
    {
        public ListView ListView;

        private DrawerMenuVM ViewModel;

        public DrawerMenuPage()
        {
            InitializeComponent();
            ViewModel=new DrawerMenuVM();
            BindingContext = ViewModel;
            ListView = MenuItemsListView;
            Title = StringResources.Menu;
        }
    }
}