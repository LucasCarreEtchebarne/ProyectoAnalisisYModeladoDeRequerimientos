using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.Abstract.Modelos.Mesas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Mesas;

public class AgregarMesaDA : IAgregarMesaDA
{
    private readonly HotelColibriContext _contexto;

    public AgregarMesaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> AgregarAsync(MesaDto mesa)
    {
        var entidad = mesa.AEntidad();
        _contexto.Mesa.Add(entidad);
        await _contexto.SaveChangesAsync();
        return entidad.IdMesa;
    }
}
