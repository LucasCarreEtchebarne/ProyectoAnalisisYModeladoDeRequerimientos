using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Clientes;

public interface IObtenerHistorialClienteBL
{
    Task<HistorialClienteDto?> ObtenerAsync(int idCliente);
}