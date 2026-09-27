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
}
