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
using Example.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Example.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerMenuPage : ContentPage
    {
        public ListView ListView;

        public DrawerMenuPage()
        {
            InitializeComponent();

            BindingContext = new RootPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class RootPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<NavigationItem> NavigationItems { get; set; }

            public RootPageMasterViewModel()
            {
                NavigationItems = new ObservableCollection<NavigationItem>(new[]
                {
                    NavigationItem.HOME_PAGE,
                    new NavigationItem { Id = 1, Title = StringResources.NavHttp, TargetType=typeof(HttpPage),Icon=ResourceUtils.GetSvg("ic_http") },                  
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}