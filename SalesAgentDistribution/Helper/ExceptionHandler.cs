using SalesAgentDistribution.Factory;
using SalesAgentDistribution.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SalesAgentDistribution.Helper
{
    public static class ExceptionHandler
    {
        public static void HandleException(this Exception ex)
        {
            if (App.Current.Properties.Keys.Contains(AppHelper.ON_GOING_ACTIVITY))
            {
                var OnGoingActivity = App.Current.Properties[AppHelper.ON_GOING_ACTIVITY] as BGActivity;
                if (OnGoingActivity != null)
                {
                    using (var _serviceQueue = SADFactory.GetServiceQueue())
                    {
                        bool isSaved = _serviceQueue.SaveActivity(OnGoingActivity);
                        App.Current.Properties.Remove(AppHelper.ON_GOING_ACTIVITY);
                    }
                } 
            }
        }
    }
}
