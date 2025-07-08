using System.ComponentModel.DataAnnotations;

namespace Melodix.MVC.Models
{
    public class Factura
    {
        [Key]
        public int FacturaId { get; set; }

        public string UsuarioId { get; set; }
        public int? SuscripcionId { get; set; }
        public int? PlanId { get; set; }

        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }

        public ApplicationUser Usuario { get; set; }
        public Suscripcion Suscripcion { get; set; }
        public Plan Plan { get; set; }

        public List<DetallePago> DetallesPago { get; set; } = new();
    }
}
