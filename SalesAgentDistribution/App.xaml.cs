using SalesAgentDistribution.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SalesAgentDistribution
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var tp = new TabbedPage();
            var np = new NavigationPage(new SalesAgentDistribution.View.MainPage());
            np.Title = "Agents Work Deliverables";            
            tp.Children.Add(np);
            tp.Children.Add(new SalesAgentDistribution.View.PendingTasks());
           
            MainPage = tp;
            tp.CurrentPageChanged += Tp_CurrentPageChanged;            
        }

        private void Tp_CurrentPageChanged(object sender,  EventArgs e)
        {
            if (((TabbedPage)sender).CurrentPage is View.PendingTasks)
            {
                var pt = ((TabbedPage)sender).CurrentPage;
                pt.BindingContext = new PendingTasksVM();
            }          
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Debug.WriteLine("OnStart from pipeline");
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Debug.WriteLine("OnSleep from pipeline");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            Debug.WriteLine("OnResume from pipeline");
        }
       
    }
}
