using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class SeguidoresArtistas
    {
        [Key]
        public int Id { get; set; }

        public string UsuarioId { get; set; }
        public int ArtistaId { get; set; }
        public DateTime Fecha { get; set; }

        public ApplicationUser? Usuario { get; set; }
        public Artista? Artista { get; set; }
    }
}
