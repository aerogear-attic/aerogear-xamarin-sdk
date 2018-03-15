using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Resources;

namespace Example.Views
{
    public class MenuItem
    {

        public static MenuItem HOME_PAGE {
            get{
                return _HOME_PAGE.Value;
            }
        }

        private static Lazy<MenuItem> _HOME_PAGE = new Lazy<MenuItem>(() =>
        {
            var item = new MenuItem();
            item.Id = 0;
            item.Title = StringResources.NavHome;
            return item;
        });

        public MenuItem()
        {
            TargetType = typeof(Pages.HomePage);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}