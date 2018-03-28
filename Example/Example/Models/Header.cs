using System;
using Xamarin.Forms;

namespace Example.Models
{
    /// <summary>
    /// Class for holding drawer menu header data.
    /// </summary>
    public class Header
    {


        public String Line1 { get; set; }
        public String Line2 { get; set; }
        public ImageSource Image { get; set; }
       
        public Header(String line1, String line2, ImageSource image)
        {
            Line1 = line1;
            Line2 = line2;
            Image = image;
        }
    }
}
