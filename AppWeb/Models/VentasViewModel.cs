using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models
{
    public class VentasViewModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; }

        public int UsuarioId { get; set; }
        public int VideojuegosId { get; set; }
        public int cantidad { get; set; }
        public decimal total { get; set; }
        public string estadoCompra { get; set; }
        public DateTime fechaHoraTransaccion { get; set; }
        public string codigoTransaccion { get; set; }
        public int IdCompra { get; set; }
    }
}