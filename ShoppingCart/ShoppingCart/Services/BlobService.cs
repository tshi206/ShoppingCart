using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ShoppingCart.Models;
using Xamarin.Forms.Internals;

namespace ShoppingCart.Services
{
    public class BlobService
    {

        public static BlobService AzureBlobService { get; } = new BlobService();

        private const string _connectionString = "DefaultEndpointsProtocol=https;AccountName=xamarinshoppingcart;AccountKey=ydDuO/ZLq7ErRiKzixBiu4y0seN7ZtqR9SslDvFSNuUni+118WAHNfJDeZNMAMBc+RrcLXJmdAByAok0gXBn1Q==;EndpointSuffix=core.windows.net";

        private CloudBlobClient _blobClient = CloudStorageAccount.Parse(_connectionString).CreateCloudBlobClient();

        private CloudBlobContainer _imgBlobContainer;

        private BlobService()
        {
            _imgBlobContainer = _blobClient.GetContainerReference("itemimg");
        }

        public async Task<List<Uri>> GetAllBlobUrisAsync()
        {
            var contToken = new BlobContinuationToken();
            var allBlobs = await _imgBlobContainer.ListBlobsSegmentedAsync(contToken).ConfigureAwait(false);

            foreach (IListBlobItem blobItem in allBlobs.Results)
            {
                Debug.WriteLine(blobItem.StorageUri);
                Debug.WriteLine(blobItem.Uri);
                Debug.WriteLine(blobItem.ToString());
            }

            var uris = allBlobs.Results.Select(b => b.Uri).ToList();
            return uris;
        }

        public async Task UploadFileAsync(string localPath, Item item)
        {
            // remove the first '/'
            localPath = localPath.Substring(1);

            var blobRef = _imgBlobContainer.GetBlockBlobReference(localPath);

            Debug.WriteLine("BlobType : " + blobRef.BlobType);
            blobRef.Metadata.Keys.ForEach(k =>
            {
                Debug.WriteLine("key : " + k);
            });
            blobRef.Metadata.Values.ForEach(v =>
            {
                Debug.WriteLine(v);
            });
            Debug.WriteLine(blobRef.Name);
            Debug.WriteLine(blobRef.Properties);
            Debug.WriteLine(blobRef.Uri);
            Debug.WriteLine(blobRef.StorageUri);

            item.SourcePath = blobRef.Uri.AbsoluteUri;

            await blobRef.UploadFromFileAsync(localPath);
        }

    }
}
