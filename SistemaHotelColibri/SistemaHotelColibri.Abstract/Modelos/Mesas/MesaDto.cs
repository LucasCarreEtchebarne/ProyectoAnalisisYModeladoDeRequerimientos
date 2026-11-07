namespace SistemaHotelColibri.Abstract.Modelos.Mesas;

public class MesaDto
{
    public int IdMesa { get; set; }
    public int NumeroMesa { get; set; }
    public int Capacidad { get; set; }
    public string EstadoMesa { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
