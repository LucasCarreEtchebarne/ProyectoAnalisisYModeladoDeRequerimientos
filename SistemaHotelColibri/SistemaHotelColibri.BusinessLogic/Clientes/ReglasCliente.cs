using System.Net.Mail;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Clientes;

namespace SistemaHotelColibri.BusinessLogic.Clientes;

internal static class ReglasCliente
{
    public static void Normalizar(ClienteDto cliente)
    {
        cliente.Identificacion = cliente.Identificacion.Trim().ToUpperInvariant();
        cliente.NombreCompleto = cliente.NombreCompleto.Trim();
        cliente.PrimerApellido = cliente.PrimerApellido.Trim();
        cliente.SegundoApellido = Limpiar(cliente.SegundoApellido);
        cliente.Telefono = Limpiar(cliente.Telefono);
        cliente.CorreoElectronico = Limpiar(cliente.CorreoElectronico)?.ToLowerInvariant();
        cliente.Direccion = Limpiar(cliente.Direccion);
    }

    public static string? Validar(ClienteDto cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Identificacion))
        {
            return string.Format(Mensajes.CampoObligatorio, "identificación");
        }

        if (string.IsNullOrWhiteSpace(cliente.NombreCompleto))
        {
            return string.Format(Mensajes.CampoObligatorio, "nombre");
        }

        if (string.IsNullOrWhiteSpace(cliente.PrimerApellido))
        {
            return string.Format(Mensajes.CampoObligatorio, "primer apellido");
        }

        if (cliente.CorreoElectronico != null && !EsCorreoValido(cliente.CorreoElectronico))
        {
            return Mensajes.CorreoInvalido;
        }

        if (!Estados.Cliente.Todos.Contains(cliente.EstadoCliente))
        {
            return Mensajes.EstadoClienteInvalido;
        }

        return null;
    }

    private static string? Limpiar(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();

    private static bool EsCorreoValido(string correo)
    {
        return MailAddress.TryCreate(correo, out var direccion) && direccion.Address == correo;
    }
}