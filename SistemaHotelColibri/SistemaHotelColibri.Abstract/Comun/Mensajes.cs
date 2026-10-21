namespace SistemaHotelColibri.Abstract.Comun;

public static class Mensajes
{
    public const string CreadoOk = "El registro se creó correctamente.";
    public const string ActualizadoOk = "Los cambios se guardaron correctamente.";
    public const string EliminadoOk = "El registro se inactivó correctamente.";
    public const string NoEncontrado = "El registro solicitado no existe.";
    public const string YaInactivo = "El registro ya se encuentra inactivo.";
    public const string RevisarFormulario = "Revise los datos marcados en el formulario.";

    public const string CampoObligatorio = "El campo {0} es obligatorio.";
    public const string LongitudMaxima = "El campo {0} admite como máximo {1} caracteres.";
    public const string ValorNoNegativo = "El campo {0} no puede ser negativo.";
    public const string RangoValores = "El campo {0} debe estar entre {1} y {2}.";

    public const string ProductoRegistrado = "El producto «{0}» se registró correctamente.";
    public const string ProductoDuplicado = "Ya existe un producto de inventario con ese nombre.";
    public const string UnidadMedidaInvalida = "La unidad de medida seleccionada no es válida.";
    public const string StockNegativo = "El stock no puede ser negativo.";
    public const string StockMinimoNegativo = "El stock mínimo no puede ser negativo.";
    public const string CategoriaInvalida = "Seleccione una categoría válida.";

    public const string HabitacionRegistrada = "Se ha registrado la habitación correctamente.";
    public const string HabitacionDuplicada = "Esta habitación ya se encuentra registrada.";
    public const string HabitacionModificadaCorrectamente = "Habitación modificada correctamente.";
    public const string ErrorModificarHabitacion = "No se pudo modificar la habitación, intente nuevamente.";
    public const string HabitacionNoDisponible = "La habitación no se encuentra disponible para modificar la reserva.";
    public const string CantidadCamasInvalida = "La cantidad de camas debe ser mayor a cero.";
    public const string TipoCamaInvalido = "El tipo de cama seleccionado no es válido.";
    public const string VistaInvalida = "La vista seleccionada no es válida.";
    public const string NoExistenHabitaciones = "No existen habitaciones registradas.";    public const string TipoHabitacionInvalido = "El tipo de habitación seleccionado no es válido.";
    public const string EstadoHabitacionInvalido = "El estado de habitación seleccionado no es válido.";
    public const string CapacidadInvalida = "La capacidad debe ser mayor a cero.";
    public const string PrecioInvalido = "El precio no puede ser menor a 0.";
    public const string PisoInvalido = "El piso no puede ser negativo.";
    public const string HabitacionEliminadaCorrectamente = "Habitación inactivada correctamente.";
    public const string ErrorEliminarHabitacion = "No se pudo inactivar la habitación, intente nuevamente.";
    public const string HabitacionInactiva = "La habitación está inactiva y no se puede modificar.";

    public const string FormatoInvalido = "El campo {0} no tiene un formato válido.";
    public const string CorreoInvalido = "El correo electrónico no tiene un formato válido.";
    public const string ClienteRegistrado = "El cliente se registró correctamente con el código #{0}.";
    public const string ClienteDuplicado = "Ya existe un cliente registrado con esa identificación.";
    public const string CorreoClienteDuplicado = "Ya existe un cliente registrado con ese correo electrónico.";
    public const string ClienteModificado = "Los datos del cliente se actualizaron correctamente.";
    public const string ClienteEliminado = "El cliente se inactivó correctamente.";
    public const string ClienteYaInactivo = "El cliente ya se encuentra inactivo.";
    public const string EstadoClienteInvalido = "El estado de cliente seleccionado no es válido.";
    public const string NoExistenClientes = "No existen clientes registrados.";
    public const string SinResultadosBusqueda = "No se encontraron clientes que coincidan con la búsqueda.";

