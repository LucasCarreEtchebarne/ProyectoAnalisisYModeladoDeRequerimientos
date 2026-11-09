using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Pedidos;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.DataAccess.Comun;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Pedidos;

public class ObtenerPedidoDA : IObtenerPedidoDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerPedidoDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<ResultadoPaginado<PedidoDto>> ObtenerAsync(FiltroListado filtro)
    {
        var consulta = _contexto.Pedido.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtro.Estado) &&
            filtro.Estado != FiltroListado.TodosLosEstados)
        {
            var estado = filtro.Estado.Trim();
            consulta = consulta.Where(pedido => pedido.EstadoPedido == estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var texto = filtro.Busqueda.Trim();

            consulta = consulta.Where(pedido =>
                pedido.IdPedido.ToString().Contains(texto) ||
                pedido.TipoPedido.Contains(texto) ||
                (pedido.IdClienteNavigation != null &&
                 (pedido.IdClienteNavigation.Nombre + " " +
                  pedido.IdClienteNavigation.PrimerApellido + " " +
                  (pedido.IdClienteNavigation.SegundoApellido ?? "")).Contains(texto)));
        }

        var pagina = await consulta
            .Include(pedido => pedido.IdClienteNavigation)
            .Include(pedido => pedido.DetallePedido)
                .ThenInclude(detalle => detalle.IdProductoMenuNavigation)
            .OrderByDescending(pedido => pedido.FechaHoraPedido)
            .ThenByDescending(pedido => pedido.IdPedido)
            .PaginarAsync(filtro.Pagina, filtro.TamanoPagina);

        return pagina.Mapear(pedido => pedido.ADto());
    }
}