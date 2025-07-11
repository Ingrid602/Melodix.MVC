using Microsoft.AspNetCore.Identity;

namespace Melodix.Modelos
{
    public class ApplicationUser: IdentityUser
    {
        // Necesarios
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public RolUsuario Rol { get; set; }

        // Perfil extendido
        public PerfilUsuario? Perfil { get; set; }
        // Navegación
        public List<Suscripcion>? Suscripciones { get; set; } 
        public List<Factura>? Facturas { get; set; } 
        public List<Playlist>?   Playlists { get; set; } 
        public List<Notificacion>? Notificaciones { get; set; } 
        public List<Historial>? Historiales { get; set; } 

        public List<SeguidoresUsuarios>? Seguidores { get; set; } 
        public List<SeguidoresUsuarios>? Siguiendo { get; set; } 

        public List<SeguidoresArtistas>? SeguidoresArtistas { get; set; }

        // Para álbumes y playlists guardados en biblioteca (muchos a muchos simplificado)
        public List<Album>? AlbumesGuardados { get; set; } 
        public List<Playlist>?  PlaylistsGuardadas { get; set; } 

    }
}
