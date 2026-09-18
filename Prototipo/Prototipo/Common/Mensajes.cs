namespace Prototipo.Common
{
    public static class Mensajes
    {
        public const string CreadoOk = "El registro se creo correctamente.";
        public const string ActualizadoOk = "Los cambios se guardaron correctamente.";
        public const string InactivadoOk = "El registro se inactivo correctamente.";
        public const string NoEncontrado = "El registro solicitado no existe.";
        public const string ErrorGeneral = "Ocurrio un error al procesar la solicitud.";

        public const string CredencialesInvalidas = "Usuario o contrasena incorrectos.";
        public const string UsuarioInactivo = "El usuario esta inactivo. Contacte al administrador.";
        public const string UsuarioBloqueado = "El usuario esta bloqueado por intentos fallidos. Contacte al administrador.";
        public const string UsuarioDuplicado = "Ya existe un usuario con ese nombre de usuario o correo.";
        public const string ContrasenaCorta = "La contrasena debe tener al menos 8 caracteres.";

        public const string ClienteDuplicado = "Ya existe un cliente con esa identificacion.";

        public const string HabitacionDuplicada = "Ya existe una habitacion con ese numero.";
        public const string FechasInvalidas = "La fecha de salida debe ser posterior a la fecha de entrada.";
        public const string HabitacionNoDisponible = "La habitacion ya tiene una reserva que se traslapa con esas fechas.";
        public const string CapacidadExcedida = "La cantidad de huespedes supera la capacidad de la habitacion.";
        public const string ReservaNoConfirmada = "Solo se puede hacer check-in de una reserva confirmada.";
        public const string CheckOutInvalido = "Solo se puede hacer check-out de una reserva en check-in.";

        public const string PedidoSinDetalle = "El pedido debe incluir al menos un producto.";
        public const string PedidoRequiereMesa = "Un pedido de tipo Mesa requiere seleccionar una mesa.";
        public const string PedidoRequiereHabitacion = "Un pedido de tipo Habitacion requiere una habitacion con reserva en check-in.";
        public const string StockInsuficiente = "No hay stock suficiente para preparar el pedido.";
        public const string PedidoYaConfirmado = "El pedido ya fue confirmado y el stock ya se descontó.";
        public const string ProductoDuplicado = "Ya existe un producto con ese nombre.";

        public const string EventoHorasInvalidas = "La hora de fin debe ser posterior a la hora de inicio.";
        public const string EventoTraslapado = "El espacio ya tiene un evento programado en ese horario.";
        public const string EventoCapacidadExcedida = "La cantidad de participantes supera la capacidad del espacio.";

        public const string FacturaSinDetalle = "La factura debe incluir al menos un concepto.";
        public const string FacturaAnulada = "La factura esta anulada y no admite cambios.";
        public const string FacturaPagada = "La factura ya esta pagada.";
        public const string MotivoAnulacionRequerido = "Debe indicar el motivo de la anulacion.";
        public const string PagoExcedeSaldo = "El monto pagado supera el saldo pendiente de la factura.";
        public const string EfectivoInsuficiente = "El monto recibido debe ser mayor o igual al monto a pagar.";
        public const string TarjetaDatosRequeridos = "Para pagos con tarjeta debe indicar el tipo y los ultimos cuatro digitos.";
        public const string ReferenciaRequerida = "Para transferencias y SINPE debe indicar el numero de referencia.";
    }
}
