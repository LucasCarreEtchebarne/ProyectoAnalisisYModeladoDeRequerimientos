using System.Collections.Generic;

namespace Prototipo.Common
{
    public static class Estados
    {
        public static class Usuario
        {
            public const string Activo = "Activo";
            public const string Inactivo = "Inactivo";
            public const string Bloqueado = "Bloqueado";
            public static readonly string[] Todos = { Activo, Inactivo, Bloqueado };
        }

        public static class Cliente
        {
            public const string Activo = "Activo";
            public const string Inactivo = "Inactivo";
            public static readonly string[] Todos = { Activo, Inactivo };
        }

        public static class Habitacion
        {
            public const string Disponible = "Disponible";
            public const string Ocupada = "Ocupada";
            public const string Limpieza = "Limpieza";
            public const string Mantenimiento = "Mantenimiento";
            public static readonly string[] Todos = { Disponible, Ocupada, Limpieza, Mantenimiento };
        }

        public static class Mesa
        {
            public const string Disponible = "Disponible";
            public const string Ocupada = "Ocupada";
            public const string Reservada = "Reservada";
            public static readonly string[] Todos = { Disponible, Ocupada, Reservada };
        }

        public static class ReservaMesa
        {
            public const string Pendiente = "Pendiente";
            public const string Confirmada = "Confirmada";
            public const string Atendida = "Atendida";
            public const string Cancelada = "Cancelada";
            public static readonly string[] Todos = { Pendiente, Confirmada, Atendida, Cancelada };
        }

        public static class ProductoMenu
        {
            public const string Disponible = "Disponible";
            public const string Agotado = "Agotado";
            public const string Inactivo = "Inactivo";
            public static readonly string[] Todos = { Disponible, Agotado, Inactivo };
        }

        public static class Producto
        {
            public const string Activo = "Activo";
            public const string Inactivo = "Inactivo";
            public static readonly string[] Todos = { Activo, Inactivo };
        }

        public static class Movimiento
        {
            public const string Entrada = "Entrada";
            public const string Salida = "Salida";
            public const string Ajuste = "Ajuste";
            public static readonly string[] Todos = { Entrada, Salida, Ajuste };
        }

        public static class Pedido
        {
            public const string Pendiente = "Pendiente";
            public const string Confirmado = "Confirmado";
            public const string Entregado = "Entregado";
            public const string Cancelado = "Cancelado";
            public static readonly string[] Todos = { Pendiente, Confirmado, Entregado, Cancelado };
        }

        public static class TipoPedido
        {
            public const string Mesa = "Mesa";
            public const string Habitacion = "Habitacion";
            public const string Llevar = "Llevar";
            public static readonly string[] Todos = { Mesa, Habitacion, Llevar };
        }

        public static class Reserva
        {
            public const string Pendiente = "Pendiente";
            public const string Confirmada = "Confirmada";
            public const string CheckIn = "CheckIn";
            public const string CheckOut = "CheckOut";
            public const string Cancelada = "Cancelada";
            public static readonly string[] Todos = { Pendiente, Confirmada, CheckIn, CheckOut, Cancelada };
        }

        public static class Tarea
        {
            public const string Pendiente = "Pendiente";
            public const string EnProceso = "EnProceso";
            public const string Completada = "Completada";
            public static readonly string[] Todos = { Pendiente, EnProceso, Completada };
        }

        public static class Evento
        {
            public const string Programado = "Programado";
            public const string Realizado = "Realizado";
            public const string Cancelado = "Cancelado";
            public static readonly string[] Todos = { Programado, Realizado, Cancelado };
        }

        public static class Espacio
        {
            public const string Activo = "Activo";
            public const string Inactivo = "Inactivo";
            public static readonly string[] Todos = { Activo, Inactivo };
        }

        public static class TipoFactura
        {
            public const string Hospedaje = "Hospedaje";
            public const string Restaurante = "Restaurante";
            public const string Evento = "Evento";
            public const string Mixta = "Mixta";
            public static readonly string[] Todos = { Hospedaje, Restaurante, Evento, Mixta };
        }

        public static class Factura
        {
            public const string Pendiente = "Pendiente";
            public const string Pagada = "Pagada";
            public const string Anulada = "Anulada";
            public static readonly string[] Todos = { Pendiente, Pagada, Anulada };
        }

        public static class TipoConcepto
        {
            public const string Hospedaje = "Hospedaje";
            public const string Restaurante = "Restaurante";
            public const string Evento = "Evento";
            public const string Otro = "Otro";
            public static readonly string[] Todos = { Hospedaje, Restaurante, Evento, Otro };
        }

        public static class MetodoPago
        {
            public const string Efectivo = "Efectivo";
            public const string Tarjeta = "Tarjeta";
            public const string Transferencia = "Transferencia";
            public const string SINPE = "SINPE";
            public static readonly string[] Todos = { Efectivo, Tarjeta, Transferencia, SINPE };
        }

        public static class TipoTarjeta
        {
            public static readonly string[] Todos = { "Visa", "MasterCard", "AmericanExpress", "Otra" };
        }

        public static class Pago
        {
            public const string Aprobado = "Aprobado";
            public const string Rechazado = "Rechazado";
            public const string Reversado = "Reversado";
            public static readonly string[] Todos = { Aprobado, Rechazado, Reversado };
        }

        public static class Roles
        {
            public const string Administrador = "Administrador";
            public const string Recepcionista = "Recepcionista";
            public const string Mesero = "Mesero";
            public const string Housekeeping = "Housekeeping";
            public static readonly string[] Todos = { Administrador, Recepcionista, Mesero, Housekeeping };
        }

        public static class Modulos
        {
            public const string Hospedaje = "HRE";
            public const string Restaurante = "RPV";
            public const string Inventario = "INV";
            public const string Clientes = "CLI";
            public const string Housekeeping = "HSK";
            public const string Facturacion = "FAC";
            public const string Usuarios = "USR";
            public const string Reportes = "REP";
            public const string IA = "IA";

            public static readonly Dictionary<string, string> Nombres = new Dictionary<string, string>
            {
                { Hospedaje, "Hospedaje y reservas" },
                { Restaurante, "Restaurante" },
                { Inventario, "Inventario" },
                { Clientes, "Clientes y eventos" },
                { Housekeeping, "Housekeeping" },
                { Facturacion, "Facturacion y pagos" },
                { Usuarios, "Usuarios y seguridad" },
                { Reportes, "Reportes" },
                { IA, "Asistente IA" }
            };
        }

        public static class Acciones
        {
            public const string Crear = "CREAR";
            public const string Editar = "EDITAR";
            public const string Inactivar = "INACTIVAR";
            public const string Anular = "ANULAR";
            public const string IniciarSesion = "INICIAR_SESION";
            public const string CerrarSesion = "CERRAR_SESION";
            public const string Consultar = "CONSULTAR";
        }
    }
}
