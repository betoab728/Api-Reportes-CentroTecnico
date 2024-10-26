using System.ComponentModel.DataAnnotations;

namespace ApiReportes.Models
{
    public class OrdenesCliente
    {
        [Key]
        public int id_detalle_orden { get; set; }
        public int cantidad { get; set; }
        public double precio_unitario { get; set; }
        public string descripcion { get; set; }
        public int idorden { get; set; }
        public string fecha { get; set; }
        public string nombre { get; set; }
      
       
      
    }
}
