using System;
using System.Collections.Generic;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Infrastructure;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class UsuarioService : ServicioBase
    {
        public const int MaximoIntentosFallidos = 3;

        private readonly BitacoraService _bitacora;

        public UsuarioService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<Usuario> Listar(string estado, string rol, string busqueda)
        {
            var consulta = Db.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(u => u.Estado == estado);
            }

            if (!string.IsNullOrEmpty(rol))
            {
                consulta = consulta.Where(u => u.Rol == rol);
            }

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                consulta = consulta.Where(u => u.NombreCompleto.Contains(busqueda)
                                            || u.NombreUsuario.Contains(busqueda)
                                            || u.CorreoElectronico.Contains(busqueda));
            }

            return consulta.OrderBy(u => u.NombreCompleto).ToList();
        }

        public Usuario Obtener(int id)
        {
            return Db.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
        }

        public ResultadoOperacion Autenticar(string nombreUsuario, string contrasena)
        {
            var usuario = Db.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);

            if (usuario == null)
            {
                return ResultadoOperacion.Error(Mensajes.CredencialesInvalidas);
            }

            if (usuario.Estado == Estados.Usuario.Bloqueado)
            {
                return ResultadoOperacion.Error(Mensajes.UsuarioBloqueado);
            }

            if (usuario.Estado == Estados.Usuario.Inactivo)
            {
                return ResultadoOperacion.Error(Mensajes.UsuarioInactivo);
            }

            if (!Seguridad.VerificarContrasena(contrasena, usuario.ContrasenaHash))
            {
                usuario.IntentosFallidos = usuario.IntentosFallidos + 1;

                if (usuario.IntentosFallidos >= MaximoIntentosFallidos)
                {
                    usuario.Estado = Estados.Usuario.Bloqueado;
                    usuario.FechaBloqueo = DateTime.Now;
                    Db.SaveChanges();
                    return ResultadoOperacion.Error(Mensajes.UsuarioBloqueado);
                }

                Db.SaveChanges();
                return ResultadoOperacion.Error(Mensajes.CredencialesInvalidas);
            }

            usuario.IntentosFallidos = 0;
            usuario.FechaUltimoAcceso = DateTime.Now;
            _bitacora.Registrar(Estados.Modulos.Usuarios, Estados.Acciones.IniciarSesion, usuario.IdUsuario,
                "Inicio de sesion de " + usuario.NombreUsuario, usuario.IdUsuario);
            Db.SaveChanges();

            return ResultadoOperacion.Ok("Bienvenido, " + usuario.NombreCompleto, usuario.IdUsuario);
        }

        public void RegistrarCierreSesion()
        {
            if (SesionActual.IdUsuario <= 0)
            {
                return;
            }

            _bitacora.Registrar(Estados.Modulos.Usuarios, Estados.Acciones.CerrarSesion, SesionActual.IdUsuario,
                "Cierre de sesion de " + SesionActual.NombreUsuario);
            Db.SaveChanges();
        }

        public ResultadoOperacion Crear(Usuario usuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(contrasena) || contrasena.Length < 8)
            {
                return ResultadoOperacion.Error(Mensajes.ContrasenaCorta);
            }

            if (Db.Usuarios.Any(u => u.NombreUsuario == usuario.NombreUsuario
                                  || u.CorreoElectronico == usuario.CorreoElectronico))
            {
                return ResultadoOperacion.Error(Mensajes.UsuarioDuplicado);
            }

            usuario.ContrasenaHash = Seguridad.GenerarHash(contrasena);
            usuario.Estado = Estados.Usuario.Activo;
            usuario.IntentosFallidos = 0;
            usuario.FechaCreacion = DateTime.Now;

            Db.Usuarios.Add(usuario);
            Db.SaveChanges();

            _bitacora.Registrar(Estados.Modulos.Usuarios, Estados.Acciones.Crear, usuario.IdUsuario,
                "Usuario creado: " + usuario.NombreUsuario);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.CreadoOk, usuario.IdUsuario);
        }

        public ResultadoOperacion Actualizar(Usuario datos, string contrasenaNueva)
        {
            var usuario = Obtener(datos.IdUsuario);
            if (usuario == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (Db.Usuarios.Any(u => u.IdUsuario != datos.IdUsuario
                                  && (u.NombreUsuario == datos.NombreUsuario
                                   || u.CorreoElectronico == datos.CorreoElectronico)))
            {
                return ResultadoOperacion.Error(Mensajes.UsuarioDuplicado);
            }

            usuario.NombreCompleto = datos.NombreCompleto;
            usuario.NombreUsuario = datos.NombreUsuario;
            usuario.CorreoElectronico = datos.CorreoElectronico;
            usuario.Rol = datos.Rol;

            if (usuario.Estado == Estados.Usuario.Bloqueado && datos.Estado != Estados.Usuario.Bloqueado)
            {
                usuario.IntentosFallidos = 0;
                usuario.FechaBloqueo = null;
            }

            usuario.Estado = datos.Estado;

            if (!string.IsNullOrWhiteSpace(contrasenaNueva))
            {
                if (contrasenaNueva.Length < 8)
                {
                    return ResultadoOperacion.Error(Mensajes.ContrasenaCorta);
                }

                usuario.ContrasenaHash = Seguridad.GenerarHash(contrasenaNueva);
            }

            _bitacora.Registrar(Estados.Modulos.Usuarios, Estados.Acciones.Editar, usuario.IdUsuario,
                "Usuario actualizado: " + usuario.NombreUsuario);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.ActualizadoOk, usuario.IdUsuario);
        }

        public ResultadoOperacion Inactivar(int id)
        {
            var usuario = Obtener(id);
            if (usuario == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            usuario.Estado = Estados.Usuario.Inactivo;

            _bitacora.Registrar(Estados.Modulos.Usuarios, Estados.Acciones.Inactivar, usuario.IdUsuario,
                "Usuario inactivado: " + usuario.NombreUsuario);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.InactivadoOk, usuario.IdUsuario);
        }

        public int SembrarContrasenasDesarrollo(string contrasena)
        {
            var pendientes = Db.Usuarios
                .Where(u => u.ContrasenaHash == "PENDIENTE_SEEDER_BCRYPT"
                         || u.ContrasenaHash.StartsWith("$2a$11$HASH"))
                .ToList();

            if (pendientes.Count == 0)
            {
                return 0;
            }

            var hash = Seguridad.GenerarHash(contrasena);
            foreach (var usuario in pendientes)
            {
                usuario.ContrasenaHash = hash;
                usuario.IntentosFallidos = 0;
            }

            Db.SaveChanges();
            return pendientes.Count;
        }
    }
}
