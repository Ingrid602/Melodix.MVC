using Melodix.Modelos;

namespace Melodix.MVC.Models
{
    public class BuscarViewModel
    {
        public string Termino { get; set; }

        public List<Cancion> Canciones { get; set; } 
        public List<Album> Albumes { get; set; } 
        public List<Artista> Artistas { get; set; } 
        public List<PerfilUsuario> Perfiles { get; set; } 
        public List<Playlist> Playlists { get; set; } 
    }
}
