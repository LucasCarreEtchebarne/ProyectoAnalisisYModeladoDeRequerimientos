using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("HOUSEKEEPING")]
    public class TareaHousekeeping
    {
        [Key]
        public int IdTarea { get; set; }

        [Display(Name = "Habitacion")]
        public int IdHabitacion { get; set; }

        [Display(Name = "Asignada a")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El tipo de tarea es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Tipo de tarea")]
        public string TipoTarea { get; set; }

        [Display(Name = "Fecha de asignacion")]
        public DateTime FechaAsignacion { get; set; }

        [Display(Name = "Fecha limite")]
        public DateTime? FechaLimite { get; set; }

        [Display(Name = "Fecha completada")]
        public DateTime? FechaCompletada { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoTarea { get; set; }

        [StringLength(250)]
        public string Observaciones { get; set; }

        [ForeignKey("IdHabitacion")]
        public virtual Habitacion Habitacion { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
