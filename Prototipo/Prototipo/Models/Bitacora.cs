using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("BITACORA")]
    public class Bitacora
    {
        [Key]
        public long IdBitacora { get; set; }

        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Modulo")]
        public string Modulo { get; set; }

        [Display(Name = "Registro")]
        public int? IdRegistro { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Accion")]
        public string AccionRealizada { get; set; }

        [Display(Name = "Fecha y hora")]
        public DateTime FechaHora { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
