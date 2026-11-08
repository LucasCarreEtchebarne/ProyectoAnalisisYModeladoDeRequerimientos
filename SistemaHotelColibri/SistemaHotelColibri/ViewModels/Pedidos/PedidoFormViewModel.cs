using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;

namespace SistemaHotelColibri.ViewModels.Pedidos;

public class PedidoFormViewModel
{
    public int IdPedido { get; set; }

    [Required(ErrorMessage = "Seleccioná el tipo de pedido.")]
    [Display(Name = "Tipo de pedido")]
    public string? TipoPedido { get; set; }

    [Display(Name = "Mesa")]
    public int? IdMesa { get; set; }

    [Display(Name = "Reserva en check-in")]
    public int? IdReserva { get; set; }

    [Display(Name = "Cliente")]
    public int? IdCliente { get; set; }

    [StringLength(250, ErrorMessage = "Las observaciones pueden tener hasta 250 caracteres.")]
    public string? Observaciones { get; set; }

    public List<LineaPedidoFormViewModel> Detalles { get; set; } = [];

    [BindNever]
    public List<SelectListItem> Mesas { get; set; } = [];

    [BindNever]
    public List<SelectListItem> Reservas { get; set; } = [];

    [BindNever]
    public List<SelectListItem> Clientes { get; set; } = [];

    [BindNever]
    public List<SelectListItem> Productos { get; set; } = [];

    public static PedidoFormViewModel DesdeDto(PedidoDto pedido) => new()
    {
        IdPedido = pedido.IdPedido,
        TipoPedido = pedido.TipoPedido,
        IdMesa = pedido.IdMesa,
        IdReserva = pedido.IdReserva,
        IdCliente = pedido.IdCliente,
        Observaciones = pedido.Observaciones,
        Detalles = pedido.Detalles.Select(detalle => new LineaPedidoFormViewModel
        {
            IdProductoMenu = detalle.IdProductoMenu,
            Cantidad = detalle.Cantidad
        }).ToList()
    };

    public PedidoDto ADto(int idUsuario)
    {
        return new PedidoDto
        {
            IdPedido = IdPedido,
            IdUsuario = idUsuario,
            TipoPedido = TipoPedido ?? string.Empty,
            IdMesa = IdMesa,
            IdReserva = IdReserva,
            IdCliente = IdCliente,
            Observaciones = Observaciones,
            Detalles = Detalles.Select(linea => new DetallePedidoDto
            {
                IdProductoMenu = linea.IdProductoMenu,
                Cantidad = linea.Cantidad
            }).ToList()
        };
    }
}

public class LineaPedidoFormViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná un producto en cada línea.")]
    public int IdProductoMenu { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; } = 1;
}