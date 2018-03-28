using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Example.Models;
using Example.Resources;
using Example.Utils;
using Example.Views.Pages;

namespace Example.ViewModels
{
    public class DrawerMenuVM : INotifyPropertyChanged
    {
        public ObservableCollection<NavigationItem> NavigationItems { get; set; }

        public DrawerMenuVM()
        {
            NavigationItems = new ObservableCollection<NavigationItem>(new[]
            {
                    NavigationItem.HOME_PAGE,
                    new NavigationItem { Id = 1, Title = StringResources.NavHttp, TargetType=typeof(HttpPage),Icon=ResourceUtils.GetSvg("ic_http") },
                    new NavigationItem { Id = 2, Title = StringResources.NavAuth, TargetType=typeof(AuthPage),Icon=ResourceUtils.GetSvg("ic_auth") },
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
