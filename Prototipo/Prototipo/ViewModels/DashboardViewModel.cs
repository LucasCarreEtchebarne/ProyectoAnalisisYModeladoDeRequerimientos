using System.Collections.Generic;
using Prototipo.Models;

namespace Prototipo.ViewModels
{
    public class DashboardViewModel
    {
        public int HabitacionesTotales { get; set; }
        public int HabitacionesOcupadas { get; set; }
        public int HabitacionesDisponibles { get; set; }
        public int HabitacionesLimpieza { get; set; }

        public decimal PorcentajeOcupacion
        {
            get
            {
                return HabitacionesTotales == 0
                    ? 0m
                    : decimal.Round(HabitacionesOcupadas * 100m / HabitacionesTotales, 1);
            }
        }

        public int LlegadasHoy { get; set; }
        public int SalidasHoy { get; set; }
        public int ReservasPendientes { get; set; }

        public int PedidosHoy { get; set; }
        public int PedidosPendientes { get; set; }

        public decimal IngresosHoy { get; set; }
        public decimal IngresosMes { get; set; }
        public int FacturasPendientes { get; set; }
        public decimal MontoPorCobrar { get; set; }

        public int TareasPendientes { get; set; }
        public int EventosProximos { get; set; }

        public List<Inventario> AlertasStock { get; set; }
        public List<Reserva> ProximasLlegadas { get; set; }

        public DashboardViewModel()
        {
            AlertasStock = new List<Inventario>();
            ProximasLlegadas = new List<Reserva>();
        }
    }
}
