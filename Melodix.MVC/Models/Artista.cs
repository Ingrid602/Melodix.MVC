using System.ComponentModel.DataAnnotations;

namespace Melodix.MVC.Models
{
    public class Artista
    {
        [Key]
        public int ArtistaId { get; set; }
        public string NombreArtistico { get; set; }
        public string Biografia { get; set; }

        public List<Album> Albumes { get; set; } = new();
        public List<SeguidoresArtistas> Seguidores { get; set; } = new();
        public List<Cancion> Canciones { get; set; } = new();

    }
}
