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
        public async Task<IActionResult> CreateContainer(string containername)
        {
            await this._service.CreateContainerAsync(containername);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteContainer(string containername)
        {
            await this._service.DeleteContainerAsync(containername);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListBlobs(string containername)
        {
            List<BlobModel> models = await this._service.GetBlobsAsync(containername);
            return View(models);
        }

        public IActionResult UploadBlob(string containername)
        {
            ViewData["CONTAINER"] = containername;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadBlob(string containername, IFormFile file)
        {
            string blobname = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this._service.UploadBlobAsync(containername, blobname, stream);
            }
            return RedirectToAction("ListBlobs", new { containername = containername });
        }

        public async Task<IActionResult> DeleteBlob(string containername, string blobname)
        {
            await this._service.DeleteBlobAsync(containername, blobname);
            return RedirectToAction("ListBlobs", new { containername = containername });
        }


    }
}
