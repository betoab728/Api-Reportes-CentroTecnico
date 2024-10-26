using ApiReportes.Models;

namespace ApiReportes.Services
{
    public interface IClienteService
    {
        //servicio para listar los clientes
        Task<IEnumerable<Cliente>> GetClientes();

        //servicio para listar las ordenes de los clientes
        Task<IEnumerable<OrdenesCliente>> GetOrdenesCliente(DateTime fechaInicio, DateTime fechaFin);

        Task<byte[]> GetReporteOrdenesClientes(DateTime fechaInicio, DateTime fechaFin);
    }
}
