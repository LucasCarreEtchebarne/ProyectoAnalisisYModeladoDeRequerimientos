using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("EVENTO")]
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }

        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Display(Name = "Espacio")]
        public int IdEspacio { get; set; }

        [Required(ErrorMessage = "El nombre del evento es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Evento")]
        public string NombreEvento { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public DateTime FechaEvento { get; set; }

        [Display(Name = "Hora de inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Display(Name = "Hora de fin")]
        public TimeSpan HoraFin { get; set; }

        [Range(1, 10000, ErrorMessage = "La cantidad de participantes debe ser mayor que cero.")]
        public int Participantes { get; set; }

        [Range(0, 99999999, ErrorMessage = "El monto no puede ser negativo.")]
        [Display(Name = "Monto acordado")]
        public decimal MontoAcordado { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoEvento { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdEspacio")]
        public virtual EspacioEvento Espacio { get; set; }
    }
}
