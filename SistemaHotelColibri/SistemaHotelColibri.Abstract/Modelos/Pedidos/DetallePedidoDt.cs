namespace SistemaHotelColibri.Abstract.Modelos.Pedidos;

public class DetallePedidoDto
{
    public int IdDetallePedido { get; set; }

    public int IdProductoMenu { get; set; }

    public string NombreProducto { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal => Cantidad * PrecioUnitario;
}