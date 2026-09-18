using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Prototipo.Data;
using Prototipo.Infrastructure;
using Prototipo.Services;

namespace Prototipo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SembrarContrasenasDesarrollo();
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                return;
            }

            try
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket == null || ticket.Expired)
                {
                    return;
                }

                Context.User = Seguridad.ConstruirPrincipal(ticket);
            }
            catch (Exception)
            {
                FormsAuthentication.SignOut();
            }
        }

        private static void SembrarContrasenasDesarrollo()
        {
            var contrasena = ConfigurationManager.AppSettings["ContrasenaSeedDesarrollo"];
            if (string.IsNullOrWhiteSpace(contrasena))
            {
                return;
            }

            try
            {
                using (var db = new HotelColibriContext())
                {
                    new UsuarioService(db).SembrarContrasenasDesarrollo(contrasena);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
