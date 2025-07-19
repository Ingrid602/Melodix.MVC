using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }
        public int ArtistaId { get; set; }

        public string Nombre { get; set; }
        public DateTime FechaLanzamiento { get; set; }

        public String? urlImagen{ get; set; }

        public Artista? Artista { get; set; }
        public List<Cancion>? Canciones { get; set; } 

        // Usuarios que guardaron este álbum (muchos a muchos simplificado)
        public List<ApplicationUser>? UsuariosQueLoGuardaron { get; set; } 

    }
}
