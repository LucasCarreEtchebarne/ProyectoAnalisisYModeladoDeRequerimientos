using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SistemaHotelColibri.Abstract.Interfaces.DataAccess.Correo;

namespace SistemaHotelColibri.DataAccess.Correo;

public class EnviarCorreoSmtpDA : IEnviarCorreoDA
{
    private readonly ConfiguracionCorreo _configuracion;

    public EnviarCorreoSmtpDA(IOptions<ConfiguracionCorreo> configuracion)
    {
        _configuracion = configuracion.Value;
    }

    public async Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml)
    {
        using var mensaje = new MailMessage
        {
            From = new MailAddress(_configuracion.Remitente!, _configuracion.NombreRemitente),
            Subject = asunto,
            Body = cuerpoHtml,
            IsBodyHtml = true
        };
        mensaje.To.Add(destinatario);

        using var cliente = new SmtpClient(_configuracion.Servidor, _configuracion.Puerto)
        {
            EnableSsl = _configuracion.UsarSsl,
            Credentials = string.IsNullOrWhiteSpace(_configuracion.Usuario)
                ? null
                : new NetworkCredential(_configuracion.Usuario, _configuracion.Contrasena)
        };

        await cliente.SendMailAsync(mensaje);
    }
}
