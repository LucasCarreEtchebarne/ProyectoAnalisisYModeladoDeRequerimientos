using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("FACTURA")]
    public class Factura
    {
        [Key]
        public int IdFactura { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Numero de factura")]
        public string NumeroFactura { get; set; }

        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Display(Name = "Emitida por")]
        public int IdUsuario { get; set; }

        [Display(Name = "Fecha de emision")]
        public DateTime FechaEmision { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Tipo de factura")]
        public string TipoFactura { get; set; }

        [Display(Name = "Monto total")]
        public decimal MontoTotal { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoFactura { get; set; }

        [StringLength(250)]
        [Display(Name = "Motivo de anulacion")]
        public string MotivoAnulacion { get; set; }

        [Display(Name = "Fecha de anulacion")]
        public DateTime? FechaAnulacion { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<DetalleFactura> Detalles { get; set; }
        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
