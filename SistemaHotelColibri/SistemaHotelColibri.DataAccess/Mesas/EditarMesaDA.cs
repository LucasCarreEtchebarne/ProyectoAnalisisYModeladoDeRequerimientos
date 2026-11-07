using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Mesas;
using SistemaHotelColibri.Abstract.Modelos.Mesas;
using SistemaHotelColibri.DataAccess.Contexto;

namespace SistemaHotelColibri.DataAccess.Mesas;

public class EditarMesaDA : IEditarMesaDA
{
    private readonly HotelColibriContext _contexto;

    public EditarMesaDA(HotelColibriContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> EditarAsync(MesaDto mesa)
    {
        var entidad = await _contexto.Mesa.FindAsync(mesa.IdMesa);
        if (entidad == null)
        {
            return false;
        }

        _contexto.Entry(entidad).CurrentValues.SetValues(mesa.AEntidad());
        await _contexto.SaveChangesAsync();
        return true;
    }
}
