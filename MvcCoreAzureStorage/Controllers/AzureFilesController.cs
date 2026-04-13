using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureStorage.Services;

namespace MvcCoreAzureStorage.Controllers
{
    public class AzureFilesController : Controller
    {
        private ServiceStorageFile _service;
        public AzureFilesController(ServiceStorageFile service)
        {
            this._service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<string> files = await this._service.GetFilesAsync();
            return View(files);
        }

        public async Task<IActionResult> ReadFile(string filename)
        {
            string data = await this._service.ReadFileAsync(filename);
            ViewData["DATA"] = data;
            return View();
        }

        public async Task<IActionResult> DeleteFile(string filename)
        {
            await this._service.DeleteFileAsync(filename);
            return RedirectToAction("Index");
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string fileName = file.FileName;
            //DEBERIAMOS TENER UN HELPER QUE SE ENCARGUE
            //DE "LIMPIAR" EL FILENAME
            using (Stream stream = file.OpenReadStream())
            {
                await this._service.UploadFileAsync(fileName, stream);
            }
            ViewData["MENSAJE"] = "Fichero subido, OK?";
            return View();
        }
    }
}
