using System;
using System.Collections.Generic;
using System.Linq;
using Prototipo.Common;
using Prototipo.Data;
using Prototipo.Models;

namespace Prototipo.Services
{
    public class ClienteService : ServicioBase
    {
        private readonly BitacoraService _bitacora;

        public ClienteService(HotelColibriContext db) : base(db)
        {
            _bitacora = new BitacoraService(db);
        }

        public List<Cliente> Listar(string estado, string busqueda)
        {
            var consulta = Db.Clientes.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(c => c.EstadoCliente == estado);
            }

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                consulta = consulta.Where(c => c.NombreCompleto.Contains(busqueda)
                                            || c.PrimerApellido.Contains(busqueda)
                                            || c.SegundoApellido.Contains(busqueda)
                                            || c.Identificacion.Contains(busqueda));
            }

            return consulta.OrderBy(c => c.PrimerApellido).ThenBy(c => c.NombreCompleto).ToList();
        }

        public List<Cliente> ListarActivos()
        {
            return Db.Clientes
                .Where(c => c.EstadoCliente == Estados.Cliente.Activo)
                .OrderBy(c => c.PrimerApellido)
                .ToList();
        }

        public Cliente Obtener(int id)
        {
            return Db.Clientes.FirstOrDefault(c => c.IdCliente == id);
        }

        public ResultadoOperacion Crear(Cliente cliente)
        {
            if (Db.Clientes.Any(c => c.Identificacion == cliente.Identificacion))
            {
                return ResultadoOperacion.Error(Mensajes.ClienteDuplicado);
            }

            cliente.FechaRegistro = DateTime.Now;
            if (string.IsNullOrEmpty(cliente.EstadoCliente))
            {
                cliente.EstadoCliente = Estados.Cliente.Activo;
            }

            Db.Clientes.Add(cliente);
            Db.SaveChanges();

            _bitacora.Registrar(Estados.Modulos.Clientes, Estados.Acciones.Crear, cliente.IdCliente,
                "Cliente creado: " + cliente.NombreMostrar);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.CreadoOk, cliente.IdCliente);
        }

        public ResultadoOperacion Actualizar(Cliente datos)
        {
            var cliente = Obtener(datos.IdCliente);
            if (cliente == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            if (Db.Clientes.Any(c => c.IdCliente != datos.IdCliente && c.Identificacion == datos.Identificacion))
            {
                return ResultadoOperacion.Error(Mensajes.ClienteDuplicado);
            }

            cliente.Identificacion = datos.Identificacion;
            cliente.NombreCompleto = datos.NombreCompleto;
            cliente.PrimerApellido = datos.PrimerApellido;
            cliente.SegundoApellido = datos.SegundoApellido;
            cliente.Telefono = datos.Telefono;
            cliente.CorreoElectronico = datos.CorreoElectronico;
            cliente.Direccion = datos.Direccion;
            cliente.EstadoCliente = datos.EstadoCliente;

            _bitacora.Registrar(Estados.Modulos.Clientes, Estados.Acciones.Editar, cliente.IdCliente,
                "Cliente actualizado: " + cliente.NombreMostrar);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.ActualizadoOk, cliente.IdCliente);
        }

        public ResultadoOperacion Inactivar(int id)
        {
            var cliente = Obtener(id);
            if (cliente == null)
            {
                return ResultadoOperacion.Error(Mensajes.NoEncontrado);
            }

            cliente.EstadoCliente = Estados.Cliente.Inactivo;

            _bitacora.Registrar(Estados.Modulos.Clientes, Estados.Acciones.Inactivar, cliente.IdCliente,
                "Cliente inactivado: " + cliente.NombreMostrar);
            Db.SaveChanges();

            return ResultadoOperacion.Ok(Mensajes.InactivadoOk, cliente.IdCliente);
        }
    }
}
