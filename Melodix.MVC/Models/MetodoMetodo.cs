using System.ComponentModel.DataAnnotations;

namespace Melodix.MVC.Models
{
    public class MetodoMetodo
    {
        [Key]
        public int MetodoPagoId { get; set; }
        public string Nombre { get; set; }

        public List<DetallePago> DetallesPago { get; set; } = new();
    }
}
