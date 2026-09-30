namespace SistemaHotelColibri.Abstract.Comun;

public static class Estados
{
    public static class Usuario
    {
        public const string Activo = "Activo";
        public const string Inactivo = "Inactivo";
        public static readonly string[] Todos = [Activo, Inactivo];
    }

    public static class Rol
    {
        public const string Activo = "Activo";
        public const string Inactivo = "Inactivo";
        public static readonly string[] Todos = [Activo, Inactivo];
    }

    public static class Cliente
    {
        public const string Activo = "Activo";
        public const string Inactivo = "Inactivo";
        public static readonly string[] Todos = [Activo, Inactivo];
    }

    public static class Habitacion
    {
        public const string Disponible = "Disponible";
        public const string Ocupada = "Ocupada";
        public const string Limpieza = "Limpieza";
        public const string Mantenimiento = "Mantenimiento";
        public static readonly string[] Todos = [Disponible, Ocupada, Limpieza, Mantenimiento];
    }

    public static class Mesa
    {
        public const string Disponible = "Disponible";
        public const string Ocupada = "Ocupada";
        public const string Reservada = "Reservada";
        public static readonly string[] Todos = [Disponible, Ocupada, Reservada];
    }

    public static class ReservaMesa
    {
        public const string Pendiente = "Pendiente";
        public const string Confirmada = "Confirmada";
        public const string Atendida = "Atendida";
        public const string Cancelada = "Cancelada";
        public static readonly string[] Todos = [Pendiente, Confirmada, Atendida, Cancelada];
    }

    public static class ProductoMenu
    {
        public const string Disponible = "Disponible";
        public const string Agotado = "Agotado";
        public const string Inactivo = "Inactivo";
        public static readonly string[] Todos = [Disponible, Agotado, Inactivo];
    }

    public static class ProductoInventario
    {
        public const string Activo = "Activo";
        public const string Inactivo = "Inactivo";
        public static readonly string[] Todos = [Activo, Inactivo];
    }

    public static class EspacioEvento
    {
        public const string Activo = "Activo";
        public const string Inactivo = "Inactivo";
        public static readonly string[] Todos = [Activo, Inactivo];
    }

    public static class Pedido
    {
        public const string Pendiente = "Pendiente";
        public const string Confirmado = "Confirmado";
        public const string Entregado = "Entregado";
        public const string Cancelado = "Cancelado";
        public static readonly string[] Todos = [Pendiente, Confirmado, Entregado, Cancelado];
    }

    public static class Reserva
    {
        public const string Pendiente = "Pendiente";
        public const string Confirmada = "Confirmada";
        public const string CheckIn = "CheckIn";
        public const string CheckOut = "CheckOut";
        public const string Cancelada = "Cancelada";
        public static readonly string[] Todos = [Pendiente, Confirmada, CheckIn, CheckOut, Cancelada];
    }

    public static class Tarea
    {
        public const string Pendiente = "Pendiente";
        public const string EnProceso = "EnProceso";
        public const string Completada = "Completada";
        public static readonly string[] Todos = [Pendiente, EnProceso, Completada];
    }

    public static class Evento
    {
        public const string Programado = "Programado";
        public const string Realizado = "Realizado";
        public const string Cancelado = "Cancelado";
        public static readonly string[] Todos = [Programado, Realizado, Cancelado];
    }

    public static class Factura
    {
        public const string Pendiente = "Pendiente";
        public const string Pagada = "Pagada";
        public const string Anulada = "Anulada";
        public static readonly string[] Todos = [Pendiente, Pagada, Anulada];
    }

    public static class Pago
    {
        public const string Aprobado = "Aprobado";
        public const string Rechazado = "Rechazado";
        public const string Reversado = "Reversado";
        public static readonly string[] Todos = [Aprobado, Rechazado, Reversado];
    }
}
