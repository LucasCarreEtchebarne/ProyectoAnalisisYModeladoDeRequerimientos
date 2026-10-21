namespace SistemaHotelColibri.Abstract.Interfaces.DataAccess.Correo;

public interface IEnviarCorreoDA
{
    Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml);
}
