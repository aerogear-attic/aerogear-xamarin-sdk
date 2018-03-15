using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Views
{
    public class MenuItem
    {
        public MenuItem()
        {
            TargetType = typeof(Pages.HomePage);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}