using System.ComponentModel.DataAnnotations;

namespace CamelRegistry.Models
{
    public class Camel
    {
        [Key]
        public int Id { get; set; } // Elsődleges kulcs

        [Required]
        public string Name { get; set; } = string.Empty; // A teve neve, kötelező mező

        public string? Color { get; set; } // A teve színe, opcionális mező

        [Range(1,2)]
        public int HumpCount { get; set; } // A teve púpjainak száma, csak 1 vagy 2 lehet

        public DateTime LastFed { get; set; } // Az utolsó etetés időpontja, alapértelmezés szerint a létrehozás időpontja
    }
}
