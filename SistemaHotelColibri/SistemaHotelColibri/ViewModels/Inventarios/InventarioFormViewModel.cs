using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.ViewModels.Inventarios;

public class InventarioFormViewModel
{
    public int IdProducto { get; set; }

    [Display(Name = "nombre del producto")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(100, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? NombreProducto { get; set; }

    [Display(Name = "categoría")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [StringLength(50, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? CategoriaProducto { get; set; }

    [Display(Name = "unidad de medida")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    public string? UnidadMedida { get; set; } = "Unidad";

    [Display(Name = "stock")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(0, 999999999.999, ErrorMessage = Mensajes.ValorNoNegativo)]
    public decimal? Stock { get; set; }

    [Display(Name = "stock mínimo")]
    [Required(ErrorMessage = Mensajes.CampoObligatorio)]
    [Range(0, 999999999.999, ErrorMessage = Mensajes.ValorNoNegativo)]
    public decimal? StockMinimo { get; set; }

    [Display(Name = "descripción")]
    [StringLength(250, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? Descripcion { get; set; }

    public InventarioDto ADto() => new()
    {
        IdProducto = IdProducto,
        NombreProducto = NombreProducto ?? string.Empty,
        CategoriaProducto = CategoriaProducto ?? string.Empty,
        UnidadMedida = UnidadMedida ?? string.Empty,
        Stock = Stock ?? 0,
        StockMinimo = StockMinimo ?? 0,
        Descripcion = Descripcion
    };
}
