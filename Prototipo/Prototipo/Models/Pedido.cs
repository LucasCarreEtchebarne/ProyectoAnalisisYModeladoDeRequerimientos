using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Prototipo.Models
{
    [Table("PEDIDO")]
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        [Display(Name = "Atendido por")]
        public int IdUsuario { get; set; }

        [Display(Name = "Mesa")]
        public int? IdMesa { get; set; }

        [Display(Name = "Habitacion")]
        public int? IdHabitacion { get; set; }

        [Display(Name = "Reserva")]
        public int? IdReserva { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Tipo de pedido")]
        public string TipoPedido { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoPedido { get; set; }

        [Display(Name = "Fecha y hora")]
        public DateTime FechaHoraPedido { get; set; }

        [StringLength(250)]
        public string Observaciones { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("IdMesa")]
        public virtual Mesa Mesa { get; set; }

        [ForeignKey("IdHabitacion")]
        public virtual Habitacion Habitacion { get; set; }

        [ForeignKey("IdReserva")]
        public virtual Reserva Reserva { get; set; }

        public virtual ICollection<DetallePedido> Detalles { get; set; }

        [NotMapped]
        [Display(Name = "Total")]
        public decimal Total
        {
            get { return Detalles == null ? 0m : Detalles.Sum(d => d.Subtotal); }
        }
    }
}
