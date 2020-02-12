using App4.Models;
using App4.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPage
    {
        private MediaFile file;
        public GalleryPage()
        {
            InitializeComponent();
        }

        async void BtnSelectGallery(Object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not supported", "Your device does not currently support this functionality", "Ok");
                return;
            }
            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            file = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (file == null)
            {
                await DisplayAlert("Error", "Could not get the image, please try again", "Ok");
                return;
            }
            
            
            imgPhoto.Source = ImageSource.FromStream(() => 
            {
                var stream = file.GetStream();
                return stream;
            }
                );


        }

        async void BtnOCRAPIUrl(Object sender, EventArgs e)
        {
            ServiceManager manager = new ServiceManager();
             if (file == null)
                {
                    await DisplayAlert("Warning", "First upload an image, please try again", "Ok");
                    return;
                }
            var stream = file.GetStream();
           
            var result = await manager.GetTextListWithCamera(stream);

            lblTexts.Text = result;
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
        async void BtnSave(object sender , EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(lblTexts.Text)));
        }
    }
    
}