using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaHotelColibri.ViewModels.Comun;

public class BarraBusquedaViewModel
{
    public string? Busqueda { get; set; }
    public string Placeholder { get; set; } = "Buscar...";
    public List<FiltroSeleccion> Filtros { get; set; } = [];
}

public class FiltroSeleccion
{
    public string Nombre { get; set; } = string.Empty;
    public string Etiqueta { get; set; } = string.Empty;
    public List<SelectListItem> Opciones { get; set; } = [];

    public static FiltroSeleccion Crear(string nombre, string etiqueta, IEnumerable<string> opciones, string? seleccionado) => new()
    {
        Nombre = nombre,
        Etiqueta = etiqueta,
        Opciones = opciones.Select(o => new SelectListItem(o, o, o == seleccionado)).ToList()
    };
}
