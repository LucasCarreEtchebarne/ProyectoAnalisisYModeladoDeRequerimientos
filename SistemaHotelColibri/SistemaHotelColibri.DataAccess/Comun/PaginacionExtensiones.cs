using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.DataAccess.Comun;

public static class PaginacionExtensiones
{
    public static async Task<ResultadoPaginado<T>> PaginarAsync<T>(this IQueryable<T> consulta, int pagina, int tamanoPagina)
    {
        tamanoPagina = Math.Clamp(tamanoPagina, 1, FiltroListado.TamanoPaginaMaximo);

        var totalRegistros = await consulta.CountAsync();
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(totalRegistros / (double)tamanoPagina));
        pagina = Math.Clamp(pagina, 1, totalPaginas);

        var elementos = await consulta
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();

        return new ResultadoPaginado<T>
        {
            Elementos = elementos,
            PaginaActual = pagina,
            TamanoPagina = tamanoPagina,
            TotalRegistros = totalRegistros
        };
    }
}
