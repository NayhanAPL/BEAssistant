using BEAssistant;
using BEAssistant.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(RoundedEntry), typeof(EntryRenderesIO))]
namespace BEAssistant.iOS
{
    public class EntryRenderesIO : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Layer.CornerRadius = 5;
                Control.Layer.BorderWidth = 2;
                Control.Layer.BorderColor = Color.LightGray.ToCGColor();
                Control.Layer.BackgroundColor = Color.White.ToCGColor();
                Control.TextColor = Color.Green.ToUIColor();
                Control.Placeholder = "Escribir Aquí";

            }
        }
    }
}