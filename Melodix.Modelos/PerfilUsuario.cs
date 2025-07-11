using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Melodix.Modelos
{
    public class PerfilUsuario
    {
        [Key, ForeignKey("Usuario")]
        public string PerfilUsuarioId { get; set; } // PK y FK a ApplicationUser.Id
        public string Bio { get; set; }
        public string FotoUrl { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Pais { get; set; }

        public ApplicationUser? Usuario { get; set; }
    }
}
