using SalesAgentDistribution.Controls;
using SalesAgentDistribution.Droid.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SADExtendedLabel), typeof(SADExtendedLabelRenderer))]
namespace SalesAgentDistribution.Droid.Controls
{
    public class SADExtendedLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            Control.SetMaxLines(500);
        }
    }
}



