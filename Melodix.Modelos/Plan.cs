using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class Plan
    {
        [Key]
        public int PlanId { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioMensual { get; set; }

        public List<Suscripcion>? Suscripciones { get; set; } 
        public List<Factura>? Facturas { get; set; } 
    }
}
