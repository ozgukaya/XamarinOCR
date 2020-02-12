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
    public partial class CameraPage : ContentPage
    {
        private MediaFile file;
        public CameraPage()
        {
            InitializeComponent();
            
        }
        async void BtnTakePhoto(Object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported) 
            {
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize=PhotoSize.Medium,
                    Name = Guid.NewGuid().ToString() + ".jpeg"
                });

                if (file == null)
                    return;

                imgPhoto.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
               
            }
        }
        async void BtnOCRAPIUrl(Object sender, EventArgs e)
        {
            ServiceManager manager = new ServiceManager();
            if(file == null)
            {
                await DisplayAlert("Warning", " Take photo before, please try again ", "OK");
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
        async void BtnSave(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage(lblTexts.Text)));
        }

    }
}