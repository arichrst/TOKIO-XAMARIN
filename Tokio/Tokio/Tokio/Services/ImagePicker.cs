using System;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Tokio.Services
{
    public static class ImagePicker
    {
        private static async Task<MediaFile> Gallery()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    CompressionQuality = 50
                });
                if (file == null)
                    return null;
                return file;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast(ex.Message);
                return null;
            }
        }

        private static async Task<MediaFile> Camera()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                var file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    AllowCropping = true,
                    CompressionQuality = 50,
                    DefaultCamera = CameraDevice.Front,
                    Directory = "Sample",
                    Name = "tokio.png"
                });
                if (file == null)
                    return null;
                return file;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast(ex.Message);
                return null;
            }
        }

        public static async Task PickImage(Action<MediaFile> func)
        {
            var config = new ActionSheetConfig
            {
                Cancel = new ActionSheetOption("Batalkan"),
                Title = "Pilih sumber gambar"
            };
            config.Add("Galeri", new Action(async () =>
            {
                func(await Gallery());
            }));

            config.Add("Kamera", new Action(async () =>
            {
                func(await Camera());
            }));

            UserDialogs.Instance.ActionSheet(config);
            
        }
    }
}
