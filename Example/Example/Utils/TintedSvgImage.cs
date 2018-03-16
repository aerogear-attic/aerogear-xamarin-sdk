using System;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;

namespace Example.Utils
{
    public class TintedSvgImage:FFImageLoading.Svg.Forms.SvgCachedImage
    {
        public static BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintedSvgImage), Color.Transparent, propertyChanged: UpdateColor);

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        private static void UpdateColor(BindableObject bindable, object oldColor, object newColor)
        {
            var oldcolor = (Color)oldColor;
            var newcolor = (Color)newColor;

            if (!oldcolor.Equals(newcolor))
            {
                var view = (TintedSvgImage)bindable;
                var transformations = new System.Collections.Generic.List<ITransformation>() {
                    new TintTransformation((int)(newcolor.R * 255), (int)(newcolor.G * 255), (int)(newcolor.B * 255), (int)(newcolor.A * 255)) {
                        EnableSolidColor = true
                    }
                };
                view.Transformations = transformations;
            }
        }
    }
}

