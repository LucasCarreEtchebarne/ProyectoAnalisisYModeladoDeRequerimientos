using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaHotelColibri.DataAccess.Entidades;

namespace SistemaHotelColibri.DataAccess.Contexto;

public partial class HotelColibriContext : DbContext
{
    public HotelColibriContext(DbContextOptions<HotelColibriContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bitacora> Bitacora { get; set; }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<ConsultaIa> ConsultaIa { get; set; }

    public virtual DbSet<DetalleFactura> DetalleFactura { get; set; }

    public virtual DbSet<DetallePedido> DetallePedido { get; set; }

    public virtual DbSet<EspacioEvento> EspacioEvento { get; set; }

    public virtual DbSet<Evento> Evento { get; set; }

    public virtual DbSet<Factura> Factura { get; set; }

    public virtual DbSet<Habitacion> Habitacion { get; set; }

    public virtual DbSet<Housekeeping> Housekeeping { get; set; }

    public virtual DbSet<Inventario> Inventario { get; set; }

    public virtual DbSet<Menu> Menu { get; set; }

    public virtual DbSet<Mesa> Mesa { get; set; }

    public virtual DbSet<MovimientoInventario> MovimientoInventario { get; set; }

    public virtual DbSet<Pago> Pago { get; set; }

    public virtual DbSet<Pedido> Pedido { get; set; }

    public virtual DbSet<RecetaProducto> RecetaProducto { get; set; }

    public virtual DbSet<Reserva> Reserva { get; set; }

    public virtual DbSet<ReservaMesa> ReservaMesa { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AI");

        modelBuilder.Entity<Bitacora>(entity =>
        {
            entity.HasKey(e => e.IdBitacora).HasName("PK__BITACORA__ED3A1B13CBC22921");

            entity.ToTable("BITACORA");

            entity.HasIndex(e => new { e.Modulo, e.FechaHora }, "IX_Bitacora_Modulo_Fecha");

            entity.HasIndex(e => new { e.IdUsuario, e.FechaHora }, "IX_Bitacora_Usuario_Fecha");

            entity.Property(e => e.AccionRealizada).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.FechaHora).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Modulo).HasMaxLength(10);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__CLIENTE__D59466423E9DF878");

            entity.ToTable("CLIENTE");

            entity.HasIndex(e => e.Identificacion, "UQ__CLIENTE__D6F931E5B84A2F3E").IsUnique();

            entity.Property(e => e.CorreoElectronico).HasMaxLength(150);
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.EstadoCliente)
                .HasMaxLength(20)
                .HasDefaultValue("Activo");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Identificacion).HasMaxLength(30);
            entity.Property(e => e.NombreCompleto).HasMaxLength(100);
            entity.Property(e => e.PrimerApellido).HasMaxLength(50);
            entity.Property(e => e.SegundoApellido).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<ConsultaIa>(entity =>
        {
            entity.HasKey(e => e.IdConsultaIa).HasName("PK__CONSULTA__655A2C850B531FBB");

            entity.ToTable("CONSULTA_IA");

            entity.Property(e => e.IdConsultaIa).HasColumnName("IdConsultaIA");
            entity.Property(e => e.FechaRealizada).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<DetalleFactura>(entity =>
        {
            entity.HasKey(e => e.IdDetalleFactura).HasName("PK__DETALLE___DB5F4631E9E787C3");

            entity.ToTable("DETALLE_FACTURA");

            entity.HasIndex(e => e.IdFactura, "IX_DetalleFactura_Factura");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(12, 3)");
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Subtotal)
                .HasComputedColumnSql("([Cantidad]*[PrecioUnitario])", true)
                .HasColumnType("decimal(25, 5)");
            entity.Property(e => e.TipoConcepto).HasMaxLength(20);

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.IdEvento)
                .HasConstraintName("FK__DETALLE_F__IdEve__5CA1C101");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.IdFactura)
                .HasConstraintName("FK__DETALLE_F__IdFac__58D1301D");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__DETALLE_F__IdPed__5BAD9CC8");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__DETALLE_F__IdRes__5AB9788F");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__DETALLE___48AFFD9513EF1A4F");

            entity.ToTable("DETALLE_PEDIDO");

