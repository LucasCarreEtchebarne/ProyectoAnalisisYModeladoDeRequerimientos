using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(150)]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El correo electronico es obligatorio.")]
        [StringLength(150)]
        [EmailAddress(ErrorMessage = "El formato del correo electronico no es valido.")]
        [Display(Name = "Correo electronico")]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(255)]
        public string ContrasenaHash { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(30)]
        public string Rol { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        [Display(Name = "Intentos fallidos")]
        public int IntentosFallidos { get; set; }

        [Display(Name = "Fecha de bloqueo")]
        public DateTime? FechaBloqueo { get; set; }

        [Display(Name = "Ultimo acceso")]
        public DateTime? FechaUltimoAcceso { get; set; }

        [Display(Name = "Fecha de creacion")]
        public DateTime FechaCreacion { get; set; }

        public virtual ICollection<Bitacora> Bitacoras { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<TareaHousekeeping> Tareas { get; set; }
    }
}
