using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureStorage.Models;
using MvcCoreAzureStorage.Services;

namespace MvcCoreAzureStorage.Controllers
{
    public class AzureTableController : Controller
    {
        private ServiceStorageTables _service;

        public AzureTableController(ServiceStorageTables service)
        {
            this._service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Cliente> clientes = await this._service.GetClientesAsync();
            return View(clientes);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string empresa)
        {
            List<Cliente> clientes = await this._service.GetClientesEmpresaAsync(empresa);
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            await this._service.CreateClientAsync(cliente.IdCliente, cliente.Nombre, cliente.Empresa
                , cliente.Edad, cliente.Salario);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string partitionkey, string rowkey)
        {
            await this._service.DeleteClienteAsync(partitionkey, rowkey);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string partitionkey, string rowkey)
        {
            Cliente cliente = await this._service.FindClienteAsync(partitionkey, rowkey);
            return View(cliente);
        }
    }
}
