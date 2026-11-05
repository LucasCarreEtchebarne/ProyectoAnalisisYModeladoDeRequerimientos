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

    public virtual DbSet<CategoriaInventario> CategoriaInventario { get; set; }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<ConsultaIa> ConsultaIa { get; set; }

    public virtual DbSet<DetalleFactura> DetalleFactura { get; set; }

    public virtual DbSet<DetallePedido> DetallePedido { get; set; }

    public virtual DbSet<EspacioEvento> EspacioEvento { get; set; }

    public virtual DbSet<Evento> Evento { get; set; }

    public virtual DbSet<Factura> Factura { get; set; }

    public virtual DbSet<Habitacion> Habitacion { get; set; }

    public virtual DbSet<HabitacionImagen> HabitacionImagen { get; set; }

    public virtual DbSet<Housekeeping> Housekeeping { get; set; }

    public virtual DbSet<Inventario> Inventario { get; set; }

    public virtual DbSet<Menu> Menu { get; set; }

    public virtual DbSet<Mesa> Mesa { get; set; }

    public virtual DbSet<MovimientoInventario> MovimientoInventario { get; set; }

    public virtual DbSet<Pago> Pago { get; set; }

    public virtual DbSet<Pedido> Pedido { get; set; }

    public virtual DbSet<Proveedor> Proveedor { get; set; }

    public virtual DbSet<RecetaProducto> RecetaProducto { get; set; }

    public virtual DbSet<Reserva> Reserva { get; set; }

    public virtual DbSet<ReservaMesa> ReservaMesa { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AI");

        modelBuilder.Entity<Bitacora>(entity =>
        {
            entity.HasKey(e => e.IdBitacora).HasName("PK__BITACORA__ED3A1B1326CCBB8A");

            entity.ToTable("BITACORA");

            entity.HasIndex(e => new { e.Modulo, e.FechaHora }, "IX_Bitacora_Modulo_Fecha");

            entity.HasIndex(e => new { e.IdUsuario, e.FechaHora }, "IX_Bitacora_Usuario_Fecha");

            entity.Property(e => e.AccionRealizada).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.FechaHora).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Modulo).HasMaxLength(10);
        });

        modelBuilder.Entity<CategoriaInventario>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__CATEGORI__A3C02A104125019E");

            entity.ToTable("CATEGORIA_INVENTARIO");

            entity.HasIndex(e => e.NombreCategoria, "UQ__CATEGORI__A21FBE9F2078476E").IsUnique();

            entity.Property(e => e.Area).HasMaxLength(20);
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoCategoria)
                .HasMaxLength(20)
                .HasDefaultValue("Activo");
            entity.Property(e => e.NombreCategoria).HasMaxLength(50);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__CLIENTE__D594664251346643");

            entity.ToTable("CLIENTE");

            entity.HasIndex(e => e.Identificacion, "UQ__CLIENTE__D6F931E560170258").IsUnique();

            entity.HasIndex(e => e.CorreoElectronico, "UX_Cliente_Correo")
                .IsUnique()
                .HasFilter("([CorreoElectronico] IS NOT NULL)");

            entity.Property(e => e.CorreoElectronico).HasMaxLength(150);
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.EstadoCliente)
                .HasMaxLength(20)
                .HasDefaultValue("Activo");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Identificacion).HasMaxLength(30);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PrimerApellido).HasMaxLength(50);
            entity.Property(e => e.SegundoApellido).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<ConsultaIa>(entity =>
        {
            entity.HasKey(e => e.IdConsultaIa).HasName("PK__CONSULTA__655A2C85C2FA3020");

            entity.ToTable("CONSULTA_IA");

            entity.HasIndex(e => e.IdUsuario, "IX_ConsultaIA_Usuario");

            entity.Property(e => e.IdConsultaIa).HasColumnName("IdConsultaIA");
            entity.Property(e => e.FechaRealizada).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<DetalleFactura>(entity =>
        {
            entity.HasKey(e => e.IdDetalleFactura).HasName("PK__DETALLE___DB5F46313E175954");

            entity.ToTable("DETALLE_FACTURA");

            entity.HasIndex(e => e.IdEvento, "IX_DetalleFactura_Evento");

            entity.HasIndex(e => e.IdFactura, "IX_DetalleFactura_Factura");

            entity.HasIndex(e => e.IdPedido, "IX_DetalleFactura_Pedido");

            entity.HasIndex(e => e.IdReserva, "IX_DetalleFactura_Reserva");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(12, 3)");
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Subtotal)
                .HasComputedColumnSql("([Cantidad]*[PrecioUnitario])", true)
                .HasColumnType("decimal(25, 5)");
            entity.Property(e => e.TipoConcepto).HasMaxLength(20);

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.IdEvento)
                .HasConstraintName("FK__DETALLE_F__IdEve__08D548FA");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.IdFactura)
                .HasConstraintName("FK__DETALLE_F__IdFac__0504B816");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__DETALLE_F__IdPed__07E124C1");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__DETALLE_F__IdRes__06ED0088");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__DETALLE___48AFFD95C186DFD7");

            entity.ToTable("DETALLE_PEDIDO");

            entity.HasIndex(e => e.IdProductoMenu, "IX_DetallePedido_Menu");

            entity.HasIndex(e => e.IdPedido, "IX_DetallePedido_Pedido");

            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Subtotal)
                .HasComputedColumnSql("([Cantidad]*[PrecioUnitario])", true)
                .HasColumnType("decimal(23, 2)");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedido)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__DETALLE_P__IdPed__6C390A4C");

            entity.HasOne(d => d.IdProductoMenuNavigation).WithMany(p => p.DetallePedido)
                .HasForeignKey(d => d.IdProductoMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DETALLE_P__IdPro__6D2D2E85");
        });

        modelBuilder.Entity<EspacioEvento>(entity =>
        {
            entity.HasKey(e => e.IdEspacio).HasName("PK__ESPACIO___CA4C08890376F286");

            entity.ToTable("ESPACIO_EVENTO");

            entity.HasIndex(e => e.NombreEspacio, "UQ__ESPACIO___585163C02F85A6D2").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoEspacio)
                .HasMaxLength(20)
                .HasDefaultValue("Activo");
            entity.Property(e => e.NombreEspacio).HasMaxLength(100);
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento).HasName("PK__EVENTO__034EFC0440C8DB53");

            entity.ToTable("EVENTO");

            entity.HasIndex(e => e.IdCliente, "IX_Evento_Cliente");

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
                .HasConstraintName("FK__EVENTO__IdClient__51851410");

            entity.HasOne(d => d.IdEspacioNavigation).WithMany(p => p.Evento)
                .HasForeignKey(d => d.IdEspacio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EVENTO__IdEspaci__52793849");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__FACTURA__50E7BAF1AFFA23BD");

            entity.ToTable("FACTURA");

            entity.HasIndex(e => e.IdCliente, "IX_Factura_Cliente");

            entity.HasIndex(e => e.IdUsuario, "IX_Factura_Usuario");

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
                .HasConstraintName("FK__FACTURA__IdClien__7A8729A3");
        });

        modelBuilder.Entity<Habitacion>(entity =>
        {
            entity.HasKey(e => e.IdHabitacion).HasName("PK__HABITACI__8BBBF901979A897D");

            entity.ToTable("HABITACION");

            entity.HasIndex(e => e.NumeroHabitacion, "UQ__HABITACI__08B8232F73425556").IsUnique();

            entity.Property(e => e.CantidadCamas).HasDefaultValue(1);
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoHabitacion)
                .HasMaxLength(20)
                .HasDefaultValue("Disponible");
            entity.Property(e => e.NumeroHabitacion).HasMaxLength(10);
            entity.Property(e => e.Precio).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TipoCama)
                .HasMaxLength(20)
                .HasDefaultValue("Matrimonial");
            entity.Property(e => e.TipoHabitacion).HasMaxLength(30);
            entity.Property(e => e.Vista).HasMaxLength(20);
        });

        modelBuilder.Entity<HabitacionImagen>(entity =>
        {
            entity.HasKey(e => e.IdImagen).HasName("PK__HABITACI__B42D8F2ACDFE835D");

            entity.ToTable("HABITACION_IMAGEN");

            entity.HasIndex(e => e.IdHabitacion, "IX_HabitacionImagen_Habitacion");

            entity.HasIndex(e => e.RutaArchivo, "UQ__HABITACI__8B47300C4090A880").IsUnique();

            entity.Property(e => e.FechaSubida).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.NombreOriginal).HasMaxLength(150);
            entity.Property(e => e.RutaArchivo).HasMaxLength(260);

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.HabitacionImagen)
                .HasForeignKey(d => d.IdHabitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HABITACIO__IdHab__09FE775D");
        });

        modelBuilder.Entity<Housekeeping>(entity =>
        {
            entity.HasKey(e => e.IdTarea).HasName("PK__HOUSEKEE__EADE9098B9F0EB9B");

            entity.ToTable("HOUSEKEEPING");

            entity.HasIndex(e => new { e.EstadoTarea, e.FechaLimite }, "IX_Housekeeping_Estado");

            entity.HasIndex(e => e.IdHabitacion, "IX_Housekeeping_Habitacion");

            entity.HasIndex(e => e.IdUsuario, "IX_Housekeeping_Usuario");

            entity.Property(e => e.EstadoTarea)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaAsignacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(250);
            entity.Property(e => e.MotivoCancelacion).HasMaxLength(250);
            entity.Property(e => e.TipoTarea).HasMaxLength(50);

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Housekeeping)
                .HasForeignKey(d => d.IdHabitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HOUSEKEEP__IdHab__49E3F248");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__INVENTAR__09889210716F58DD");

            entity.ToTable("INVENTARIO");

            entity.HasIndex(e => e.IdCategoria, "IX_Inventario_Categoria");

            entity.HasIndex(e => e.IdProveedor, "IX_Inventario_Proveedor");

            entity.HasIndex(e => e.NombreProducto, "UQ__INVENTAR__74F263DE820CEFD4").IsUnique();

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

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Inventario)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__INVENTARI__IdCat__278EDA44");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Inventario)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__INVENTARI__IdPro__2882FE7D");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdProductoMenu).HasName("PK__MENU__1BE1B888C4985D83");

            entity.ToTable("MENU");

            entity.HasIndex(e => e.NombreProducto, "UQ__MENU__74F263DE55A0CC8E").IsUnique();

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
            entity.HasKey(e => e.IdMesa).HasName("PK__MESA__4D7E81B1ED34DA78");

            entity.ToTable("MESA");

            entity.HasIndex(e => e.NumeroMesa, "UQ__MESA__A5588DD2E32FFCA4").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoMesa)
                .HasMaxLength(20)
                .HasDefaultValue("Disponible");
        });

        modelBuilder.Entity<MovimientoInventario>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("PK__MOVIMIEN__881A6AE07F44189E");

            entity.ToTable("MOVIMIENTO_INVENTARIO");

            entity.HasIndex(e => e.IdPedido, "IX_Movimiento_Pedido");

            entity.HasIndex(e => new { e.IdProducto, e.FechaHora }, "IX_Movimiento_Producto_Fecha");

            entity.HasIndex(e => e.IdUsuario, "IX_Movimiento_Usuario");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(12, 3)");
            entity.Property(e => e.FechaHora).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Motivo).HasMaxLength(250);
            entity.Property(e => e.StockResultante).HasColumnType("decimal(12, 3)");
            entity.Property(e => e.TipoMovimiento).HasMaxLength(20);

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.MovimientoInventario)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__MOVIMIENT__IdPed__73DA2C14");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.MovimientoInventario)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MOVIMIENT__IdPro__71F1E3A2");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__PAGO__FC851A3A33C293D2");

            entity.ToTable("PAGO");

            entity.HasIndex(e => e.IdFactura, "IX_Pago_Factura");

            entity.HasIndex(e => e.IdUsuario, "IX_Pago_Usuario");

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
                .HasConstraintName("FK__PAGO__IdFactura__0F824689");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__PEDIDO__9D335DC3FADBB3D5");

            entity.ToTable("PEDIDO");

            entity.HasIndex(e => e.IdCliente, "IX_Pedido_Cliente");

            entity.HasIndex(e => e.EstadoPedido, "IX_Pedido_Estado");

            entity.HasIndex(e => e.FechaHoraPedido, "IX_Pedido_Fecha");

            entity.HasIndex(e => e.IdHabitacion, "IX_Pedido_Habitacion");

            entity.HasIndex(e => e.IdMesa, "IX_Pedido_Mesa");

            entity.HasIndex(e => e.IdReserva, "IX_Pedido_Reserva");

            entity.HasIndex(e => e.IdUsuario, "IX_Pedido_Usuario");

            entity.Property(e => e.EstadoPedido)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaHoraPedido).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(250);
            entity.Property(e => e.TipoPedido).HasMaxLength(20);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__PEDIDO__IdClient__60C757A0");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdHabitacion)
                .HasConstraintName("FK__PEDIDO__IdHabita__63A3C44B");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdMesa)
                .HasConstraintName("FK__PEDIDO__IdMesa__62AFA012");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__PEDIDO__IdReserv__6497E884");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__PROVEEDO__E8B631AF3E4A58DA");

            entity.ToTable("PROVEEDOR");

            entity.HasIndex(e => e.NombreProveedor, "UQ__PROVEEDO__C20DF5534479FA5B").IsUnique();

            entity.Property(e => e.CorreoElectronico).HasMaxLength(150);
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.EstadoProveedor)
                .HasMaxLength(20)
                .HasDefaultValue("Activo");
            entity.Property(e => e.NombreContacto).HasMaxLength(100);
            entity.Property(e => e.NombreProveedor).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<RecetaProducto>(entity =>
        {
            entity.HasKey(e => e.IdRecetaProducto).HasName("PK__RECETA_P__89735AAD18B7B8AB");

            entity.ToTable("RECETA_PRODUCTO");

            entity.HasIndex(e => e.IdProducto, "IX_Receta_Producto");

            entity.HasIndex(e => new { e.IdProductoMenu, e.IdProducto }, "UQ_Receta").IsUnique();

            entity.Property(e => e.CantidadUtilizada).HasColumnType("decimal(12, 3)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.RecetaProducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RECETA_PR__IdPro__5CF6C6BC");

            entity.HasOne(d => d.IdProductoMenuNavigation).WithMany(p => p.RecetaProducto)
                .HasForeignKey(d => d.IdProductoMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RECETA_PR__IdPro__5C02A283");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__RESERVA__0E49C69D2CB06972");

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
            entity.Property(e => e.PlacaVehiculo).HasMaxLength(10);
            entity.Property(e => e.PrecioNoche).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Reserva)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RESERVA__IdClien__38B96646");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Reserva)
                .HasForeignKey(d => d.IdHabitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RESERVA__IdHabit__39AD8A7F");
        });

        modelBuilder.Entity<ReservaMesa>(entity =>
        {
            entity.HasKey(e => e.IdReservaMesa).HasName("PK__RESERVA___9086E2E8F61940F4");

            entity.ToTable("RESERVA_MESA");

            entity.HasIndex(e => e.IdCliente, "IX_ReservaMesa_Cliente");

            entity.HasIndex(e => e.IdMesa, "IX_ReservaMesa_Mesa");

            entity.HasIndex(e => e.IdUsuario, "IX_ReservaMesa_Usuario");

            entity.Property(e => e.EstadoReservaMesa)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Observaciones).HasMaxLength(250);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.ReservaMesa)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__RESERVA_M__IdCli__4336F4B9");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.ReservaMesa)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RESERVA_M__IdMes__4242D080");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
