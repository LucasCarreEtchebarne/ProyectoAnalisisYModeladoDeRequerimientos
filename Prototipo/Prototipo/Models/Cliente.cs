using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("CLIENTE")]
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "La identificacion es obligatoria.")]
        [StringLength(30)]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Primer apellido")]
        public string PrimerApellido { get; set; }

        [StringLength(50)]
        [Display(Name = "Segundo apellido")]
        public string SegundoApellido { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(150)]
        [EmailAddress(ErrorMessage = "El formato del correo electronico no es valido.")]
        [Display(Name = "Correo electronico")]
        public string CorreoElectronico { get; set; }

        [StringLength(250)]
        public string Direccion { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoCliente { get; set; }

        [NotMapped]
        public string NombreMostrar
        {
            get { return (NombreCompleto + " " + PrimerApellido + " " + SegundoApellido).Trim(); }
        }

        public virtual ICollection<Reserva> Reservas { get; set; }
        public virtual ICollection<Evento> Eventos { get; set; }
        public virtual ICollection<Factura> Facturas { get; set; }
    }
}
