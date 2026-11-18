namespace SistemaHotelColibri.ViewModels.Comun;

// Tarjeta con un número de resumen que se muestra arriba de los listados y en el inicio.
public class TarjetaResumenViewModel
{
    public string Titulo { get; set; } = string.Empty;
    public int Valor { get; set; }
    public string? Detalle { get; set; }
    public string Icono { get; set; } = "bi-circle";
    public string Color { get; set; } = "verde";
    public string? Url { get; set; }
    public bool Seleccionada { get; set; }

    public static TarjetaResumenViewModel Crear(string titulo, int valor, string icono, string color,
        string? url = null, bool seleccionada = false, string? detalle = null) => new()
    {
        Titulo = titulo,
        Valor = valor,
        Icono = icono,
        Color = color,
        Url = url,
        Seleccionada = seleccionada,
        Detalle = detalle
    };
}
