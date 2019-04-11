using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.BlobClient
{
    public class BlobClient
    {
        public static class AzureStorageConstants
        {
            public static string Account = "glimpseimages";
            public static string SharedKeyAuthorizationScheme = "SharedKey";
            public static string BlobEndPoint = "https://glimpseimages.blob.core.windows.net/";
            public static string Key = "XHIr8SaKFci88NT8Z+abpJaH1FeLC4Zq6ZRaIkaAJQc+N/1nwTqGPzDLdNZXGqcLNg+mK7ugGW3PyJsYU2gB7w==";
            public static string ContainerName = "imagestorage";
            public static string FileLocation = BlobEndPoint + ContainerName;
        }


        public static async Task<bool> DeleteBlob(string blobName)
        {
            string containerName = AzureStorageConstants.ContainerName;

            string requestMethod = "DELETE";

            const string blobType = "BlockBlob";

            string urlPath = string.Format("{0}/{1}", containerName, blobName);
            string msVersion = "2009-09-19";
            string dateInRfc1123Format = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

            string canonicalizedHeaders = string.Format("x-ms-blob-type:{0}\nx-ms-date:{1}\nx-ms-version:{2}", blobType, dateInRfc1123Format, msVersion);
            string canonicalizedResource = string.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
            string stringToSign = string.Format("{0}\n\n\n{1}\n\n\n\n\n\n\n\n\n{2}\n{3}", requestMethod, 0, canonicalizedHeaders, canonicalizedResource);

            string authorizationHeader = CreateAuthorizationHeader(stringToSign);            

            string uri = AzureStorageConstants.BlobEndPoint + urlPath;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-blob-type", blobType);
            client.DefaultRequestHeaders.Add("x-ms-date", dateInRfc1123Format);
            client.DefaultRequestHeaders.Add("x-ms-version", msVersion);
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

           
            HttpResponseMessage response = await client.DeleteAsync(uri);
            
            if (response.IsSuccessStatusCode == true) return true;
            
            return false;
        }

        public static async Task<string> UploadBlob(string blobName, Byte[] blobContent)
        {
            string containerName = AzureStorageConstants.ContainerName;

            string requestMethod = "PUT";

            //String content = "The Name of This Band is Talking Heads";
            //UTF8Encoding utf8Encoding = new UTF8Encoding();
            //Byte[] blobContent = utf8Encoding.GetBytes(content);
            int blobLength = blobContent.Length;

            const string blobType = "BlockBlob";

            string urlPath = string.Format("{0}/{1}", containerName, blobName);
            string msVersion = "2009-09-19";
            string dateInRfc1123Format = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

            string canonicalizedHeaders = string.Format("x-ms-blob-type:{0}\nx-ms-date:{1}\nx-ms-version:{2}", blobType, dateInRfc1123Format, msVersion);
            string canonicalizedResource = string.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
            string stringToSign = string.Format("{0}\n\n\n{1}\n\n\n\n\n\n\n\n\n{2}\n{3}", requestMethod, blobLength, canonicalizedHeaders, canonicalizedResource);          
            string authorizationHeader = CreateAuthorizationHeader(stringToSign);

            string uri = AzureStorageConstants.BlobEndPoint + urlPath;
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("x-ms-blob-type", blobType);
            client.DefaultRequestHeaders.Add("x-ms-date", dateInRfc1123Format);
            client.DefaultRequestHeaders.Add("x-ms-version", msVersion);
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
           
            HttpContent requestContent = new ByteArrayContent(blobContent);
            HttpResponseMessage response = await client.PutAsync(uri, requestContent);
         
            if (response.IsSuccessStatusCode == true)
            {
                foreach (var aHeader in response.Headers)
                {
                    if (aHeader.Key == "ETag") return aHeader.Value.ElementAt(0);
                }
            }           
            
            return null;
        }



        public static async Task<byte[]> GetBlob(string blobName)
        {
            string containerName = AzureStorageConstants.ContainerName;
            string requestMethod = "GET";

            const string blobType = "BlockBlob";

            string urlPath = string.Format("{0}/{1}", containerName, blobName);
            string msVersion = "2009-09-19";
            string dateInRfc1123Format = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

            string canonicalizedHeaders = string.Format("x-ms-blob-type:{0}\nx-ms-date:{1}\nx-ms-version:{2}", blobType, dateInRfc1123Format, msVersion);
            string canonicalizedResource = string.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
            string stringToSign = string.Format("{0}\n\n\n\n\n\n\n\n\n\n\n\n{2}\n{3}", requestMethod, 0, canonicalizedHeaders, canonicalizedResource);          
            string authorizationHeader = CreateAuthorizationHeader(stringToSign);           

            string uri = AzureStorageConstants.BlobEndPoint + urlPath;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-blob-type", blobType);
            client.DefaultRequestHeaders.Add("x-ms-date", dateInRfc1123Format);
            client.DefaultRequestHeaders.Add("x-ms-version", msVersion);
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode == true)
            {
                return response.Content.ReadAsByteArrayAsync().Result;
            }

            return null;
        }

        //If you need to wait for the result, use this method
        public static byte[] GetBlobSynchronous(string blobName)
        {
            string containerName = AzureStorageConstants.ContainerName;
            string requestMethod = "GET";

            const string blobType = "BlockBlob";

            string urlPath = string.Format("{0}/{1}", containerName, blobName);
            string msVersion = "2009-09-19";
            string dateInRfc1123Format = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

            string canonicalizedHeaders = string.Format("x-ms-blob-type:{0}\nx-ms-date:{1}\nx-ms-version:{2}", blobType, dateInRfc1123Format, msVersion);
            string canonicalizedResource = string.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
            string stringToSign = string.Format("{0}\n\n\n\n\n\n\n\n\n\n\n\n{2}\n{3}", requestMethod, 0, canonicalizedHeaders, canonicalizedResource);
            string authorizationHeader = CreateAuthorizationHeader(stringToSign);

            string uri = AzureStorageConstants.BlobEndPoint + urlPath;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-blob-type", blobType);
            client.DefaultRequestHeaders.Add("x-ms-date", dateInRfc1123Format);
            client.DefaultRequestHeaders.Add("x-ms-version", msVersion);
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

            HttpResponseMessage response = client.GetAsync(uri).Result;

            if (response.IsSuccessStatusCode == true)
            {
                return response.Content.ReadAsByteArrayAsync().Result;
            }

            return null;
        }


        public static async Task<List<byte[]>> GetAllBlobs()
        {
            string containerName = AzureStorageConstants.ContainerName;
            string requestMethod = "GET";

           // const string blobType = "BlockBlob";

            string urlPath = string.Format("{0}{1}", containerName, "?restype=container&comp=list");
            string msVersion = "2009-09-19";
            string dateInRfc1123Format = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

            string canonicalizedHeaders = string.Format("x-ms-date:{0}\nx-ms-version:{1}", dateInRfc1123Format, msVersion);
            string canonicalizedResource = string.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
            string stringToSign = string.Format("{0}\n\n\n\n\n\n\n\n\n\n\n\n{2}\n{3}", requestMethod, 0, canonicalizedHeaders, canonicalizedResource);
            string authorizationHeader = CreateAuthorizationHeader(stringToSign);

            string uri = AzureStorageConstants.BlobEndPoint + urlPath;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-date", dateInRfc1123Format);
            client.DefaultRequestHeaders.Add("x-ms-version", msVersion);
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode == true)
            {
                return response.Content.ReadAsAsync<List<byte[]>>().Result;
            }

            return null;
        }


        private static string CreateAuthorizationHeader(string canonicalizedString)
        {
            if (string.IsNullOrEmpty(canonicalizedString))
            {
                throw new ArgumentNullException("canonicalizedString");
            }

            string signature = CreateHmacSignature(canonicalizedString, Convert.FromBase64String(AzureStorageConstants.Key));
            string authorizationHeader = string.Format(CultureInfo.InvariantCulture, "{0} {1}:{2}", AzureStorageConstants.SharedKeyAuthorizationScheme, AzureStorageConstants.Account, signature);

            return authorizationHeader;
        }

        private static string CreateHmacSignature(string unsignedString, Byte[] key)
        {
            if (string.IsNullOrEmpty(unsignedString))
            {
                throw new ArgumentNullException("unsignedString");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Byte[] dataToHmac = System.Text.Encoding.UTF8.GetBytes(unsignedString);
            using (HMACSHA256 hmacSha256 = new HMACSHA256(key))
            {
                return Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
            }
        }   
    }
}
