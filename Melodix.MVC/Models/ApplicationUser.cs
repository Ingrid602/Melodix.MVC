using Microsoft.AspNetCore.Identity;

namespace Melodix.MVC.Models
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
        public List<Suscripcion> Suscripciones { get; set; } = new();
        public List<Factura> Facturas { get; set; } = new();
        public List<Playlist> Playlists { get; set; } = new();
        public List<Notificacion> Notificaciones { get; set; } = new();
        public List<Historial> Historiales { get; set; } = new();

        public List<SeguidoresUsuarios> Seguidores { get; set; } = new();
        public List<SeguidoresUsuarios> Siguiendo { get; set; } = new();

        public List<SeguidoresArtistas> SeguidoresArtistas { get; set; } = new();

        // Para álbumes y playlists guardados en biblioteca (muchos a muchos simplificado)
        public List<Album> AlbumesGuardados { get; set; } = new();
        public List<Playlist> PlaylistsGuardadas { get; set; } = new();

    }
}
