using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Example.Resources;
using Example.Utils;
using Xamarin.Forms;

namespace Example.Models
{
    public class NavigationItem : INotifyPropertyChanged
    {

        public static NavigationItem HOME_PAGE
        {
            get
            {
                return _HOME_PAGE.Value;
            }
        }

        private static Lazy<NavigationItem> _HOME_PAGE = new Lazy<NavigationItem>(() =>
        {
            var item = new NavigationItem();
            item.Id = 0;
            item.Title = StringResources.NavHome;
            item.Icon = ResourceUtils.GetSvg("ic_home");
            item.Selected = true;
            return item;
        });


        public NavigationItem()
        {
            TargetType = typeof(Views.Pages.HomePage);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Selected
        {
            get => selected;
            set
            {
                if (selected != value)
                {
                    selected = value;
                    OnPropertyChanged("Selected");
                }
				OnPropertyChanged("SelectedColor");
            }
        }
        private bool selected;

        public Type TargetType { get; set; }

        public Color SelectedColor
        {
            get
            {
                return (Color)Application.Current.Resources[selected ? "Accent" : "PrimaryTextColor"];
            }
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