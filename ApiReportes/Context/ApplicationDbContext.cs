using Microsoft.EntityFrameworkCore;
using ApiReportes.Models;

namespace ApiReportes.Context
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
        //dbset para la tabla clientes
        public DbSet<Cliente> Clientes { get; set; }
        //dbset para la tabla ordenes
        public DbSet<OrdenesCliente> OrdenesCliente { get; set; }
       
    }
}
