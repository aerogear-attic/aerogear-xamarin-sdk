using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Example.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            InitializeComponent();
            DrawerMenuPage.ListView.ItemSelected += ListView_ItemSelected;
            ChangePage(MenuItem.HOME_PAGE);
        }

        /// <summary>
        /// Changes the current page to the new based by menu item.
        /// </summary>
        /// <param name="item">menu item</param>
        private void ChangePage(MenuItem item) {
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);

            page.Title = StringResources.AppName;

            Detail = new NavigationPage(page);
            IsPresented = false;

            DrawerMenuPage.ListView.SelectedItem = null;
        }


        /// <summary>
        /// Replaces page when item is selected in the drawer menu.
        /// </summary>
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuItem;
            ChangePage(item);
        }
    }
}