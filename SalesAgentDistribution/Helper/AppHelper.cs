using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SalesAgentDistribution.Helper
{
    public class AppHelper
    {
        public const string DB_FILE_NAME = "Delivery_app.db3";
        public const string JSON_CONTENT = "application/json";
        internal const string BACKGROUND_ACTIVITY_MESSAAGE = "BackgroundActivityMessage";
        internal const string ON_GOING_ACTIVITY = "OnGoingActivity";
        public const string CURRENT_UNHANDLE_EXCEPTION = "CurrentDomainOnUnhandledException";
    }
}
