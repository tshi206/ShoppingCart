using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using ShoppingCart.iOS;
using ShoppingCart.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PicturePickerImplementation))]
namespace ShoppingCart.iOS
{
    public class PicturePickerImplementation : IPicturePicker, ILocalUserFolderLocator
    {
        TaskCompletionSource<Stream> taskCompletionSource;
        UIImagePickerController imagePicker;

        public Task<Stream> GetImageStreamAsync()
        {
            // Create and define UIImagePickerController
            imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
            };

            // Set event handlers
            imagePicker.FinishedPickingMedia += OnImagePickerFinishedPickingMedia;
            imagePicker.Canceled += OnImagePickerCancelled;

            // Present UIImagePickerController;
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.PresentModalViewController(imagePicker, true);

            // Return Task object
            taskCompletionSource = new TaskCompletionSource<Stream>();
            return taskCompletionSource.Task;
        }

        void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs args)
        {
            UIImage image = args.EditedImage ?? args.OriginalImage;

            if (image != null)
            {
                // Convert UIImage to .NET Stream object
                NSData data = image.AsJPEG(1);
                Stream stream = data.AsStream();

                // Set the Stream as the completion of the Task
                taskCompletionSource.SetResult(stream);
            }
            else
            {
                taskCompletionSource.SetResult(null);
            }
            imagePicker.DismissModalViewController(true);
        }

        void OnImagePickerCancelled(object sender, EventArgs args)
        {
            taskCompletionSource.SetResult(null);
            imagePicker.DismissModalViewController(true);
        }

        public string GetPathToLocalImageDir(string filename, Stream stream)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string resFolder = Path.Combine(docFolder, "..", "Resources", "Img");

            if (!Directory.Exists(resFolder))
            {
                Directory.CreateDirectory(resFolder);
            }

            string path = Path.Combine(resFolder, filename + ".jpg");

            // This is where we copy the img to program-accessible folder
            if (!File.Exists(path))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    using (var binaryWriter = new BinaryWriter(new FileStream(path, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            binaryWriter.Write(buffer, 0, length);
                        }
                    }
                }
            }

            return path;
        }
    }
}