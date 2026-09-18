using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("PAGO")]
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }

        [Display(Name = "Factura")]
        public int IdFactura { get; set; }

        [Display(Name = "Registrado por")]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Metodo de pago")]
        public string MetodoPago { get; set; }

        [Range(0.01, 99999999, ErrorMessage = "El monto pagado debe ser mayor que cero.")]
        [Display(Name = "Monto pagado")]
        public decimal MontoPagado { get; set; }

        [Display(Name = "Monto recibido")]
        public decimal? MontoRecibido { get; set; }

        [Display(Name = "Cambio devuelto")]
        public decimal? CambioDevuelto { get; set; }

        [StringLength(20)]
        [Display(Name = "Tipo de tarjeta")]
        public string TipoTarjeta { get; set; }

        [StringLength(4)]
        [Display(Name = "Ultimos 4 digitos")]
        public string UltimosCuatroDigitos { get; set; }

        [StringLength(50)]
        [Display(Name = "Numero de referencia")]
        public string NumeroReferencia { get; set; }

        [Display(Name = "Fecha y hora")]
        public DateTime FechaHoraPago { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoPago { get; set; }

        [ForeignKey("IdFactura")]
        public virtual Factura Factura { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
