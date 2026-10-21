using Microsoft.Extensions.Logging;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Correo;

namespace SistemaHotelColibri.DataAccess.Correo;

public class EnviarCorreoRegistroDA : IEnviarCorreoDA
{
    private readonly ILogger<EnviarCorreoRegistroDA> _registro;

    public EnviarCorreoRegistroDA(ILogger<EnviarCorreoRegistroDA> registro)
    {
        _registro = registro;
    }

    public Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml)
    {
        _registro.LogWarning(
            "SMTP no configurado. Correo NO enviado.\nPara: {Destinatario}\nAsunto: {Asunto}\n{Cuerpo}",
            destinatario, asunto, cuerpoHtml);
        return Task.CompletedTask;
    }
}
