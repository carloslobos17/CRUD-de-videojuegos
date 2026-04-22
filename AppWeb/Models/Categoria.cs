using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models
{
    public class Categoria
    {
        [Key]
        public int idCategoria { get; set; }
        public string categoria { get; set; }
    }
}
