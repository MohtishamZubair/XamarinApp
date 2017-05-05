using System;
using SalesAgentDistribution.Model;
using SalesAgentDistribution.ViewModel;
using Xamarin.Forms;
using System.Collections.Generic;

namespace SalesAgentDistribution.View
{
    internal class EditDelivery : ContentPage
    {
        private string _selectedStatus;
        string[] Status;

        //private Delivery deliveryToEdit;

        public EditDelivery(DeliveryVM dvm)
        {
            //this.deliveryToEdit = deliveryToEdit;
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = dvm;
            dvm.UpdateCommandCompleteResponse = ShowAlert;
            dvm.ChangeDelivery.Status = DeliveryVM.GetStatusIndex(dvm.ChangeDelivery.Status);
            Status = DeliveryVM.DeliveryStatus;
            DrawLayout();
        }

        private void DrawLayout()
        {
            var content = new StackLayout
            {
                //BindingContext = $"{Binding ChangeDelivery}",
                VerticalOptions = LayoutOptions.Start,                
                Children = {
                    new Label {
                            HorizontalOptions=LayoutOptions.FillAndExpand,
                            HorizontalTextAlignment = TextAlignment.Start,
                            Text = "Edit Delivery",
                            FontSize = 30,
                            TextColor = Color.White,
                            BackgroundColor = Color.Black,
                        },
                    new Label {
                            HorizontalOptions=LayoutOptions.FillAndExpand,
                            Text = "",
                            FontSize = 40,
                        },
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Start,
                            Text = "Please enter any note"
                        },
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Start,
                            Text = "Set status"
                        },
                        new Button {
                            Text = "Update Delivery",
                            HorizontalOptions = LayoutOptions.Center,
                        }                        ,

                    }
            };

            var button = content.Children[4] as Button;
            button.SetBinding(Button.CommandProperty, nameof(DeliveryVM.UpdateDelivery));
           
            var entryNotes = new Entry();
            entryNotes.SetBinding(Entry.TextProperty, "ChangeDelivery.Notes");
            content.Children.Insert(3, entryNotes);         


            Picker picker = new Picker
            {
                Title = "Status",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            foreach (var status in Status)
            {
                picker.Items.Add(status);
            }

            picker.SetBinding(Picker.SelectedIndexProperty, "ChangeDelivery.Status");
            //BindableProperty bp = BindableProperty.Create("ChangeDelivery.Status", typeof(string), typeof(string), "0");
            //picker.SetValue(bp, _selectedStatus);

            picker.SelectedIndexChanged += (sender, args) =>
            {
                var vm = picker.BindingContext as DeliveryVM;
                vm.ChangeDelivery.Status = DeliveryVM.DeliveryStatus[picker.SelectedIndex != -1 ? picker.SelectedIndex : 0];
            };


            content.Children.Insert(5, picker);

            var messageLabel = new Label
            {
                HeightRequest = 30,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Green,
                LineBreakMode= LineBreakMode.WordWrap
            };

            messageLabel.SetBinding(Label.TextProperty, "AlertMessage");
            var backbutton = new Button
            {
                Text = "Back",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End
            };

            backbutton.Clicked += Backbutton_Clicked;

            content.Children.Add(messageLabel);
            content.Children.Add(backbutton);

            Content = content;
        }

        private void Backbutton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        public void ShowAlert(string msg, bool flag)
        {
            DisplayAlert("Delivery Edit",msg,"OK");
            //ContentPage page = new ContentPage();
            //page.DisplayAlert("Delivery Edit Output", msg, "OK");
        }
    }
}