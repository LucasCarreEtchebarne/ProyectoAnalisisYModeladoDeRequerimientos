using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Clientes;

public interface IObtenerHistorialClienteDA
{
    Task<HistorialClienteDto> ObtenerAsync(int idCliente);
}