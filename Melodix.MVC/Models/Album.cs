using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Melodix.MVC.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }
        public int ArtistaId { get; set; }

        public string Nombre { get; set; }
        public DateTime FechaLanzamiento { get; set; }

        public Artista Artista { get; set; }
        public List<Cancion> Canciones { get; set; } = new();

        // Usuarios que guardaron este álbum (muchos a muchos simplificado)
        public List<ApplicationUser> UsuariosQueLoGuardaron { get; set; } = new();

    }
}
