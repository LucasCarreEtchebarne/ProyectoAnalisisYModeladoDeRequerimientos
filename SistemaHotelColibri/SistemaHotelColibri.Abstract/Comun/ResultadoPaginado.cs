namespace SistemaHotelColibri.Abstract.Comun;

public interface IPaginacion
{
    int PaginaActual { get; }
    int TamanoPagina { get; }
    int TotalRegistros { get; }
    int TotalPaginas { get; }
    int PrimerRegistro { get; }
    int UltimoRegistro { get; }
    bool TienePaginaAnterior { get; }
    bool TienePaginaSiguiente { get; }
}

public class ResultadoPaginado<T> : IPaginacion
{
    public List<T> Elementos { get; init; } = [];
    public int PaginaActual { get; init; } = 1;
    public int TamanoPagina { get; init; } = FiltroListado.TamanoPaginaPorDefecto;
    public int TotalRegistros { get; init; }

    public int TotalPaginas => TotalRegistros == 0 ? 1 : (int)Math.Ceiling(TotalRegistros / (double)TamanoPagina);
    public int PrimerRegistro => TotalRegistros == 0 ? 0 : (PaginaActual - 1) * TamanoPagina + 1;
    public int UltimoRegistro => Math.Min(PaginaActual * TamanoPagina, TotalRegistros);
    public bool TienePaginaAnterior => PaginaActual > 1;
    public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;

    public ResultadoPaginado<TDestino> Mapear<TDestino>(Func<T, TDestino> mapeo) => new()
    {
        Elementos = Elementos.Select(mapeo).ToList(),
        PaginaActual = PaginaActual,
        TamanoPagina = TamanoPagina,
        TotalRegistros = TotalRegistros
    };
}
