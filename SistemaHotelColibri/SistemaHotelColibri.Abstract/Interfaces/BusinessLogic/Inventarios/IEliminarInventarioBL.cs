using SistemaHotelColibri.Abstract.Comun;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Inventarios;

public interface IEliminarInventarioBL
{
    Task<ResultadoOperacion> EliminarAsync(int idProducto);
}
