using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BEAssistant;
using BEAssistant.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRendererAndroid))]
namespace BEAssistant.Droid
{
    [Obsolete]
    public class RoundedEntryRendererAndroid: EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(e.OldElement == null)
            {

                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(60f);
                gradientDrawable.SetStroke(5, Android.Graphics.Color.LightGray);
                gradientDrawable.SetColor(Android.Graphics.Color.White);
                Control.SetBackground(gradientDrawable);

                Control.SetPadding(50, 10, 50, 10);
            }
        }
    }
}