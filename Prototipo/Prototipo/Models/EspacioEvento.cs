using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("ESPACIO_EVENTO")]
    public class EspacioEvento
    {
        [Key]
        public int IdEspacio { get; set; }

        [Required(ErrorMessage = "El nombre del espacio es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Espacio")]
        public string NombreEspacio { get; set; }

        [Range(1, 10000, ErrorMessage = "La capacidad maxima debe ser mayor que cero.")]
        [Display(Name = "Capacidad maxima")]
        public int CapacidadMaxima { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoEspacio { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        public virtual ICollection<Evento> Eventos { get; set; }
    }
}
