namespace SistemaHotelColibri.Abstract.Comun;

public static class Mensajes
{
    public const string CreadoOk = "El registro se creó correctamente.";
    public const string ActualizadoOk = "Los cambios se guardaron correctamente.";
    public const string EliminadoOk = "El registro se eliminó correctamente.";
    public const string NoEncontrado = "El registro solicitado no existe.";
    public const string YaInactivo = "El registro ya se encuentra inactivo.";
    public const string RevisarFormulario = "Revise los datos marcados en el formulario.";

    public const string CampoObligatorio = "El campo {0} es obligatorio.";
    public const string LongitudMaxima = "El campo {0} admite como máximo {1} caracteres.";
    public const string ValorNoNegativo = "El campo {0} no puede ser negativo.";

    public const string ProductoRegistrado = "El producto se registró con el código #{0}.";
    public const string ProductoDuplicado = "Ya existe un producto de inventario con ese nombre.";
    public const string UnidadMedidaInvalida = "La unidad de medida seleccionada no es válida.";
    public const string StockNegativo = "El stock no puede ser negativo.";
    public const string StockMinimoNegativo = "El stock mínimo no puede ser negativo.";

    public const string HabitacionRegistrada = "Se ha registrado la habitación correctamente.";
    public const string HabitacionDuplicada = "Esta habitación ya se encuentra registrada.";
    public const string HabitacionModificadaCorrectamente = "Habitación modificada correctamente.";
    public const string ErrorModificarHabitacion = "No se pudo modificar la habitación, intente nuevamente.";
    public const string HabitacionNoDisponible = "La habitación no se encuentra disponible para modificar la reserva.";
    public const string NoExistenHabitaciones = "No existen habitaciones registradas.";
    public const string TipoHabitacionInvalido = "El tipo de habitación seleccionado no es válido.";
    public const string EstadoHabitacionInvalido = "El estado de habitación seleccionado no es válido.";
    public const string CapacidadInvalida = "La capacidad debe ser mayor a cero.";
    public const string PrecioInvalido = "El precio no puede ser menor a 0.";
    public const string PisoInvalido = "El piso no puede ser negativo.";
    public const string HabitacionEliminadaCorrectamente = "Habitación eliminada correctamente.";
    public const string ErrorEliminarHabitacion = "No se pudo eliminar la habitación, intente nuevamente.";

    public const string FormatoInvalido = "El campo {0} no tiene un formato válido.";
    public const string CorreoInvalido = "El correo electrónico no tiene un formato válido.";
    public const string ClienteRegistrado = "El cliente se registró correctamente con el código #{0}.";
    public const string ClienteDuplicado = "Ya existe un cliente registrado con esa identificación.";
    public const string ClienteModificado = "Los datos del cliente se actualizaron correctamente.";
    public const string ClienteEliminado = "El cliente se eliminó correctamente.";
    public const string ClienteYaInactivo = "El cliente ya se encuentra inactivo.";
    public const string EstadoClienteInvalido = "El estado de cliente seleccionado no es válido.";
    public const string NoExistenClientes = "No existen clientes registrados.";
    public const string SinResultadosBusqueda = "No se encontraron clientes que coincidan con la búsqueda.";
}
