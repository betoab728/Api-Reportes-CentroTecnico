using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiReportes.Models
{
    //especificar tabla clientes

    [Table("clientes") ]
    public class Cliente
    {//los atributos son :

        [Key]
        public int id_cliente { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string dni { get; set; }
        public string estado { get; set; }
        public string nombre { get; set; }
        public string apellido_materno { get; set; }
        public string apellido_paterno { get; set; }
      


    }
}
