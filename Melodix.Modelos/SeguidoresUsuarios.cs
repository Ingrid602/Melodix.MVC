using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class SeguidoresUsuarios
    {
        

        public string SeguidorId { get; set; }  // quien sigue
        public string SeguidoId { get; set; }   // a quien sigue
        public DateTime Fecha { get; set; }

        public ApplicationUser? Seguidor { get; set; }
        public ApplicationUser? Seguido { get; set; }
    }
}
