namespace SistemaHotelColibri.Abstract.Modelos.Eventos;

public class EspacioEventoDto
{
    public int IdEspacio { get; set; }
    public string NombreEspacio { get; set; } = string.Empty;
    public int CapacidadMaxima { get; set; }
    public string EstadoEspacio { get; set; } = string.Empty;
}
