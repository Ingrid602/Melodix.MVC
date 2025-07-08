using System.ComponentModel.DataAnnotations;

namespace Melodix.MVC.Models
{
    public class Cancion
    {
        [Key]
        public int CancionId { get; set; }
        public int AlbumId { get; set; }
        public int ArtistaId { get; set; }

        public string Nombre { get; set; }
        public string Duracion { get; set; }
        public string Genero { get; set; }
        public string ArchivoURL { get; set; }

        public Album Album { get; set; }
        public Artista Artista { get; set; }

        public List<PlaylistCancion> PlaylistCanciones { get; set; } = new();
        public List<Historial> Historiales { get; set; } = new();
    }
}
