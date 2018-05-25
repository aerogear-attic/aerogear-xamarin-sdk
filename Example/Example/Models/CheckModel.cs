using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Example.Models
{
    class CheckModel
    {
        public string name { get; set; }
        public Boolean beenRun { get; set; } = false;
        public Boolean passed { get; set; } = false;
        public Color color { get { return passed ? Color.Red : Color.Green; } }
    }
}
