using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class MetodoPago
    {

        [Key]
        public int MetodoPagoId { get; set; }
        public string Nombre { get; set; }

        public List<DetallePago>? DetallesPago { get; set; } 
    }
}
