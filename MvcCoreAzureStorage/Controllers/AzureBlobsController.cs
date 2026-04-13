using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureStorage.Models;
using MvcCoreAzureStorage.Services;

namespace MvcCoreAzureStorage.Controllers
{
    public class AzureBlobsController : Controller
    {
        private ServiceStorageBlobs _service;
        public AzureBlobsController(ServiceStorageBlobs service)
        {
            this._service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<string> containers = await this._service.GetContainersAsync();
            return View(containers);
        }

        public IActionResult CreateContainer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer(string containerName)
        {
            await this._service.CreateContainerAsync(containerName);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeteleContainer(string containerName)
        {
            await this._service.DeleteContainerAsync(containerName);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListBlobs(string containerName)
        {
            List<BlobModel> models = await this._service.GetBlobsAsync(containerName);
            return View(models);
        }

        public IActionResult UpdateBlob(string containerName)
        {
            ViewData["CONTAINER"] = containerName;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBlob(string containerName, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this._service.UploadBlobAsync(containerName, blobName, stream);
            }
            return RedirectToAction("ListBlobs", new { containerName = containerName });
        }

        public async Task<IActionResult> DeleteBlob(string containerName, string blobName)
        {
            await this._service.DeleteBlobAsync(containerName, blobName);
            return RedirectToAction("ListBlobs", new { containerName = containerName });
        }


    }
}
