using ApiReportes.Models;
using ApiReportes.Repositories;
using Microsoft.Reporting.NETCore;
using System.Reflection;
using System.Data;

namespace ApiReportes.Services
{
    public class ClienteService : IClienteService
    {
        //inyeccion de dependencias para el repositorio
        private readonly IClienteRepository _clienteRepository;
        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        public async Task<IEnumerable<Cliente>> GetClientes()
        {
               return await _clienteRepository.GetClientes();
        }

        public async Task<IEnumerable<OrdenesCliente>> GetOrdenesCliente(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _clienteRepository.GetOrdenesCliente( fechaInicio,  fechaFin);

         
        }

        public static void Load(LocalReport report, IEnumerable<Cliente> clientes)
        {
            //ruta del reporte 

            var reportPath = "ApiReportes.Reports.Report1.rdlc";

            //cargamos el archivo del reporte
            using var rdlcStream = Assembly.GetExecutingAssembly()
             .GetManifestResourceStream(reportPath);

            if (rdlcStream == null) {
                throw new FileNotFoundException($"No se encontro el reporte: {reportPath} ");
                
            }

            //tabla con los datos: nombre,direccion,correo,telefono

            var dataTable = new DataTable("Clientes");
            dataTable.Columns.Add("nombre");
            dataTable.Columns.Add("direccion");
            dataTable.Columns.Add("correo");
            dataTable.Columns.Add("telefono");

            foreach (var cliente in clientes)
            {
                dataTable.Rows.Add(cliente.nombre, cliente.direccion, cliente.correo, cliente.telefono);
            }

            report.LoadReportDefinition(rdlcStream);

            var existingDataSource = report.DataSources["DataSet1"];
            if (existingDataSource != null)
            {
                // Reemplazar el contenido del DataSource existente
                existingDataSource.Value = dataTable;
            }
            else
            {
                // Si no existe, agregar el nuevo DataSource
                report.DataSources.Add(new ReportDataSource("DataSet1", dataTable));
            }


            /*

           //limpiamos los datos del reporte
           report.DataSources.Clear();

            //agregamos la tabla al reporte
            report.DataSources.Add(new ReportDataSource("DataSet1", dataTable));*/

        }

        public async Task<byte[]> GetReporteOrdenesClientes(DateTime fechaInicio, DateTime fechaFin)
        {
            // Obtener los datos de las órdenes
            var ordenesClientes = await GetOrdenesCliente(fechaInicio,  fechaFin);

            // Ruta del reporte RDLC
            var reportPath = "ApiReportes.Reports.OrdenesClientes.rdlc";

            // Cargar el archivo del reporte
            using var rdlcStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(reportPath);

            if (rdlcStream == null)
            {
                throw new FileNotFoundException($"No se encontró el reporte: {reportPath}");
            }

            // Crear la tabla que se pasará al reporte
            var dataTable = new DataTable("OrdenesClientes");
            dataTable.Columns.Add("id_detalle_orden", typeof(int));
            dataTable.Columns.Add("cantidad", typeof(int));
            dataTable.Columns.Add("precio_unitario", typeof(double));
            dataTable.Columns.Add("descripcion", typeof(string));
            dataTable.Columns.Add("idorden", typeof(string));
            dataTable.Columns.Add("fecha", typeof(string));
            dataTable.Columns.Add("nombre", typeof(string));

            //  dataTable.Columns.Add("id_detalle_orden", typeof(string));

            // Llenar la tabla con los datos
            foreach (var orden in ordenesClientes)
            {

             //   Console.WriteLine("Ordenes de clientes: " + orden.id_detalle_orden);

                dataTable.Rows.Add(
                    orden.id_detalle_orden,
                    orden.cantidad,
                    orden.precio_unitario,
                    orden.descripcion,
                    orden.idorden,
                    orden.fecha,
                    orden.nombre
              

                );
            }

            Console.WriteLine("Ordenes de clientes: " + ordenesClientes.Count());


            // Inicializar el reporte y cargar la definición
            using var report = new LocalReport();
            report.LoadReportDefinition(rdlcStream);

            // Limpiar los DataSources y agregar el modelo directamente al reporte
            report.DataSources.Clear();
            report.DataSources.Add(new ReportDataSource("DataSetOrdenesClientes", dataTable));


            // **Agregar los parámetros al reporte**
            report.SetParameters(new[]
            {
                    new ReportParameter("pdesde", fechaInicio.ToString("dd/MM/yyyy")),
                    new ReportParameter("phasta", fechaFin.ToString("dd/MM/yyyy"))
                });

            // Renderizar el reporte como PDF
            var pdf = report.Render("PDF", null, out _, out _, out _, out _, out _);

            //retorno el reporte
            return pdf;


        }


    }
}
