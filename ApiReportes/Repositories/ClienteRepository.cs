using ApiReportes.Models;
using ApiReportes.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiReportes.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        //inyeccion de dependencias para la conexion a la base de datos
        private readonly ApplicationDbContext _context;
        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cliente>> GetClientes()
        {
           return await  _context.Set<Cliente>().ToListAsync();
        }
        public async Task<IEnumerable<OrdenesCliente>> GetOrdenesCliente()
        {

            var resultado = await _context.OrdenesCliente.FromSqlRaw(
               "select o.id_orden as idorden, TO_CHAR(o.fecha, 'dd/mm/yyyy') as fecha," +
               "concat(c.nombre,' ',apellido_paterno,' ',apellido_materno) as nombre,d.id_detalle_orden,d.cantidad,d.precio_unitario, d.descripcion,d.id_orden from ordenes o " +
               "inner join detalle_orden d on o.id_orden=d.id_orden inner join clientes c on o.id_cliente=c.id_cliente"
           ).ToListAsync();

            foreach (var item in resultado)
            {
                Console.WriteLine(   $"Descripción: {item.id_detalle_orden}, Cantidad: {item.descripcion}, ");
            }

            return resultado;
        }
    }
}
