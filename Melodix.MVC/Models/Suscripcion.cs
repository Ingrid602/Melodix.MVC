using System.ComponentModel.DataAnnotations;

namespace Melodix.MVC.Models
{
    public class Suscripcion
    {
        [Key]
        public int SuscripcionId { get; set; }

        public string UsuarioId { get; set; }
        public int PlanId { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; }

        public ApplicationUser Usuario { get; set; }
        public Plan Plan { get; set; }

        public List<Factura> Facturas { get; set; } = new();
    }
}
