using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("CONSULTA_IA")]
    public class ConsultaIA
    {
        [Key]
        public int IdConsultaIA { get; set; }

        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Debe escribir una consulta.")]
        [Display(Name = "Consulta")]
        public string ConsultaIngresada { get; set; }

        [Display(Name = "Respuesta")]
        public string RespuestaGenerada { get; set; }

        [Display(Name = "Fecha")]
        public DateTime FechaRealizada { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
