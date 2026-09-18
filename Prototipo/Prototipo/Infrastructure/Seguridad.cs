using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Prototipo.Infrastructure
{
    public static class Seguridad
    {
        private const int FactorTrabajo = 11;

        public static string GenerarHash(string contrasena)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasena, FactorTrabajo);
        }

        public static bool VerificarContrasena(string contrasena, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(contrasena, hash);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void IniciarSesion(int idUsuario, string nombreUsuario, string nombreCompleto, string rol)
        {
            var datos = string.Join("|", idUsuario.ToString(), rol, nombreCompleto);

            var ticket = new FormsAuthenticationTicket(
                1,
                nombreUsuario,
                DateTime.Now,
                DateTime.Now.AddMinutes(60),
                false,
                datos,
                FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Path = FormsAuthentication.FormsCookiePath
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void CerrarSesion()
        {
            FormsAuthentication.SignOut();
        }

        public static IPrincipal ConstruirPrincipal(FormsAuthenticationTicket ticket)
        {
            var datos = (ticket.UserData ?? string.Empty).Split('|');
            var rol = datos.Length > 1 ? datos[1] : string.Empty;

            var identidad = new UsuarioIdentidad(ticket.Name)
            {
                IdUsuario = datos.Length > 0 ? ConvertirEntero(datos[0]) : 0,
                NombreCompleto = datos.Length > 2 ? datos[2] : ticket.Name,
                Rol = rol
            };

            return new GenericPrincipal(identidad, string.IsNullOrEmpty(rol) ? new string[0] : new[] { rol });
        }

        private static int ConvertirEntero(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado) ? resultado : 0;
        }
    }

    public class UsuarioIdentidad : GenericIdentity
    {
        public UsuarioIdentidad(string nombre) : base(nombre, "Forms")
        {
        }

        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
    }
}
