namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.TareasHousekeeping;

public interface IExisteTareaActivaDA
{
    Task<bool> ExisteTareaActivaAsync(int idHabitacion, string tipoTarea, int idExcluir);
}