            entity.HasIndex(e => e.IdPedido, "IX_DetallePedido_Pedido");

            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Subtotal)
                .HasComputedColumnSql("([Cantidad]*[PrecioUnitario])", true)
                .HasColumnType("decimal(23, 2)");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedido)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__DETALLE_P__IdPed__40058253");

            entity.HasOne(d => d.IdProductoMenuNavigation).WithMany(p => p.DetallePedido)
                .HasForeignKey(d => d.IdProductoMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DETALLE_P__IdPro__40F9A68C");
        });

        modelBuilder.Entity<EspacioEvento>(entity =>
        {
            entity.HasKey(e => e.IdEspacio).HasName("PK__ESPACIO___CA4C0889D47C4BB1");

            entity.ToTable("ESPACIO_EVENTO");

            entity.HasIndex(e => e.NombreEspacio, "UQ__ESPACIO___585163C0B4981F3F").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoEspacio)
                .HasMaxLength(20)
                .HasDefaultValue("Activo");
            entity.Property(e => e.NombreEspacio).HasMaxLength(100);
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento).HasName("PK__EVENTO__034EFC041496E482");

            entity.ToTable("EVENTO");

            entity.HasIndex(e => new { e.IdEspacio, e.FechaEvento }, "IX_Evento_Espacio_Fecha");

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoEvento)
                .HasMaxLength(20)
                .HasDefaultValue("Programado");
            entity.Property(e => e.HoraFin).HasPrecision(0);
            entity.Property(e => e.HoraInicio).HasPrecision(0);
            entity.Property(e => e.MontoAcordado).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.NombreEvento).HasMaxLength(100);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Evento)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EVENTO__IdClient__25518C17");

            entity.HasOne(d => d.IdEspacioNavigation).WithMany(p => p.Evento)
                .HasForeignKey(d => d.IdEspacio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EVENTO__IdEspaci__2645B050");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__FACTURA__50E7BAF130A2149B");

            entity.ToTable("FACTURA");

            entity.HasIndex(e => e.IdCliente, "IX_Factura_Cliente");

            entity.Property(e => e.EstadoFactura)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaEmision).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MontoTotal).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.MotivoAnulacion).HasMaxLength(250);
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComputedColumnSql("('FAC-'+right('000000'+CONVERT([varchar](6),[IdFactura]),(6)))", true);
            entity.Property(e => e.TipoFactura).HasMaxLength(20);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Factura)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FACTURA__IdClien__4E53A1AA");
        });

        modelBuilder.Entity<Habitacion>(entity =>
        {
            entity.HasKey(e => e.IdHabitacion).HasName("PK__HABITACI__8BBBF9015132B8E4");

            entity.ToTable("HABITACION");

            entity.HasIndex(e => e.NumeroHabitacion, "UQ__HABITACI__08B8232F8ADA8DA0").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoHabitacion)
                .HasMaxLength(20)
                .HasDefaultValue("Disponible");
            entity.Property(e => e.NumeroHabitacion).HasMaxLength(10);
            entity.Property(e => e.Precio).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TipoHabitacion).HasMaxLength(30);
        });

        modelBuilder.Entity<Housekeeping>(entity =>
        {
            entity.HasKey(e => e.IdTarea).HasName("PK__HOUSEKEE__EADE9098F14E1CB9");

            entity.ToTable("HOUSEKEEPING");

            entity.HasIndex(e => new { e.EstadoTarea, e.FechaLimite }, "IX_Housekeeping_Estado");

            entity.Property(e => e.EstadoTarea)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaAsignacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(250);
            entity.Property(e => e.TipoTarea).HasMaxLength(50);

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Housekeeping)
                .HasForeignKey(d => d.IdHabitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HOUSEKEEP__IdHab__1DB06A4F");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__INVENTAR__09889210DF84FA77");

            entity.ToTable("INVENTARIO");

            entity.HasIndex(e => e.NombreProducto, "UQ__INVENTAR__74F263DED9BBF521").IsUnique();

            entity.Property(e => e.CategoriaProducto).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoProducto)
                .HasMaxLength(20)
                .HasDefaultValue("Activo");
            entity.Property(e => e.FechaIngreso).HasDefaultValueSql("(CONVERT([date],sysdatetime()))");
            entity.Property(e => e.NombreProducto).HasMaxLength(100);
            entity.Property(e => e.Stock).HasColumnType("decimal(12, 3)");
            entity.Property(e => e.StockMinimo).HasColumnType("decimal(12, 3)");
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(20)
                .HasDefaultValue("Unidad");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdProductoMenu).HasName("PK__MENU__1BE1B88897437D45");

            entity.ToTable("MENU");

            entity.HasIndex(e => e.NombreProducto, "UQ__MENU__74F263DE78BD0E0A").IsUnique();

            entity.Property(e => e.CategoriaMenu).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoProductoMenu)
                .HasMaxLength(20)
                .HasDefaultValue("Disponible");
            entity.Property(e => e.NombreProducto).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(12, 2)");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.IdMesa).HasName("PK__MESA__4D7E81B1958EC16C");

            entity.ToTable("MESA");

            entity.HasIndex(e => e.NumeroMesa, "UQ__MESA__A5588DD23B2419F4").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoMesa)
                .HasMaxLength(20)
                .HasDefaultValue("Disponible");
        });

        modelBuilder.Entity<MovimientoInventario>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("PK__MOVIMIEN__881A6AE0CEFA9279");

            entity.ToTable("MOVIMIENTO_INVENTARIO");

            entity.HasIndex(e => new { e.IdProducto, e.FechaHora }, "IX_Movimiento_Producto_Fecha");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(12, 3)");
            entity.Property(e => e.FechaHora).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Motivo).HasMaxLength(250);
            entity.Property(e => e.StockResultante).HasColumnType("decimal(12, 3)");
            entity.Property(e => e.TipoMovimiento).HasMaxLength(20);

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.MovimientoInventario)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__MOVIMIENT__IdPed__47A6A41B");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.MovimientoInventario)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MOVIMIENT__IdPro__45BE5BA9");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__PAGO__FC851A3A93B74217");

            entity.ToTable("PAGO");

            entity.HasIndex(e => e.IdFactura, "IX_Pago_Factura");

            entity.Property(e => e.CambioDevuelto).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(20)
                .HasDefaultValue("Aprobado");
            entity.Property(e => e.FechaHoraPago).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MetodoPago).HasMaxLength(20);
            entity.Property(e => e.MontoPagado).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.MontoRecibido).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.NumeroReferencia).HasMaxLength(50);
            entity.Property(e => e.TipoTarjeta).HasMaxLength(20);
            entity.Property(e => e.UltimosCuatroDigitos)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.Pago)
                .HasForeignKey(d => d.IdFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PAGO__IdFactura__634EBE90");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__PEDIDO__9D335DC3470C7932");

            entity.ToTable("PEDIDO");

            entity.HasIndex(e => e.EstadoPedido, "IX_Pedido_Estado");

            entity.HasIndex(e => e.FechaHoraPedido, "IX_Pedido_Fecha");

            entity.Property(e => e.EstadoPedido)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaHoraPedido).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(250);
            entity.Property(e => e.TipoPedido).HasMaxLength(20);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__PEDIDO__IdClient__3493CFA7");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdHabitacion)
                .HasConstraintName("FK__PEDIDO__IdHabita__37703C52");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdMesa)
                .HasConstraintName("FK__PEDIDO__IdMesa__367C1819");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__PEDIDO__IdReserv__3864608B");
        });

        modelBuilder.Entity<RecetaProducto>(entity =>
        {
            entity.HasKey(e => e.IdRecetaProducto).HasName("PK__RECETA_P__89735AADA54F4B7A");

            entity.ToTable("RECETA_PRODUCTO");

            entity.HasIndex(e => new { e.IdProductoMenu, e.IdProducto }, "UQ_Receta").IsUnique();

            entity.Property(e => e.CantidadUtilizada).HasColumnType("decimal(12, 3)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.RecetaProducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RECETA_PR__IdPro__30C33EC3");

            entity.HasOne(d => d.IdProductoMenuNavigation).WithMany(p => p.RecetaProducto)
                .HasForeignKey(d => d.IdProductoMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RECETA_PR__IdPro__2FCF1A8A");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__RESERVA__0E49C69D737CB4E1");

            entity.ToTable("RESERVA");

            entity.HasIndex(e => e.IdCliente, "IX_Reserva_Cliente");

            entity.HasIndex(e => new { e.IdHabitacion, e.FechaEntrada, e.FechaSalida }, "IX_Reserva_Habitacion_Fechas");

            entity.Property(e => e.CantidadNoches).HasComputedColumnSql("(datediff(day,[FechaEntrada],[FechaSalida]))", true);
            entity.Property(e => e.EstadoReserva)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MontoHospedaje)
                .HasComputedColumnSql("([PrecioNoche]*datediff(day,[FechaEntrada],[FechaSalida]))", true)
                .HasColumnType("decimal(23, 2)");
            entity.Property(e => e.Observaciones).HasMaxLength(250);
            entity.Property(e => e.PrecioNoche).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Reserva)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RESERVA__IdClien__0C85DE4D");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Reserva)
                .HasForeignKey(d => d.IdHabitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RESERVA__IdHabit__0D7A0286");
        });

        modelBuilder.Entity<ReservaMesa>(entity =>
        {
            entity.HasKey(e => e.IdReservaMesa).HasName("PK__RESERVA___9086E2E804CC1930");

            entity.ToTable("RESERVA_MESA");

            entity.Property(e => e.EstadoReservaMesa)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Observaciones).HasMaxLength(250);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.ReservaMesa)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__RESERVA_M__IdCli__17036CC0");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.ReservaMesa)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RESERVA_M__IdMes__160F4887");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
