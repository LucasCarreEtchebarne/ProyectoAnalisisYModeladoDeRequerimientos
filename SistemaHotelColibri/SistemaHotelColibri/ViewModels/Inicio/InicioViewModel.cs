using SistemaHotelColibri.Abstract.Modelos.Inventarios;
using SistemaHotelColibri.Abstract.Modelos.Reservas;
using SistemaHotelColibri.Abstract.Modelos.TareasHousekeeping;
using SistemaHotelColibri.ViewModels.Comun;

namespace SistemaHotelColibri.ViewModels.Inicio;

// Datos del panel de inicio. Cada lista se llena solo si el rol del usuario tiene acceso a ese módulo.
public class InicioViewModel
{
    public List<TarjetaResumenViewModel> Tarjetas { get; set; } = [];

    public bool MostrarActividad { get; set; }
    public List<ReservaDto> Llegadas { get; set; } = [];
    public List<ReservaDto> Salidas { get; set; } = [];

    public bool MostrarTareas { get; set; }
    public List<TareaHousekeepingDto> Tareas { get; set; } = [];

    public bool MostrarAlertas { get; set; }
    public List<InventarioDto> Alertas { get; set; } = [];

    public bool MostrarPendientes => MostrarTareas || MostrarAlertas;
}
