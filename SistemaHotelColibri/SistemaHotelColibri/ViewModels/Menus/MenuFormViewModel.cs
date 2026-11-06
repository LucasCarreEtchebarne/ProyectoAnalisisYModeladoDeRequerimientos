using System.ComponentModel.DataAnnotations;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Menus;

namespace SistemaHotelColibri.ViewModels.Menus;

public class MenuFormViewModel
{
    public int IdProductoMenu { get; set; }

    [Display(Name = "nombre del producto")]
    [Required(ErrorMessage = Mensajes.MenuCamposIncompletos)]
    [StringLength(100, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? NombreProducto { get; set; }

    [Display(Name = "categoría")]
    [Required(ErrorMessage = Mensajes.MenuCamposIncompletos)]
    [StringLength(50, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? CategoriaMenu { get; set; }

    [Display(Name = "precio")]
    [Required(ErrorMessage = Mensajes.MenuCamposIncompletos)]
    [Range(0, 9999999999.99, ErrorMessage = Mensajes.PrecioMenuInvalido)]
    public decimal? Precio { get; set; }

    [Display(Name = "estado")]
    [Required(ErrorMessage = Mensajes.MenuCamposIncompletos)]
    public string? EstadoProductoMenu { get; set; } = Estados.ProductoMenu.Disponible;

    [Display(Name = "descripción")]
    [StringLength(250, ErrorMessage = Mensajes.LongitudMaxima)]
    public string? Descripcion { get; set; }

    public static MenuFormViewModel DesdeDto(MenuDto producto) => new()
    {
        IdProductoMenu = producto.IdProductoMenu,
        NombreProducto = producto.NombreProducto,
        CategoriaMenu = producto.CategoriaMenu,
        Precio = producto.Precio,
        EstadoProductoMenu = producto.EstadoProductoMenu,
        Descripcion = producto.Descripcion
    };

    public MenuDto ADto() => new()
    {
        IdProductoMenu = IdProductoMenu,
        NombreProducto = NombreProducto ?? string.Empty,
        CategoriaMenu = CategoriaMenu ?? string.Empty,
        Precio = Precio ?? 0,
        EstadoProductoMenu = EstadoProductoMenu ?? string.Empty,
        Descripcion = Descripcion
    };
}
