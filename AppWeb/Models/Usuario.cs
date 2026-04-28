using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWeb.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [StringLength(255)]
        public byte[] Contrasena { get; set; }

        public string Salt { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        public int idRol { get; set; }
        [ForeignKey("idRol")]
        public Rol Roles { get; set; }

        public ICollection<Compra> Compras { get; set; }
    }
}
