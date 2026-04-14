using Azure.Data.Tables;
using MvcCoreAzureStorage.Models;

namespace MvcCoreAzureStorage.Services
{
    public class ServiceStorageTables
    {
        private TableClient tableClient;
        public ServiceStorageTables(TableServiceClient tableService)
        {
            this.tableClient = tableService.GetTableClient("clientes");
        }

        public async Task CreateClientAsync(int id, string nombre, string empresa, int edad, int salario)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id,
                Nombre = nombre,
                Empresa = empresa,
                Edad = edad,
                Salario = salario
            };
            await this.tableClient.AddEntityAsync<Cliente>(cliente);
        }

        //LAS ENTIDADES DE TABLA, SI DESEAMOS BUSCAR POR SU ID
        //SOLAMENTE, NO PODEMOS, DEBEMOS HACERLO MEDIANTE UNA BÚSQUEDA
        //DE SU PARTITION Y SU ROW KEY

        public async Task<Cliente> FindClienteAsync(string partitionKey, string rowKey)
        {
            Cliente cliente = await this.tableClient.GetEntityAsync<Cliente>(partitionKey, rowKey);
            return cliente;
        }

        public async Task DeleteClienteAsync(string partitionKey, string rowKey)
        {
            await this.tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            List<Cliente> clientes = new List<Cliente>();
            //PARA LAS BUSQUEDAS SE UTILIZAN QUERYS Y FILTER
            //AUNQUE NO BUSQUEMOS, SI QUEREMOS TODOS, LE MANDAMOS
            //UN FILTER VACIO
            var query =
                this.tableClient.QueryAsync<Cliente>
                (filter: "");
            //EXTRAEMOS LOS DATOS DE LA CONSULTA DEL QUERY
            await foreach (var item in query)
            {
                clientes.Add(item);
            }
            return clientes;
        }

        public async Task<List<Cliente>> GetClientesEmpresaAsync(string empresa)
        {
            //TENEMOS DOS TIPOS DE FILTER, LOS DOS CON query
            //1) SI UTILIZAMOS QueryAsync DEBEMOS ESCRIBIR UNA SINTAXIS ESPECIAL
            //DENTRO DEL filter
            //string filtro = "Campo eq valor"; IGUAL
            //string filtro = "Campo gt valor"; MAYOR
            //string filtro = "Campo eq valor and Campo2 lt valor"; IGUAL Y MENOR
            //string filtro = "Empresa eq " + empresa;
            //var query = this.tableClient.QueryAsync<Cliente>(filter: filtro);

            //1) Utilizar Query PERMITE CONSULTAR CON Lambda
            //PERO SE PIERDE EL ASINCRONO
            //Y NOS DEVUELVE TODO DIRECTAMENTE, NO DEBEMOS HACER UN BUCLE 
            //PARA EXTRAER LOS DATOS
            var query = this.tableClient.Query<Cliente>
                (x => x.Empresa == empresa);
            return query.ToList();
        }
    }
}
