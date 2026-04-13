using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MvcCoreAzureStorage.Models;

namespace MvcCoreAzureStorage.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;

        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        public async Task<List<string>> GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await foreach (BlobContainerItem container in this.client.GetBlobContainersAsync())
            {
                containers.Add(container.Name);
            }
            return containers;
        }

        public async Task CreateContainerAsync(string containerName)
        {
            await this.client.CreateBlobContainerAsync(containerName.ToLower(), PublicAccessType.Blob);
        }

        //ELIMINAR UN CONTAINER
        public async Task DeleteContainerAsync(string containerName)
        {
            await this.client.DeleteBlobContainerAsync(containerName);
        }

        //LISTADO DE FICHEROS DENTRO DE UN CONTAINER
        public async Task<List<BlobModel>> GetBlobsAsync(string containerName)
        {
            //NECESITAMOS UN CLIENTE DE BLOBS CONTAINER 
            //PARA EL ACCESO A LOS FICHEROS
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            List<BlobModel> models = new List<BlobModel>();
            await foreach (BlobItem item in containerClient.GetBlobsAsync())
            {
                BlobClient blobClient = containerClient.GetBlobClient(item.Name);
                BlobModel blob = new BlobModel();
                blob.Nombre = item.Name;
                blob.Container = containerName;
                blob.Url = blobClient.Uri.AbsoluteUri;
                models.Add(blob);
            }
            return models;
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            BlobContainerClient blobContainerClient = this.client.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteBlobAsync(blobName);
        }

        //SUBIR UN BLOB A UN CONTAINER
        public async Task UploadBlobAsync(string containerName, string blobName, Stream stream)
        {
            BlobContainerClient blobContainerClient = this.client.GetBlobContainerClient(containerName);
            await blobContainerClient.UploadBlobAsync(blobName, stream);
        }
    }
}
