using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melodix.Modelos
{
    public class SolicitudMusico
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }

        public string NombreArtistico { get; set; }  
        public string Distribuidora { get; set; }    
        public string Mensaje { get; set; }          

        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

        public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;


    }
}
