namespace SistemaHotelColibri.Abstract.Comun;

public static class Facturacion
{
    public const decimal TasaIva = 0.13m;
    public const decimal TasaServicio = 0.10m;

    public static class Tipo
    {
        public const string Hospedaje = "Hospedaje";
        public const string Restaurante = "Restaurante";
        public const string Evento = "Evento";
        public const string Mixta = "Mixta";
        public static readonly string[] Todos = [Hospedaje, Restaurante, Evento, Mixta];
    }

    public static class Concepto
    {
        public const string Hospedaje = "Hospedaje";
        public const string Restaurante = "Restaurante";
        public const string Evento = "Evento";
        public const string Otro = "Otro";
    }

    public static class MetodoPago
    {
        public const string Efectivo = "Efectivo";
        public const string Tarjeta = "Tarjeta";
        public const string Transferencia = "Transferencia";
        public const string Sinpe = "SINPE";
        public static readonly string[] Todos = [Efectivo, Tarjeta, Transferencia, Sinpe];
    }

    public static readonly string[] TiposTarjeta = ["Visa", "MasterCard", "AmericanExpress", "Otra"];

    public static readonly string[] EstadosReservaFacturables = [Estados.Reserva.CheckIn, Estados.Reserva.CheckOut];

    // El servicio (10 %) se cobra solo sobre lo del restaurante; el IVA (13 %) sobre el subtotal, sin el servicio.
    public static TotalesFactura CalcularTotales(decimal subtotal, decimal subtotalRestaurante)
    {
        var subtotalRedondeado = Redondear(subtotal);
        var servicio = Redondear(subtotalRestaurante * TasaServicio);
        var iva = Redondear(subtotalRedondeado * TasaIva);
        return new TotalesFactura(subtotalRedondeado, servicio, iva, subtotalRedondeado + servicio + iva);
    }

    public static decimal Redondear(decimal monto) => Math.Round(monto, 2, MidpointRounding.AwayFromZero);

    // Mismo formato que la columna calculada FACTURA.NumeroFactura.
    public static string NumeroFactura(int idFactura) => $"FAC-{idFactura:D6}";
}

public record TotalesFactura(decimal Subtotal, decimal Servicio, decimal Iva, decimal Total);
