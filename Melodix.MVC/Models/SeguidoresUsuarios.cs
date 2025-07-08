using System.ComponentModel.DataAnnotations;

namespace Melodix.MVC.Models
{
    public class SeguidoresUsuarios
    {
        [Key]
        public int Id { get; set; }

        public string SeguidorId { get; set; }  // quien sigue
        public string SeguidoId { get; set; }   // a quien sigue
        public DateTime Fecha { get; set; }

        public ApplicationUser Seguidor { get; set; }
        public ApplicationUser Seguido { get; set; }
    }
}
