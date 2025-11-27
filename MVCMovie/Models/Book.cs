using System.ComponentModel.DataAnnotations;

namespace MVCMovie.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Autor { get; set; }

        [DataType(DataType.Date)]
        public DateTime Lanzamiento { get; set; }

        // URL o ruta local de la imagen
        public string? Portada { get; set; }

        // Indica si está prestado o no
        public bool Prestado { get; set; }
    }
}
