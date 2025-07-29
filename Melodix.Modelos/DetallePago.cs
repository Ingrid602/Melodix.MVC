using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class DetallePago
    {
        [Key]
        public int DetallePagoId { get; set; }

        public int? FacturaId { get; set; }
        public int MetodoPagoId { get; set; }

        public string NombreTitular { get; set; }
        public string NumeroTarjeta { get; set; }
        public string Vencimiento { get; set; }
        public string CodigoSeguridad { get; set; }
        public string Estado { get; set; }

        public Factura? Factura { get; set; }
        public MetodoPago? MetodoPago { get; set; }
    }
}
