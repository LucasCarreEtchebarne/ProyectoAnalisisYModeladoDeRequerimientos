using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("HABITACION")]
    public class Habitacion
    {
        [Key]
        public int IdHabitacion { get; set; }

        [Required(ErrorMessage = "El numero de habitacion es obligatorio.")]
        [StringLength(10)]
        [Display(Name = "Numero")]
        public string NumeroHabitacion { get; set; }

        [Required(ErrorMessage = "El tipo de habitacion es obligatorio.")]
        [StringLength(30)]
        [Display(Name = "Tipo")]
        public string TipoHabitacion { get; set; }

        [Range(1, 20, ErrorMessage = "La capacidad debe estar entre 1 y 20.")]
        public int Capacidad { get; set; }

        [Range(0, 99999999, ErrorMessage = "El precio no puede ser negativo.")]
        [Display(Name = "Precio por noche")]
        public decimal Precio { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoHabitacion { get; set; }

        [Range(1, 50, ErrorMessage = "El piso debe estar entre 1 y 50.")]
        public int Piso { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        public virtual ICollection<Reserva> Reservas { get; set; }
        public virtual ICollection<TareaHousekeeping> Tareas { get; set; }
    }
}
