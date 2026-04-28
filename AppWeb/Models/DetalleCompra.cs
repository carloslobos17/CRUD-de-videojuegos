using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWeb.Models
{
    public class DetalleCompra
    {
        [Key]
        public int Id { get; set; }

        public int VideojuegosId { get; set; }
        [ForeignKey("VideojuegosId")]

        public Videojuego Videojuego { get; set; }

        public int cantidad { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal total { get; set; }

        public string estadoCompra { get; set; }

        public DateTime fechaHoraTransaccion { get; set; }

        public string codigoTransaccion { get; set; }

        public int idCompra { get; set; }
        [ForeignKey("idCompra")]
        
        public Compra Compra { get; set; } 

    }
}
