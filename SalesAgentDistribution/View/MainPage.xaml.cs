using SalesAgentDistribution.Model;
using SalesAgentDistribution.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SalesAgentDistribution.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);                

                if (BindingContext == null)
                {
                    BindingContext = DeliveryVM.GetInstance();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                if (ex.InnerException !=null)
                {
                    var inEx = ex.InnerException;
                    Debug.WriteLine(inEx.Message);
                    Debug.WriteLine(inEx.StackTrace);
                }
             
            }
        }

        protected async void OnEditDelivery(object sender, ItemTappedEventArgs ie)
        {            
            Delivery deliveryToEdit = ie.Item as Delivery;
            DeliveryVM.Instance.ChangeDelivery = deliveryToEdit;
            DeliveryVM.Instance.AlertMessage = string.Empty;

            await Navigation.PushAsync(new EditDelivery(DeliveryVM.Instance),true);
        }

        protected  void OnSelection(object sender, SelectedItemChangedEventArgs ie)
        {
            ((ListView)sender).SelectedItem = null;
        }

            

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height);
        //    Metrics.Instance.Width = width;
        //    Metrics.Instance.Height = height;
        //}
    }
}
