using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }

        public string UsuarioId { get; set; }

        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EsPrivada { get; set; }
        public string? ImagenUrl { get; set; }

        public ApplicationUser? Usuario { get; set; }

        public List<PlaylistCancion>? PlaylistCanciones { get; set; } 

        // Usuarios que guardaron esta playlist (muchos a muchos simplificado)
        public List<ApplicationUser>? UsuariosQueLaGuardaron { get; set; } 
    }
}
