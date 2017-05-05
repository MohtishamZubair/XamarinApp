using System;
using SalesAgentDistribution.Model;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using SalesAgentDistribution.Helper;
using System.Linq;
using SalesAgentDistribution.Factory;
using SalesAgentDistribution.ViewModel;

namespace SalesAgentDistribution.Service
{
    public interface IServiceQueue : IDisposable
    {
        bool SaveActivity(BGActivity bGActivity);
      //  Task RunPendingActivitiesJob(CancellationToken token);
        Task<bool> CompleteActivity(BGActivity bgActivity);
    }

    public class ServiceQueue : IServiceQueue
    {
        private  IDBHandler<BGActivity> _dbHandler;

        public bool SaveActivity(BGActivity bGActivity)
        {
            int rowsEffected = -1;
            using (_dbHandler = SADFactory.GetDBHandler<BGActivity>())
            {

                var existingActivitys = _dbHandler.GetAll(a => a.TargetObjectId == bGActivity.TargetObjectId);

                if (existingActivitys.Count > 0)
                {
                    var existingActivity = existingActivitys.First();
                    _dbHandler.DeleteItem(existingActivity.ActivityId);
                }
                rowsEffected = _dbHandler.Add(bGActivity);

            }
            return rowsEffected != -1;
        }
        /// need to check this for dispose call
        //public async Task RunPendingActivitiesJob(CancellationToken token)
        //{
        //    await Task.Run(async () =>
        //    {
        //        var pendingActivities = _dbHandler.GetAll();

        //        foreach (var bgActivity in pendingActivities)
        //        {
        //            await CompleteActivity(bgActivity);
        //            Device.BeginInvokeOnMainThread(() =>
        //            {
        //                MessagingCenter.Send<BGActivity>(bgActivity, AppHelper.BACKGROUND_ACTIVITY_MESSAAGE);
        //            });
        //        }
        //    }, token);
        //}

        public async Task<bool> CompleteActivity(BGActivity bgActivity)
        {
            IRestHandler<BGActivity> _restHandler = new RestHandler<BGActivity>(bgActivity.ServiceURL);
            var result = await _restHandler.TriggerPUT(bgActivity.TargetObjectId, bgActivity.TargetObjectJSON);
            if (result)
            {
                DeliveryVM.Instance.RefreshDeliveries();
                using (_dbHandler = SADFactory.GetDBHandler<BGActivity>())
                {
                    _dbHandler.DeleteItem(bgActivity.ActivityId);
                }                
            }
            return result;
        }

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                if (_dbHandler != null)
                {
                    _dbHandler.Dispose();
                }
            }
            disposed = true;
        }
    }
}