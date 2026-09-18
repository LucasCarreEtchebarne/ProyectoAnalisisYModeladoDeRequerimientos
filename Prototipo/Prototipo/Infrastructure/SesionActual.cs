using System.Web;

namespace Prototipo.Infrastructure
{
    public static class SesionActual
    {
        private static UsuarioIdentidad Identidad
        {
            get
            {
                var contexto = HttpContext.Current;
                if (contexto == null || contexto.User == null)
                {
                    return null;
                }

                return contexto.User.Identity as UsuarioIdentidad;
            }
        }

        public static bool Autenticado
        {
            get { return Identidad != null && Identidad.IsAuthenticated; }
        }

        public static int IdUsuario
        {
            get { return Identidad == null ? 0 : Identidad.IdUsuario; }
        }

        public static string NombreUsuario
        {
            get { return Identidad == null ? string.Empty : Identidad.Name; }
        }

        public static string NombreCompleto
        {
            get { return Identidad == null ? string.Empty : Identidad.NombreCompleto; }
        }

        public static string Rol
        {
            get { return Identidad == null ? string.Empty : Identidad.Rol; }
        }
    }
}
