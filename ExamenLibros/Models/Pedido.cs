using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenLibros.Models
{
    [Table("PEDIDOS")]
    public class Pedido
    {
        [Key]
        [Column("IdPedido")]
        public int IdPedido { get; set; }
        [Column("IdFactura")]
        public int IdFactura { get; set; }
        [Column("Fecha")]
        public DateTime Fecha { get; set; }
        [Column("IdLibro")]
        public int IdLibro { get; set; }
        [Column("IdUsuario")]
        public int IdUsuario { get; set; }
        [Column("Cantidad")]
        public int Cantidad { get; set; }
    }
}
