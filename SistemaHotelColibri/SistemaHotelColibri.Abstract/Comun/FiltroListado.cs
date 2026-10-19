namespace SistemaHotelColibri.Abstract.Comun;

public class FiltroListado
{
    public const int TamanoPaginaPorDefecto = 10;
    public const int TamanoPaginaMaximo = 50;
    public const string TodosLosEstados = "Todos";

    public string? Busqueda { get; set; }
    public string? Estado { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanoPagina { get; set; } = TamanoPaginaPorDefecto;
}
