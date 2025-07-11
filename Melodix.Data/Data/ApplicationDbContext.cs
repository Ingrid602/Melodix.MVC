using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Melodix.Modelos;
using Microsoft.AspNetCore.Identity;

namespace Melodix.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Cancion> Canciones { get; set; }

        public DbSet<DetallePago> DetallePagos { get; set; }

        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Historial> Historiales  { get; set; }
        public DbSet <MetodoPago> MetodoPagos { get; set; }

        public DbSet<Notificacion> Notificaciones { get; set; }

        public DbSet<PerfilUsuario> PerfilUsuarios { get; set; }

        public DbSet <Plan> Planes { get; set; }

        public DbSet <Playlist> Playlists { get; set; }
         
        public DbSet <PlaylistCancion> PlaylistCanciones { get; set; }

        public DbSet <SeguidoresArtistas> SeguidoresArtistas { get; set; }

        public DbSet <SeguidoresUsuarios> SeguidoresUsuarios { get; set; }

        public DbSet <Suscripcion> Suscripciones   { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>()
           .HasKey(r => new { r.UserId, r.RoleId });

            modelBuilder.Entity<IdentityUserToken<string>>()
                   .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            // Clave compuesta en PlaylistCancion
            modelBuilder.Entity<PlaylistCancion>()
                .HasKey(pc => new { pc.PlaylistId, pc.CancionId });

            // Relación uno a muchos: Usuario - Playlist
            modelBuilder.Entity<Playlist>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UsuarioId);

            // Relación muchos a muchos: Playlist - Usuarios que la guardaron
            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.UsuariosQueLaGuardaron)
                .WithMany(u => u.PlaylistsGuardadas);

            modelBuilder.Entity<SeguidoresUsuarios>()
    .HasKey(su => new { su.SeguidorId, su.SeguidoId });

            modelBuilder.Entity<SeguidoresUsuarios>()
                .HasOne(su => su.Seguidor)
                .WithMany(u => u.Siguiendo)
                .HasForeignKey(su => su.SeguidorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SeguidoresUsuarios>()
                .HasOne(su => su.Seguido)
                .WithMany(u => u.Seguidores)
                .HasForeignKey(su => su.SeguidoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SeguidoresArtistas>()
    .HasKey(sa => new { sa.UsuarioId, sa.ArtistaId });

            modelBuilder.Entity<SeguidoresArtistas>()
                .HasOne(sa => sa.Usuario)
                .WithMany(u => u.SeguidoresArtistas)
                .HasForeignKey(sa => sa.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SeguidoresArtistas>()
                .HasOne(sa => sa.Artista)
                .WithMany(a => a.Seguidores)
                .HasForeignKey(sa => sa.ArtistaId)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
