using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Views
{

    public class RootPageMenuItem
    {
        public RootPageMenuItem()
        {
            TargetType = typeof(Pages.HomePage);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}