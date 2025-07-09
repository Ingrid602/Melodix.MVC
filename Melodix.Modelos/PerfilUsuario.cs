using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class PerfilUsuario
    {
        [Key]
        public string PerfilUsuarioId { get; set; } // PK y FK a ApplicationUser.Id
        public string Bio { get; set; }
        public string FotoUrl { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Pais { get; set; }

        public ApplicationUser? Usuario { get; set; }
    }
}
