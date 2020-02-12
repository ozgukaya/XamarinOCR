using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App4.Models;

namespace App4.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage(string texts)
        {
            InitializeComponent();

            Item = new Item
            {
                Title = "Note Name",
                Text = texts
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {

            Item.Id = Guid.NewGuid().ToString();
            await App.Database.SaveNoteAsync(Item);

            MessagingCenter.Send(this, "AddItem", Item);
            
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}