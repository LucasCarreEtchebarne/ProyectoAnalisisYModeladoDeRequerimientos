using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Inventarios;

namespace SistemaHotelColibri.BusinessLogic.Inventarios;

public class ObtenerAlertasInventarioBL : IObtenerAlertasInventarioBL
{
    private readonly IObtenerAlertasInventarioDA _obtenerAlertasDA;

    public ObtenerAlertasInventarioBL(IObtenerAlertasInventarioDA obtenerAlertasDA)
    {
        _obtenerAlertasDA = obtenerAlertasDA;
    }

    public Task<List<InventarioDto>> ObtenerAlertasAsync()
    {
        return _obtenerAlertasDA.ObtenerAlertasAsync();
    }
}
