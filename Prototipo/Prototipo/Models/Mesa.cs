using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("MESA")]
    public class Mesa
    {
        [Key]
        public int IdMesa { get; set; }

        [Display(Name = "Numero de mesa")]
        [Range(1, 999, ErrorMessage = "El numero de mesa debe estar entre 1 y 999.")]
        public int NumeroMesa { get; set; }

        [Range(1, 20, ErrorMessage = "La capacidad debe estar entre 1 y 20.")]
        public int Capacidad { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoMesa { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<ReservaMesa> Reservas { get; set; }
    }
}
