using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Clientes;

public class ObtenerClienteDA : IObtenerClienteDA
{
    private readonly HotelColibriContext _contexto;

    public ObtenerClienteDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<ClienteDto>> ObtenerAsync(string? estado)
    {
        var consulta = _contexto.Cliente.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(c => c.EstadoCliente == estado);
        }

        var clientes = await consulta
            .OrderBy(c => c.PrimerApellido)
            .ThenBy(c => c.NombreCompleto)
            .ToListAsync();

        return clientes.Select(c => c.ADto()).ToList();
    }
}