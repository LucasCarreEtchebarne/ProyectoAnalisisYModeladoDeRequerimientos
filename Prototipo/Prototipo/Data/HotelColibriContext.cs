using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Prototipo.Models;

namespace Prototipo.Data
{
    public class HotelColibriContext : DbContext
    {
        public HotelColibriContext() : base("name=HotelColibri")
        {
        }

        static HotelColibriContext()
        {
            Database.SetInitializer<HotelColibriContext>(null);
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<ProductoMenu> Menu { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<EspacioEvento> EspaciosEvento { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ReservaMesa> ReservasMesa { get; set; }
        public DbSet<TareaHousekeeping> Housekeeping { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<RecetaProducto> Recetas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetallesFactura { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Bitacora> Bitacoras { get; set; }
        public DbSet<ConsultaIA> ConsultasIA { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Inventario>().Property(p => p.Stock).HasPrecision(12, 3);
            modelBuilder.Entity<Inventario>().Property(p => p.StockMinimo).HasPrecision(12, 3);
            modelBuilder.Entity<RecetaProducto>().Property(p => p.CantidadUtilizada).HasPrecision(12, 3);
            modelBuilder.Entity<MovimientoInventario>().Property(p => p.Cantidad).HasPrecision(12, 3);
            modelBuilder.Entity<MovimientoInventario>().Property(p => p.StockResultante).HasPrecision(12, 3);
            modelBuilder.Entity<DetalleFactura>().Property(p => p.Cantidad).HasPrecision(12, 3);

            modelBuilder.Entity<Habitacion>().Property(p => p.Precio).HasPrecision(12, 2);
            modelBuilder.Entity<ProductoMenu>().Property(p => p.Precio).HasPrecision(12, 2);
            modelBuilder.Entity<Reserva>().Property(p => p.PrecioNoche).HasPrecision(12, 2);
            modelBuilder.Entity<Reserva>().Property(p => p.MontoHospedaje).HasPrecision(12, 2);
            modelBuilder.Entity<Evento>().Property(p => p.MontoAcordado).HasPrecision(12, 2);
            modelBuilder.Entity<DetallePedido>().Property(p => p.PrecioUnitario).HasPrecision(12, 2);
            modelBuilder.Entity<DetallePedido>().Property(p => p.Subtotal).HasPrecision(12, 2);
            modelBuilder.Entity<DetalleFactura>().Property(p => p.PrecioUnitario).HasPrecision(12, 2);
            modelBuilder.Entity<DetalleFactura>().Property(p => p.Subtotal).HasPrecision(12, 2);
            modelBuilder.Entity<Factura>().Property(p => p.MontoTotal).HasPrecision(12, 2);
            modelBuilder.Entity<Pago>().Property(p => p.MontoPagado).HasPrecision(12, 2);
            modelBuilder.Entity<Pago>().Property(p => p.MontoRecibido).HasPrecision(12, 2);
            modelBuilder.Entity<Pago>().Property(p => p.CambioDevuelto).HasPrecision(12, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
