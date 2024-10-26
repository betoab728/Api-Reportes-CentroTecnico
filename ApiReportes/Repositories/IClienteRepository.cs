using ApiReportes.Models;

namespace ApiReportes.Repositories
{
    public interface IClienteRepository
    {
        //metodo para listar los clientes
        Task<IEnumerable<Cliente>> GetClientes();
        Task<IEnumerable<OrdenesCliente>> GetOrdenesCliente();

    }
}
