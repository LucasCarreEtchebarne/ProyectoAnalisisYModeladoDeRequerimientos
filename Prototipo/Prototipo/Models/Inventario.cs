using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("INVENTARIO")]
    public class Inventario
    {
        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Producto")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "La categoria es obligatoria.")]
        [StringLength(50)]
        [Display(Name = "Categoria")]
        public string CategoriaProducto { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Unidad de medida")]
        public string UnidadMedida { get; set; }

        [Range(0, 99999999, ErrorMessage = "El stock no puede ser negativo.")]
        public decimal Stock { get; set; }

        [Range(0, 99999999, ErrorMessage = "El stock minimo no puede ser negativo.")]
        [Display(Name = "Stock minimo")]
        public decimal StockMinimo { get; set; }

        [Display(Name = "Fecha de ingreso")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoProducto { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        [NotMapped]
        public bool StockBajo
        {
            get { return Stock <= StockMinimo; }
        }

        public virtual ICollection<RecetaProducto> Recetas { get; set; }
        public virtual ICollection<MovimientoInventario> Movimientos { get; set; }
    }
}
