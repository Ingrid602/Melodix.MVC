using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class Notificacion
    {
        [Key]
        public int NotificacionId { get; set; }
        public string UsuarioId { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public bool Leida { get; set; }

        public ApplicationUser? Usuario { get; set; }
    }
}
