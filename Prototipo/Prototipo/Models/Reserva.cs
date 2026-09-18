using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("RESERVA")]
    public class Reserva
    {
        [Key]
        public int IdReserva { get; set; }

        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Display(Name = "Habitacion")]
        public int IdHabitacion { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de entrada")]
        public DateTime FechaEntrada { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de salida")]
        public DateTime FechaSalida { get; set; }

        [Range(1, 20, ErrorMessage = "La cantidad de huespedes debe estar entre 1 y 20.")]
        [Display(Name = "Huespedes")]
        public int CantidadHuespedes { get; set; }

        [Display(Name = "Precio por noche")]
        public decimal PrecioNoche { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Noches")]
        public int CantidadNoches { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Monto de hospedaje")]
        public decimal MontoHospedaje { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoReserva { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }

        [StringLength(250)]
        public string Observaciones { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdHabitacion")]
        public virtual Habitacion Habitacion { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
