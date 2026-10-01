using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.BusinessLogic.Clientes;

public class BuscarClienteBL : IBuscarClienteBL
{
    private const int LongitudMaximaBusqueda = 100;

    private readonly IBuscarClienteDA _buscarDA;

    public BuscarClienteBL(IBuscarClienteDA buscarDA)
    {
        _buscarDA = buscarDA;
    }

    public async Task<List<ClienteDto>> BuscarAsync(string? texto, string? estado)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return [];
        }

        texto = texto.Trim();
        if (texto.Length > LongitudMaximaBusqueda)
        {
            texto = texto[..LongitudMaximaBusqueda];
        }

        return await _buscarDA.BuscarAsync(texto, estado);
    }
}