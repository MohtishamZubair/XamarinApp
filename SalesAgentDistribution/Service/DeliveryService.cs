using System;
using System.Collections.Generic;
using SalesAgentDistribution.Model;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SalesAgentDistribution.Service
{
    interface IDeliveryService:IDisposable
    {
        List<Delivery> GetAll();
        List<Delivery> RefreshAll();
        Task<bool> UpdateAsync(Delivery changeDelivery);
    }

    class DeliveryService //: IDeliveryService
    {
        public bool Update(Delivery changeDelivery)
        {
            throw new NotImplementedException();
        }

        List<Delivery> GetAll()
        {
            return Enumerable.Range(1, 10).Select(d =>
            new Delivery
            {
                DeliverableId = d,
                InvoiceId = d,
                AgentId = d.ToString(),
                AddressId = d,
                DeliveryTime = DateTime.Now.AddMinutes(d),
                Notes=string.Format("Devlivery No {0} dipatched {1}", d, DateTime.Now.AddMinutes(d)),
                Status="Packed"           
            }).ToList();
        }
    }

    class DeliveryServiceRemote : IDeliveryService
    {
        private bool disposed = false;
        const string _serviceURL = "http://taskforce.somee.com/api/api/deliveries";

        private readonly IRestHandler<Delivery> _remoteHandler = new RestHandler<Delivery>(_serviceURL);
        private readonly IDBHandler<Delivery> _dbHandler = new DBHandler<Delivery>();

        List<Delivery> IDeliveryService.GetAll()
        {
            List<Delivery> deliveries = (this as IDeliveryService).RefreshAll();
            if (deliveries == null || deliveries.Count ==0)
            {
                deliveries = _dbHandler.GetAll();

            }
            return deliveries ;
        }

        List<Delivery> IDeliveryService.RefreshAll()
        {            
            List<Delivery> deliveries = _remoteHandler.GetAll();
            if (deliveries != null && deliveries.Count >0 )
            {
                _dbHandler.DeleteAll();
                _dbHandler.Add(deliveries);
            }
            return deliveries ;
        }


        public async Task<bool> UpdateAsync(Delivery changeDelivery)
        {
            var result = await  _remoteHandler.UpdateAsync(changeDelivery.DeliverableId, changeDelivery);
            return result;
        }

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
                if (_remoteHandler != null)
                {
                    _remoteHandler.Dispose();
                }
            }
            disposed = true;
        }


    }
}