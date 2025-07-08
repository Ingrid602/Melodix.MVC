using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Melodix.MVC.Models
{
    public class PlaylistCancion
    {
        [Key]
        [Column(Order = 0)]
        public int PlaylistId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CancionId { get; set; }

        public int Orden { get; set; }

        public Playlist Playlist { get; set; }
        public Cancion Cancion { get; set; }
    }
}
