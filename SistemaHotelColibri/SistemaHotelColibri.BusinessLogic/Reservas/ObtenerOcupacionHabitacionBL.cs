using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Interfaces.BusinessLogic.Reservas;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Reservas;
using SistemaHotelColibri.Abstract.Modelos.Reservas;

namespace SistemaHotelColibri.BusinessLogic.Reservas;

// Una sola regla para saber si una habitación está libre, reservada u ocupada hoy.
// La usan Habitaciones, Pedidos e Inicio para que todos muestren lo mismo.
public class ObtenerOcupacionHabitacionBL : IObtenerOcupacionHabitacionBL
{
    private readonly IObtenerReservasVigentesDA _vigentesDA;

    public ObtenerOcupacionHabitacionBL(IObtenerReservasVigentesDA vigentesDA)
    {
        _vigentesDA = vigentesDA;
    }

    public async Task<Dictionary<int, OcupacionHabitacionDto>> ObtenerAsync()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var vigentes = await _vigentesDA.ObtenerAsync(hoy);

        return vigentes
            .GroupBy(r => r.IdHabitacion)
            .ToDictionary(grupo => grupo.Key, grupo =>
            {
                var enCheckIn = grupo.FirstOrDefault(r => r.EstadoReserva == Estados.Reserva.CheckIn);
                var deHoy = grupo
                    .Where(r => r.EstadoReserva != Estados.Reserva.CheckIn && r.FechaEntrada <= hoy && r.FechaSalida > hoy)
                    .OrderBy(r => r.FechaEntrada)
                    .FirstOrDefault();
                var actual = enCheckIn ?? deHoy;

                return new OcupacionHabitacionDto
                {
                    IdHabitacion = grupo.Key,
                    ReservaActual = actual,
                    ProximaReserva = grupo
                        .Where(r => r.EstadoReserva != Estados.Reserva.CheckIn && r.FechaEntrada > hoy && r != actual)
                        .OrderBy(r => r.FechaEntrada)
                        .FirstOrDefault()
                };
            });
    }
}
