using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("RESERVA_MESA")]
    public class ReservaMesa
    {
        [Key]
        public int IdReservaMesa { get; set; }

        [Display(Name = "Mesa")]
        public int IdMesa { get; set; }

        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        public int IdUsuario { get; set; }

        [Display(Name = "Fecha y hora")]
        public DateTime FechaHoraReserva { get; set; }

        [Range(1, 20, ErrorMessage = "La cantidad de personas debe estar entre 1 y 20.")]
        [Display(Name = "Personas")]
        public int CantidadPersonas { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoReservaMesa { get; set; }

        [StringLength(250)]
        public string Observaciones { get; set; }

        [ForeignKey("IdMesa")]
        public virtual Mesa Mesa { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
