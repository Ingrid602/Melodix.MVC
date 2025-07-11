using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Melodix.Modelos
{
    public class PlaylistCancion
    {
        
        public int PlaylistId { get; set; }
        
        public int CancionId { get; set; }

        public int Orden { get; set; }

        public Playlist? Playlist { get; set; }
        public Cancion? Cancion { get; set; }
    }
}
