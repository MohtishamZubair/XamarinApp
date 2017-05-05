using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SalesAgentDistribution.Controls
{
    public class SADExtendedLabel : Label
    {        
    }

    public class SADExtendedLV : ListView
    {
        public Command ItemSelectTap { get; set; }
    }

}
