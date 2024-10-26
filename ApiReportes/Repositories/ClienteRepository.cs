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
        public async Task<IEnumerable<OrdenesCliente>> GetOrdenesCliente(DateTime fechaInicio, DateTime fechaFin)
        {
            // Convertir las fechas a UTC
            fechaInicio = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Utc);
            fechaFin = DateTime.SpecifyKind(fechaFin, DateTimeKind.Utc);

            var resultado = await _context.OrdenesCliente.FromSqlInterpolated($@"
                SELECT o.id_orden AS idorden, TO_CHAR(o.fecha, 'dd/mm/yyyy') AS fecha,
                       CONCAT(c.nombre, ' ', apellido_paterno, ' ', apellido_materno) AS nombre,
                       d.id_detalle_orden, d.cantidad, d.precio_unitario, d.descripcion, d.id_orden 
                FROM ordenes o 
                INNER JOIN detalle_orden d ON o.id_orden = d.id_orden 
                INNER JOIN clientes c ON o.id_cliente = c.id_cliente
                WHERE o.fecha BETWEEN {fechaInicio} AND {fechaFin}
            ").ToListAsync();

            foreach (var item in resultado)
            {
                Console.WriteLine(   $"Descripción: {item.id_detalle_orden}, Cantidad: {item.descripcion}, ");
            }

            return resultado;
        }
    }
}
