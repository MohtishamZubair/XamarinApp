using SalesAgentDistribution.Factory;
using SalesAgentDistribution.Model;
using SalesAgentDistribution.Service;
using SalesAgentDistribution.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SalesAgentDistribution.ViewModel
{
    internal class DeliveryVM : INotifyPropertyChanged
    {
        //public static readonly DeliveryVM Instance = new DeliveryVM();
        public static DeliveryVM Instance = null;

        internal static DeliveryVM GetInstance()
        {
            var lockObject = new object();
            lock (lockObject)
            {
                if (Instance == null)
                {
                    Instance = new DeliveryVM();
                }
            }
            return Instance;
        }

        private List<Delivery> _deliveries;
        internal static string[] DeliveryStatus = new[]{
                "UnPack",
                "Packed",
                "Agent",
                "Ready",
                "OntheWay",
                "Delivered",
            };
        

        internal static string GetStatusIndex(string status)
        {
            return !string.IsNullOrWhiteSpace(status) ? Array.IndexOf(DeliveryStatus, status).ToString():"0";
        }
        
        internal Action<string, bool> UpdateCommandCompleteResponse;

        private DeliveryVM()
        {}

        private string _actionMessage;

        public string AlertMessage
        {
            get { return _actionMessage; }
            set
            {
                _actionMessage = value;
                OnPropertyChanged();
            }
        }

        private string _searchDelivery;

        public string SearchDelivery
        {
            get { return _searchDelivery; }
            set
            {
                _searchDelivery = value;
                OnPropertyChanged(nameof(Deliveries));
            }
        }

        private Delivery deliveryToEdit;

        public Delivery ChangeDelivery
        {
            get { return deliveryToEdit; }
            set
            {
                deliveryToEdit = value;
            }
        }

        public void RefreshDeliveries()
        {
            using (var deliveryService = SADFactory.GetDeliveryService())
            {
                Deliveries = deliveryService.RefreshAll();
            }
        }

        public Command UpdateDelivery
        {
            get
            {
                return new Command(async () =>
                {
                    using (var _service = SADFactory.GetDeliveryService())
                    {

                       // ChangeDelivery.Status = GetStatusText(ChangeDelivery.Status);
                        bool result = await _service.UpdateAsync(ChangeDelivery);

                        if (result)
                        {
                            Deliveries = _service.RefreshAll();
                        }

                        string messageToSent = string.Format("{2}! Updating delivery for id {0} for status {1}", ChangeDelivery.DeliverableId, ChangeDelivery.Status, result ? " Success " : " Error ");
                        AlertMessage = messageToSent;
                        UpdateCommandCompleteResponse?.Invoke(messageToSent, result);
                        //UpdateCommandCompleteResponse = null; 
                    }
                });
            }
        }

      //  public double StackChildSize { get { return Metrics.Instance.Width / 3; } }

        public List<Delivery> Deliveries
        {
            get
            {
               // return new List<Delivery>();
                List<Delivery> deliveries = null;

                using (var _service = SADFactory.GetDeliveryService())
                {
                   deliveries = !string.IsNullOrWhiteSpace(SearchDelivery) ? _service.GetAll().Where(d => d.Notes.Contains(SearchDelivery)).ToList() : _service.GetAll();                    
                }
                return deliveries; 
            }
            set
            {
                OnPropertyChanged();
            }
        }

        private void LoadAllDeliveries()
        {
            using (var _service = SADFactory.GetDeliveryService())
            {
                _deliveries = _service.GetAll();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
