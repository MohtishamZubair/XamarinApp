using SalesAgentDistribution.Factory;
using SalesAgentDistribution.Model;
using SalesAgentDistribution.Service;
using SalesAgentDistribution.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SalesAgentDistribution.ViewModel
{
    class PendingTasksVM : INotifyPropertyChanged
    {
        //internal static readonly PendingTasksVM Instance = new PendingTasksVM();
       IDBHandler<BGActivity> _dbHandler;
       IServiceQueue _queueService;

        internal PendingTasksVM()
        { }

        public string Subheading
        {
            get
            {
                return "All pending tasks";
            }
        }

        public List<BGActivity> PendingActivities
        {
            get
            {
                List<BGActivity> activities = null;
                using (_dbHandler=SADFactory.GetDBHandler<BGActivity>())
                {
                    activities = _dbHandler.GetAll();
                }
                return activities;
            }
        }
        public Command CompleteActivityAsync
        {
            get
            {
                return new Command<BGActivity>(async (a) =>
                {
                    using (_queueService = SADFactory.GetServiceQueue())
                    {
                        bool result = await _queueService.CompleteActivity(a);
                        if (result)
                        {
                            await new ContentPage().DisplayAlert("Background Activity", string.Format("Background action of {0} for {1} has been completed!", a.ActivityActionName, a.ActivityId), "OK");                            
                        }
                        else
                        {
                            await new ContentPage().DisplayAlert("Background Activity", string.Format("Background action of {0} for {1} has been failed!", a.ActivityActionName, a.ActivityId), "OK");
                        }
                        OnPropertyChanged(nameof(PendingActivities));
                    }
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}