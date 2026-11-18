using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaHotelColibri.Abstract.Comun;
using SistemaHotelColibri.Abstract.Modelos.Menus;
using SistemaHotelColibri.Abstract.Modelos.Pedidos;
using SistemaHotelColibri.ViewModels.Comun;

namespace SistemaHotelColibri.ViewModels.Salon;

// Página de Pedidos: tarjetas de resumen y pestañas con los cuadritos de mesas, habitaciones y para llevar.
public class SalonViewModel
{
    public const string VistaMesas = "mesas";
    public const string VistaHabitaciones = "habitaciones";
    public const string VistaLlevar = "llevar";

    public string Vista { get; set; } = VistaMesas;
    public List<TarjetaResumenViewModel> Resumen { get; set; } = [];
    public List<CuadroViewModel> Mesas { get; set; } = [];
    public List<CuadroViewModel> Habitaciones { get; set; } = [];
    public List<CuadroViewModel> ParaLlevar { get; set; } = [];
}

// Un cuadrito: una mesa, una habitación o un pedido para llevar.
public class CuadroViewModel
{
    public string Tipo { get; set; } = Catalogos.TipoPedido.Mesa;
    public int Id { get; set; }
    public string Etiqueta { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public int? Capacidad { get; set; }

    // libre, en-uso, reservada u hospedada (clases de CSS)
    public string Estado { get; set; } = "libre";
    public string? Nota { get; set; }
    public PedidoDto? Cuenta { get; set; }

    public int Productos => Cuenta?.Detalles.Sum(detalle => detalle.Cantidad) ?? 0;

    public int MinutosAbierta => Cuenta == null ? 0 : (int)Math.Max(0, (DateTime.Now - Cuenta.FechaHoraPedido).TotalMinutes);
}

// Pantalla para tomar el pedido: menú por categorías a la izquierda y la cuenta a la derecha.
public class CuentaViewModel
{
    public const string TodasLasCategorias = "Todas";

    public string Tipo { get; set; } = Catalogos.TipoPedido.Mesa;
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Subtitulo { get; set; } = string.Empty;
    public string VistaRegreso { get; set; } = SalonViewModel.VistaMesas;
    public PedidoDto? Cuenta { get; set; }
    public List<string> Categorias { get; set; } = [];
    public string CategoriaSeleccionada { get; set; } = TodasLasCategorias;
    public List<MenuDto> Productos { get; set; } = [];

    public string? CategoriaRuta => CategoriaSeleccionada == TodasLasCategorias ? null : CategoriaSeleccionada;

    public IEnumerable<MenuDto> ProductosVisibles =>
        CategoriaSeleccionada == TodasLasCategorias
            ? Productos
            : Productos.Where(producto => producto.CategoriaMenu == CategoriaSeleccionada);

    public int CantidadEnCuenta(int idProductoMenu) =>
        Cuenta?.Detalles.Where(detalle => detalle.IdProductoMenu == idProductoMenu).Sum(detalle => detalle.Cantidad) ?? 0;

    public static string IconoCategoria(string categoria)
    {
        var texto = categoria.ToLowerInvariant();
        if (texto == "todas") return "bi-grid-3x3-gap";
        if (texto.Contains("entrada")) return "bi-egg-fried";
        if (texto.Contains("acompa")) return "bi-basket";
        if (texto.Contains("roja")) return "bi-fire";
        if (texto.Contains("blanca")) return "bi-feather";
        if (texto.Contains("casa") || texto.Contains("fuerte")) return "bi-star";
        if (texto.Contains("postre")) return "bi-cake2";
        if (texto.Contains("cerveza") || texto.Contains("vino") || texto.Contains("coctel")) return "bi-cup-straw";
        if (texto.Contains("caliente")) return "bi-cup-hot";
        if (texto.Contains("fría") || texto.Contains("fria") || texto.Contains("bebida")) return "bi-cup";
        if (texto.Contains("desayuno")) return "bi-sunrise";
        return "bi-tag";
    }

    // Cada categoría tiene un color fijo para reconocerla rápido.
    public static string ColorCategoria(string categoria)
    {
        string[] colores = ["verde", "ambar", "coral", "cielo", "violeta", "turquesa", "azul"];
        var indice = Array.IndexOf(Catalogos.CategoriasMenu, categoria);
        if (indice < 0)
        {
            indice = categoria.Sum(letra => letra);
        }

        return colores[indice % colores.Length];
    }

    public static string VistaDe(string tipo) => tipo switch
    {
        Catalogos.TipoPedido.Habitacion => SalonViewModel.VistaHabitaciones,
        Catalogos.TipoPedido.Llevar => SalonViewModel.VistaLlevar,
        _ => SalonViewModel.VistaMesas
    };
}

// Formulario para cobrar una cuenta.
public class CobroViewModel
{
    [BindNever]
    public string Tipo { get; set; } = Catalogos.TipoPedido.Mesa;

    [BindNever]
    public int Id { get; set; }

    [BindNever]
    public string Titulo { get; set; } = string.Empty;

    [BindNever]
    public PedidoDto? Cuenta { get; set; }

    // Subtotal, servicio 10 %, IVA 13 % y total, calculados como en Facturación.
    [BindNever]
    public TotalesFactura? Totales { get; set; }

    [BindNever]
    public List<SelectListItem> Clientes { get; set; } = [];

    [Display(Name = "Cliente")]
    public int? IdCliente { get; set; }

    [Required(ErrorMessage = "Elegí el método de pago.")]
    [Display(Name = "Método de pago")]
    public string MetodoPago { get; set; } = Facturacion.MetodoPago.Efectivo;

    [Display(Name = "Monto recibido")]
    [Range(0, 99999999, ErrorMessage = "El monto recibido no es válido.")]
    public decimal? MontoRecibido { get; set; }

    [Display(Name = "Tipo de tarjeta")]
    public string? TipoTarjeta { get; set; }

    [Display(Name = "Últimos 4 dígitos")]
    [RegularExpression("^[0-9]{4}$", ErrorMessage = "Escribí solo los últimos 4 dígitos.")]
    public string? UltimosCuatroDigitos { get; set; }

    [Display(Name = "Número de comprobante")]
    [StringLength(50, ErrorMessage = "El comprobante puede tener hasta 50 caracteres.")]
    public string? NumeroReferencia { get; set; }
}
