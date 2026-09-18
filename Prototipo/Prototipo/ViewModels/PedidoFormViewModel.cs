using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Prototipo.Models;

namespace Prototipo.ViewModels
{
    public class PedidoFormViewModel
    {
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "El tipo de pedido es obligatorio.")]
        [Display(Name = "Tipo de pedido")]
        public string TipoPedido { get; set; }

        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        [Display(Name = "Mesa")]
        public int? IdMesa { get; set; }

        [Display(Name = "Habitacion")]
        public int? IdHabitacion { get; set; }

        [StringLength(250)]
        public string Observaciones { get; set; }

        public List<LineaPedidoViewModel> Lineas { get; set; }

        public List<Cliente> Clientes { get; set; }
        public List<Mesa> Mesas { get; set; }
        public List<Habitacion> HabitacionesOcupadas { get; set; }
        public List<ProductoMenu> Menu { get; set; }

        public PedidoFormViewModel()
        {
            Lineas = new List<LineaPedidoViewModel>();
            Clientes = new List<Cliente>();
            Mesas = new List<Mesa>();
            HabitacionesOcupadas = new List<Habitacion>();
            Menu = new List<ProductoMenu>();
        }
    }

    public class LineaPedidoViewModel
    {
        public int IdProductoMenu { get; set; }
        public int Cantidad { get; set; }
    }
}