    public const string CredencialesInvalidas = "El usuario o la contraseña no son correctos.";
    public const string UsuarioInactivo = "El usuario está inactivo. Contacte al administrador.";
    public const string UsuarioBloqueadoInicio = "El usuario está bloqueado por intentos fallidos o por el administrador. Intente más tarde o contacte al administrador.";
    public const string SesionCerrada = "La sesión se cerró correctamente.";
    public const string DebeCambiarContrasena = "Debe cambiar su contraseña antes de continuar.";
    public const string ContrasenaCambiada = "La contraseña se cambió correctamente.";
    public const string ContrasenaActualIncorrecta = "La contraseña actual no es correcta.";
    public const string ContrasenaIgualActual = "La nueva contraseña debe ser distinta de la actual.";
    public const string ErrorEnvioCorreo = "No se pudo enviar el correo de recuperación. Intente más tarde o contacte al administrador.";
    public const string RecuperacionEnviada = "Si el correo pertenece a un usuario activo, recibirá un enlace para restablecer la contraseña.";
    public const string EnlaceRecuperacionInvalido = "El enlace para restablecer la contraseña no es válido o ya venció. Solicite uno nuevo.";
    public const string ContrasenaRestablecida = "La contraseña se restableció correctamente. Ya puede iniciar sesión.";
    public const string ContrasenaTemporalGenerada = "Contraseña temporal de {0}: {1}. Entréguela al usuario; deberá cambiarla al iniciar sesión.";

    public const string UsuarioRegistrado = "El usuario «{0}» se registró correctamente. Deberá cambiar la contraseña en su primer inicio de sesión.";
    public const string UsuarioModificado = "Los datos del usuario se actualizaron correctamente.";
    public const string UsuarioInactivado = "El usuario se inactivó correctamente.";
    public const string UsuarioActivado = "El usuario se activó nuevamente.";
    public const string UsuarioYaActivo = "El usuario ya se encuentra activo.";
    public const string UsuarioBloqueado = "El usuario se bloqueó correctamente.";
    public const string UsuarioDesbloqueado = "El usuario se desbloqueó correctamente.";
    public const string NombreUsuarioDuplicado = "Ya existe un usuario con ese nombre de usuario.";
    public const string CorreoUsuarioDuplicado = "Ya existe un usuario con ese correo electrónico.";
    public const string RolInvalido = "Seleccione un rol válido.";
    public const string NombreUsuarioInvalido = "El nombre de usuario solo admite letras, números, punto, guion y guion bajo.";
    public const string AccionSobreUsuarioPropio = "No puede inactivar, bloquear ni cambiar el rol de su propio usuario.";
    public const string ReservaRegistrada = "Se ha registrado la reserva correctamente.";
    public const string ErrorRegistrarReserva = "No se pudo registrar la reserva, intente nuevamente.";
    public const string ReservaModificada = "Reserva modificada correctamente.";
    public const string ErrorModificarReserva = "No se pudo modificar la reserva, intente nuevamente.";
    public const string ReservaCancelada = "Reserva cancelada correctamente.";
    public const string ErrorCancelarReserva = "No se pudo cancelar la reserva, intente nuevamente.";
    public const string ReservaYaCancelada = "La reserva ya se encuentra cancelada.";
    public const string ReservaYaEmpezo = "No se puede cancelar una reserva que ya empezó.";
    public const string ReservaHabitacionNoDisponible = "La habitación no se encuentra disponible para las fechas seleccionadas.";
    public const string ReservaCamposIncompletos = "Se debe completar la información faltante.";
    public const string ClienteNoRegistrado = "El cliente no se encuentra registrado.";
    public const string ClienteReservaInactivo = "El cliente seleccionado está inactivo.";
    public const string HabitacionReservaInactiva = "La habitación seleccionada está inactiva.";
    public const string FechasReservaInvalidas = "La fecha de salida debe ser posterior a la fecha de entrada.";
    public const string FechaEntradaPasada = "La fecha de entrada no puede ser anterior a hoy.";
    public const string HuespedesExcedenCapacidad = "La cantidad de huéspedes excede la capacidad de la habitación ({0}).";
    public const string CantidadHuespedesInvalida = "La cantidad de huéspedes debe ser mayor a cero.";
    public const string EstadoReservaInvalido = "El estado de reserva seleccionado no es válido.";
    public const string NoExistenReservas = "No existen reservas registradas.";
    public const string UsuarioInactivoNoEditable = "El usuario está inactivo. Actívelo para poder modificarlo.";
}
