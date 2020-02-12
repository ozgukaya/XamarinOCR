using App4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UrlPage : ContentPage
    {
        public UrlPage()
        {
            InitializeComponent();
        }

        async void BtnOCRAPIUrl(Object sender, EventArgs e)
        {
            ServiceManager manager = new ServiceManager();

            if (entUrl.Text == null)
            {
                await DisplayAlert("Warning", "Please enter an link and try again.", "Ok");
                return;
            }
            var result =await manager.GetTextListWithUrl(new Models.Request.UrlRequest { url = entUrl.Text });

            lblTexts.Text = result.ToString();
        }
        async void BtnCopyClicked(Object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(lblTexts.Text);

            if (Clipboard.HasText)
            {
                var text = await Clipboard.GetTextAsync();
                await DisplayAlert("Success", string.Format("Text is copied to clipboard", text), "OK");
            }
        }
        async void Clicked_History(Object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ItemsPage()));
        }
        async void BtnSave(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(lblTexts.Text)));
        }
    }
}