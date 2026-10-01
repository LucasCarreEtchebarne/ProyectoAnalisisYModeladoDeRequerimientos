using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;
using SistemaHotelColibri.Abstract.Modelos.Clientes;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Clientes;

public class BuscarClienteDA : IBuscarClienteDA
{
    private readonly HotelColibriContext _contexto;

    public BuscarClienteDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<ClienteDto>> BuscarAsync(string texto, string? estado)
    {
        var consulta = _contexto.Cliente.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(estado))
        {
            consulta = consulta.Where(c => c.EstadoCliente == estado);
        }

        
        consulta = consulta.Where(c =>
            c.Identificacion.Contains(texto)
            || c.NombreCompleto.Contains(texto)
            || c.PrimerApellido.Contains(texto)
            || (c.SegundoApellido != null && c.SegundoApellido.Contains(texto))
            || (c.NombreCompleto + " " + c.PrimerApellido).Contains(texto)
            || (c.Telefono != null && c.Telefono.Contains(texto))
            || (c.CorreoElectronico != null && c.CorreoElectronico.Contains(texto)));

        var clientes = await consulta
            .OrderBy(c => c.PrimerApellido)
            .ThenBy(c => c.NombreCompleto)
            .ToListAsync();

        return clientes.Select(c => c.ADto()).ToList();
    }
}