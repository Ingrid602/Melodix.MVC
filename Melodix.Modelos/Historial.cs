using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class Historial
    {
        [Key]
        public int Id { get; set; }  // PK simple para evitar poner Fluent API

        public string UsuarioId { get; set; }
        public int CancionId { get; set; }
        public DateTime FechaHora { get; set; }

        public ApplicationUser? Usuario { get; set; }
        public Cancion? Cancion { get; set; }
    }
}
