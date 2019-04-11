using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServices.Helpers
{
    public class BlobHelper
    {
        //these variables are used throughout the class
        string ContainerName { get; set; }
        CloudBlobContainer cloudBlobContainer { get; set; }

        //this is the only public constructor; can't use this class without this info
        public BlobHelper(string storageAccountName, string storageAccountKey, string containerName)
        {
            cloudBlobContainer = SetUpContainer(storageAccountName, storageAccountKey, containerName);
            ContainerName = containerName;
        }

        /// <summary>
        /// set up references to the container, etc.
        /// </summary>
        private CloudBlobContainer SetUpContainer(string storageAccountName,
          string storageAccountKey, string containerName)
        {
            string connectionString = string.Format(@"DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
            storageAccountName, storageAccountKey);

            //get a reference to the container where you want to put the files
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            return cloudBlobContainer;
        }

        internal string UploadFromByteArray(Byte[] uploadBytes, string targetFileName)
        {
            string status = string.Empty;
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(targetFileName);
            blob.UploadFromByteArrayAsync(uploadBytes, 0, uploadBytes.Length);
            status = "Uploaded byte array successfully.";
            return status;
        }


        internal Byte[] DownloadToByteArray(string targetFileName)
        {
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(targetFileName);
            //you have to fetch the attributes to read the length
            blob.FetchAttributesAsync();
            long fileByteLength = blob.Properties.Length;
            Byte[] myByteArray = new Byte[fileByteLength];
            blob.DownloadToByteArrayAsync(myByteArray, 0);
            return myByteArray;
        }


        //if the blob is there, delete it 
        //check returning value to see if it was there or not
        internal async Task<string> DeleteBlob(string blobName)
        {
            string status = string.Empty;
            CloudBlockBlob blobSource = cloudBlobContainer.GetBlockBlobReference(blobName);
            bool blobExisted = await blobSource.DeleteIfExistsAsync();
            if (blobExisted)
            {
                status = "Blob existed; deleted.";
            }
            else
            {
                status = "Blob did not exist.";
            }
            return status;
        }
    }
}
