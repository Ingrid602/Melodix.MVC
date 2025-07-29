using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class Artista
    {
        [Key]
        public int ArtistaId { get; set; }
        public string NombreArtistico { get; set; }
        public string Biografia { get; set; }
        public string ImagenUrl { get; set; }

        public string? UsuarioId { get; set; }
        public ApplicationUser? Usuario { get; set; }
        public List<Album>? Albumes { get; set; } 
        public List<SeguidoresArtistas>? Seguidores { get; set; } 
        public List<Cancion>?Canciones { get; set; } 

    }
}
