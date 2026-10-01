IF DB_ID('HotelColibri') IS NULL
    CREATE DATABASE HotelColibri COLLATE Modern_Spanish_CI_AI;
GO
USE HotelColibri;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'AspNetUsers') IS NOT NULL OR OBJECT_ID(N'CLIENTE') IS NOT NULL
BEGIN
    RAISERROR(N'La base HotelColibri ya tiene tablas. Ejecute primero HotelColibri_Reiniciar.sql.', 16, 1);
    SET NOEXEC ON;
END
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(250) NULL,
    [EstadoRol] nvarchar(20) NOT NULL DEFAULT N'Activo',
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_AspNetRoles_EstadoRol] CHECK ([EstadoRol] IN ('Activo','Inactivo'))
);

CREATE TABLE [AspNetUsers] (
    [Id] int NOT NULL IDENTITY,
    [NombreCompleto] nvarchar(150) NOT NULL,
    [Estado] nvarchar(20) NOT NULL DEFAULT N'Activo',
    [FechaCreacion] datetime2 NOT NULL DEFAULT (SYSDATETIME()),
    [FechaUltimoAcceso] datetime2 NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_AspNetUsers_Estado] CHECK ([Estado] IN ('Activo','Inactivo'))
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] int NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260926211623_CrearIdentidad', N'10.0.12');

COMMIT;
GO

CREATE TABLE CLIENTE (
    IdCliente           INT IDENTITY(1,1) PRIMARY KEY,
    Identificacion      NVARCHAR(30)  NOT NULL UNIQUE,
    Nombre              NVARCHAR(100) NOT NULL,
    PrimerApellido      NVARCHAR(50)  NOT NULL,
    SegundoApellido     NVARCHAR(50)  NULL,
    Telefono            NVARCHAR(20)  NULL,
    CorreoElectronico   NVARCHAR(150) NULL,
    Direccion           NVARCHAR(250) NULL,
    FechaRegistro       DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    EstadoCliente       NVARCHAR(20)  NOT NULL DEFAULT 'Activo'
        CHECK (EstadoCliente IN ('Activo','Inactivo'))
);

CREATE TABLE HABITACION (
    IdHabitacion        INT IDENTITY(1,1) PRIMARY KEY,
    NumeroHabitacion    NVARCHAR(10)  NOT NULL UNIQUE,
    TipoHabitacion      NVARCHAR(30)  NOT NULL,
    Capacidad           INT           NOT NULL CHECK (Capacidad > 0),
    Precio              DECIMAL(12,2) NOT NULL CHECK (Precio >= 0),  
    EstadoHabitacion    NVARCHAR(20)  NOT NULL DEFAULT 'Disponible'
        CHECK (EstadoHabitacion IN ('Disponible','Ocupada','Limpieza','Mantenimiento','Inactiva')),
    Piso                INT           NOT NULL,
    Descripcion         NVARCHAR(250) NULL
);

CREATE TABLE MESA (
    IdMesa              INT IDENTITY(1,1) PRIMARY KEY,
    NumeroMesa          INT           NOT NULL UNIQUE,
    Capacidad           INT           NOT NULL CHECK (Capacidad > 0),
    EstadoMesa          NVARCHAR(20)  NOT NULL DEFAULT 'Disponible'
        CHECK (EstadoMesa IN ('Disponible','Ocupada','Reservada','Inactiva')),
    Descripcion         NVARCHAR(250) NULL
);

CREATE TABLE MENU (
    IdProductoMenu      INT IDENTITY(1,1) PRIMARY KEY,
    NombreProducto      NVARCHAR(100) NOT NULL UNIQUE,
    CategoriaMenu       NVARCHAR(50)  NOT NULL,
    Precio              DECIMAL(12,2) NOT NULL CHECK (Precio >= 0),
    EstadoProductoMenu  NVARCHAR(20)  NOT NULL DEFAULT 'Disponible'
        CHECK (EstadoProductoMenu IN ('Disponible','Agotado','Inactivo')),
    Descripcion         NVARCHAR(250) NULL
);

CREATE TABLE INVENTARIO (
    IdProducto          INT IDENTITY(1,1) PRIMARY KEY,
    NombreProducto      NVARCHAR(100) NOT NULL UNIQUE,
    CategoriaProducto   NVARCHAR(50)  NOT NULL,
    UnidadMedida        NVARCHAR(20)  NOT NULL DEFAULT 'Unidad',
    Stock               DECIMAL(12,3) NOT NULL DEFAULT 0 CHECK (Stock >= 0),
    StockMinimo         DECIMAL(12,3) NOT NULL DEFAULT 0 CHECK (StockMinimo >= 0),
    FechaIngreso        DATE          NOT NULL DEFAULT CAST(SYSDATETIME() AS DATE),
    EstadoProducto      NVARCHAR(20)  NOT NULL DEFAULT 'Activo'
        CHECK (EstadoProducto IN ('Activo','Inactivo')),
    Descripcion         NVARCHAR(250) NULL
);

CREATE TABLE ESPACIO_EVENTO (
    IdEspacio           INT IDENTITY(1,1) PRIMARY KEY,
    NombreEspacio       NVARCHAR(100) NOT NULL UNIQUE,
    CapacidadMaxima     INT           NOT NULL CHECK (CapacidadMaxima > 0),
    EstadoEspacio       NVARCHAR(20)  NOT NULL DEFAULT 'Activo'
        CHECK (EstadoEspacio IN ('Activo','Inactivo')),
    Descripcion         NVARCHAR(250) NULL
);


CREATE TABLE RESERVA (
    IdReserva           INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente           INT NOT NULL REFERENCES CLIENTE(IdCliente),
    IdHabitacion        INT NOT NULL REFERENCES HABITACION(IdHabitacion),
    FechaEntrada        DATE NOT NULL,
    FechaSalida         DATE NOT NULL,
    CantidadHuespedes   INT  NOT NULL CHECK (CantidadHuespedes > 0),
    PrecioNoche         DECIMAL(12,2) NOT NULL CHECK (PrecioNoche >= 0),
    CantidadNoches      AS DATEDIFF(DAY, FechaEntrada, FechaSalida) PERSISTED,
    MontoHospedaje      AS (PrecioNoche * DATEDIFF(DAY, FechaEntrada, FechaSalida)) PERSISTED,
    EstadoReserva       NVARCHAR(20) NOT NULL DEFAULT 'Pendiente'
        CHECK (EstadoReserva IN ('Pendiente','Confirmada','CheckIn','CheckOut','Cancelada')),
    FechaRegistro       DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Observaciones       NVARCHAR(250) NULL,
    CONSTRAINT CK_Reserva_Fechas CHECK (FechaSalida > FechaEntrada)
);

CREATE TABLE RESERVA_MESA (
    IdReservaMesa       INT IDENTITY(1,1) PRIMARY KEY,
    IdMesa              INT NOT NULL REFERENCES MESA(IdMesa),
    IdCliente           INT NULL REFERENCES CLIENTE(IdCliente),
    IdUsuario           INT NOT NULL REFERENCES AspNetUsers(Id),
    FechaHoraReserva    DATETIME2 NOT NULL,
    CantidadPersonas    INT NOT NULL CHECK (CantidadPersonas > 0),
    EstadoReservaMesa   NVARCHAR(20) NOT NULL DEFAULT 'Pendiente'
        CHECK (EstadoReservaMesa IN ('Pendiente','Confirmada','Atendida','Cancelada')),
    Observaciones       NVARCHAR(250) NULL
);

CREATE TABLE HOUSEKEEPING (
    IdTarea             INT IDENTITY(1,1) PRIMARY KEY,
    IdHabitacion        INT NOT NULL REFERENCES HABITACION(IdHabitacion),
    IdUsuario           INT NOT NULL REFERENCES AspNetUsers(Id),
    TipoTarea           NVARCHAR(50) NOT NULL,
    FechaAsignacion     DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    FechaLimite         DATETIME2 NULL,
    FechaCompletada     DATETIME2 NULL,
    EstadoTarea         NVARCHAR(20) NOT NULL DEFAULT 'Pendiente'
        CHECK (EstadoTarea IN ('Pendiente','EnProceso','Completada')),
    Observaciones       NVARCHAR(250) NULL,
    CONSTRAINT CK_Housekeeping_Completada
        CHECK (EstadoTarea <> 'Completada' OR FechaCompletada IS NOT NULL)
);

CREATE TABLE EVENTO (
    IdEvento            INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente           INT NOT NULL REFERENCES CLIENTE(IdCliente),
    IdEspacio           INT NOT NULL REFERENCES ESPACIO_EVENTO(IdEspacio),
    NombreEvento        NVARCHAR(100) NOT NULL,
    FechaEvento         DATE    NOT NULL,
    HoraInicio          TIME(0) NOT NULL,
    HoraFin             TIME(0) NOT NULL,
    Participantes       INT     NOT NULL CHECK (Participantes > 0),
    MontoAcordado       DECIMAL(12,2) NOT NULL DEFAULT 0 CHECK (MontoAcordado >= 0),
    EstadoEvento        NVARCHAR(20)  NOT NULL DEFAULT 'Programado'
        CHECK (EstadoEvento IN ('Programado','Realizado','Cancelado')),
    Descripcion         NVARCHAR(250) NULL,
    CONSTRAINT CK_Evento_Horas CHECK (HoraFin > HoraInicio)
);


CREATE TABLE RECETA_PRODUCTO (
    IdRecetaProducto    INT IDENTITY(1,1) PRIMARY KEY,
    IdProductoMenu      INT NOT NULL REFERENCES MENU(IdProductoMenu),
    IdProducto          INT NOT NULL REFERENCES INVENTARIO(IdProducto),
    CantidadUtilizada   DECIMAL(12,3) NOT NULL CHECK (CantidadUtilizada > 0),
    CONSTRAINT UQ_Receta UNIQUE (IdProductoMenu, IdProducto)
);

CREATE TABLE PEDIDO (
    IdPedido            INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente           INT NULL REFERENCES CLIENTE(IdCliente),
    IdUsuario           INT NOT NULL REFERENCES AspNetUsers(Id),
    IdMesa              INT NULL REFERENCES MESA(IdMesa),            
    IdHabitacion        INT NULL REFERENCES HABITACION(IdHabitacion), 
    IdReserva           INT NULL REFERENCES RESERVA(IdReserva),        
    TipoPedido          NVARCHAR(20) NOT NULL
        CHECK (TipoPedido IN ('Mesa','Habitacion','Llevar')),
    EstadoPedido        NVARCHAR(20) NOT NULL DEFAULT 'Pendiente'
        CHECK (EstadoPedido IN ('Pendiente','Confirmado','Entregado','Cancelado')),
    FechaHoraPedido     DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Observaciones       NVARCHAR(250) NULL,
    CONSTRAINT CK_Pedido_Origen CHECK (
        (TipoPedido = 'Mesa'       AND IdMesa IS NOT NULL AND IdHabitacion IS NULL) OR
        (TipoPedido = 'Habitacion' AND IdHabitacion IS NOT NULL AND IdMesa IS NULL) OR
        (TipoPedido = 'Llevar'     AND IdMesa IS NULL AND IdHabitacion IS NULL)
    )
);

CREATE TABLE DETALLE_PEDIDO (
    IdDetallePedido     INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido            INT NOT NULL REFERENCES PEDIDO(IdPedido) ON DELETE CASCADE,
    IdProductoMenu      INT NOT NULL REFERENCES MENU(IdProductoMenu),
    Cantidad            INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario      DECIMAL(12,2) NOT NULL CHECK (PrecioUnitario >= 0),
    Subtotal            AS (Cantidad * PrecioUnitario) PERSISTED
);

CREATE TABLE MOVIMIENTO_INVENTARIO (
    IdMovimiento        BIGINT IDENTITY(1,1) PRIMARY KEY,
    IdProducto          INT NOT NULL REFERENCES INVENTARIO(IdProducto),
    IdUsuario           INT NOT NULL REFERENCES AspNetUsers(Id),
    IdPedido            INT NULL REFERENCES PEDIDO(IdPedido),  
    TipoMovimiento      NVARCHAR(20) NOT NULL
        CHECK (TipoMovimiento IN ('Entrada','Salida','Ajuste')),
    Cantidad            DECIMAL(12,3) NOT NULL CHECK (Cantidad > 0),
    StockResultante     DECIMAL(12,3) NOT NULL CHECK (StockResultante >= 0),
    FechaHora           DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Motivo              NVARCHAR(250) NULL
);


CREATE TABLE FACTURA (
    IdFactura           INT IDENTITY(1,1) PRIMARY KEY,
    NumeroFactura       AS ('FAC-' + RIGHT('000000' + CAST(IdFactura AS VARCHAR(6)), 6)) PERSISTED,
    IdCliente           INT NULL REFERENCES CLIENTE(IdCliente),
    IdUsuario           INT NOT NULL REFERENCES AspNetUsers(Id),
    FechaEmision        DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    TipoFactura         NVARCHAR(20) NOT NULL
        CHECK (TipoFactura IN ('Hospedaje','Restaurante','Evento','Mixta')),
    MontoTotal          DECIMAL(12,2) NOT NULL DEFAULT 0 CHECK (MontoTotal >= 0),
    EstadoFactura       NVARCHAR(20) NOT NULL DEFAULT 'Pendiente'
        CHECK (EstadoFactura IN ('Pendiente','Pagada','Anulada')),
    MotivoAnulacion     NVARCHAR(250) NULL,
    FechaAnulacion      DATETIME2 NULL,
    CONSTRAINT CK_Factura_Anulacion
        CHECK (EstadoFactura <> 'Anulada' OR MotivoAnulacion IS NOT NULL)
);

CREATE TABLE DETALLE_FACTURA (
    IdDetalleFactura    INT IDENTITY(1,1) PRIMARY KEY,
    IdFactura           INT NOT NULL REFERENCES FACTURA(IdFactura) ON DELETE CASCADE,
    TipoConcepto        NVARCHAR(20) NOT NULL
        CHECK (TipoConcepto IN ('Hospedaje','Restaurante','Evento','Otro')),
    IdReserva           INT NULL REFERENCES RESERVA(IdReserva),
    IdPedido            INT NULL REFERENCES PEDIDO(IdPedido),
    IdEvento            INT NULL REFERENCES EVENTO(IdEvento),
    Descripcion         NVARCHAR(250) NOT NULL,
    Cantidad            DECIMAL(12,3) NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario      DECIMAL(12,2) NOT NULL CHECK (PrecioUnitario >= 0),
    Subtotal            AS (Cantidad * PrecioUnitario) PERSISTED,
    CONSTRAINT CK_DetalleFactura_Origen CHECK (
        (CASE WHEN IdReserva IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN IdPedido  IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN IdEvento  IS NOT NULL THEN 1 ELSE 0 END) <= 1
    ),
    CONSTRAINT CK_DetalleFactura_Concepto CHECK (
        (TipoConcepto = 'Hospedaje'   AND IdPedido IS NULL AND IdEvento IS NULL) OR
        (TipoConcepto = 'Restaurante' AND IdReserva IS NULL AND IdEvento IS NULL) OR
        (TipoConcepto = 'Evento'      AND IdReserva IS NULL AND IdPedido IS NULL) OR
        (TipoConcepto = 'Otro'        AND IdReserva IS NULL AND IdPedido IS NULL AND IdEvento IS NULL)
    )
);

CREATE TABLE PAGO (
    IdPago              INT IDENTITY(1,1) PRIMARY KEY,
    IdFactura           INT NOT NULL REFERENCES FACTURA(IdFactura),
    IdUsuario           INT NOT NULL REFERENCES AspNetUsers(Id),
    MetodoPago          NVARCHAR(20) NOT NULL
        CHECK (MetodoPago IN ('Efectivo','Tarjeta','Transferencia','SINPE')),
    MontoPagado         DECIMAL(12,2) NOT NULL CHECK (MontoPagado > 0),
    MontoRecibido       DECIMAL(12,2) NULL,        
    CambioDevuelto      DECIMAL(12,2) NULL,        
    TipoTarjeta         NVARCHAR(20)  NULL         
        CHECK (TipoTarjeta IS NULL OR TipoTarjeta IN ('Visa','MasterCard','AmericanExpress','Otra')),
    UltimosCuatroDigitos CHAR(4)      NULL,       
    NumeroReferencia    NVARCHAR(50)  NULL,
    FechaHoraPago       DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    EstadoPago          NVARCHAR(20) NOT NULL DEFAULT 'Aprobado'
        CHECK (EstadoPago IN ('Aprobado','Rechazado','Reversado')),
    CONSTRAINT CK_Pago_Efectivo CHECK (
        MetodoPago <> 'Efectivo' OR (
            MontoRecibido  IS NOT NULL AND MontoRecibido >= MontoPagado AND
            CambioDevuelto IS NOT NULL AND CambioDevuelto = MontoRecibido - MontoPagado)
    ),
    CONSTRAINT CK_Pago_Tarjeta CHECK (
        MetodoPago <> 'Tarjeta' OR (TipoTarjeta IS NOT NULL AND UltimosCuatroDigitos IS NOT NULL)
    ),
    CONSTRAINT CK_Pago_Digitos CHECK (
        UltimosCuatroDigitos IS NULL OR UltimosCuatroDigitos LIKE '[0-9][0-9][0-9][0-9]'
    ),
    CONSTRAINT CK_Pago_Referencia CHECK (
        MetodoPago NOT IN ('Transferencia','SINPE') OR NumeroReferencia IS NOT NULL
    )
);


CREATE TABLE BITACORA (
    IdBitacora          BIGINT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario           INT NOT NULL REFERENCES AspNetUsers(Id),
    Modulo              NVARCHAR(10)  NOT NULL   
        CHECK (Modulo IN ('HRE','RPV','INV','CLI','HSK','FAC','USR','REP','IA')),
    IdRegistro          INT           NULL,     
    AccionRealizada     NVARCHAR(50)  NOT NULL,
    FechaHora           DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    Descripcion         NVARCHAR(500) NULL
);

CREATE TABLE CONSULTA_IA (
    IdConsultaIA        INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario           INT NOT NULL REFERENCES AspNetUsers(Id),
    ConsultaIngresada   NVARCHAR(MAX) NOT NULL,
    RespuestaGenerada   NVARCHAR(MAX) NULL,
    FechaRealizada      DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO

CREATE INDEX IX_Reserva_Habitacion_Fechas ON RESERVA(IdHabitacion, FechaEntrada, FechaSalida);
CREATE INDEX IX_Reserva_Cliente           ON RESERVA(IdCliente);
CREATE INDEX IX_Pedido_Fecha              ON PEDIDO(FechaHoraPedido);
CREATE INDEX IX_Pedido_Estado             ON PEDIDO(EstadoPedido);
CREATE INDEX IX_DetallePedido_Pedido      ON DETALLE_PEDIDO(IdPedido);
CREATE INDEX IX_Evento_Espacio_Fecha      ON EVENTO(IdEspacio, FechaEvento);
CREATE INDEX IX_Factura_Cliente           ON FACTURA(IdCliente);
CREATE INDEX IX_DetalleFactura_Factura    ON DETALLE_FACTURA(IdFactura);
CREATE INDEX IX_Pago_Factura              ON PAGO(IdFactura);
CREATE INDEX IX_Movimiento_Producto_Fecha ON MOVIMIENTO_INVENTARIO(IdProducto, FechaHora);
CREATE INDEX IX_Housekeeping_Estado       ON HOUSEKEEPING(EstadoTarea, FechaLimite);
CREATE INDEX IX_Bitacora_Usuario_Fecha    ON BITACORA(IdUsuario, FechaHora);
CREATE INDEX IX_Bitacora_Modulo_Fecha     ON BITACORA(Modulo, FechaHora);
CREATE INDEX IX_ReservaMesa_Mesa          ON RESERVA_MESA(IdMesa);
CREATE INDEX IX_ReservaMesa_Cliente       ON RESERVA_MESA(IdCliente);
CREATE INDEX IX_ReservaMesa_Usuario       ON RESERVA_MESA(IdUsuario);
CREATE INDEX IX_Housekeeping_Habitacion   ON HOUSEKEEPING(IdHabitacion);
CREATE INDEX IX_Housekeeping_Usuario      ON HOUSEKEEPING(IdUsuario);
CREATE INDEX IX_Evento_Cliente            ON EVENTO(IdCliente);
CREATE INDEX IX_Receta_Producto           ON RECETA_PRODUCTO(IdProducto);
CREATE INDEX IX_Pedido_Cliente            ON PEDIDO(IdCliente);
CREATE INDEX IX_Pedido_Usuario            ON PEDIDO(IdUsuario);
CREATE INDEX IX_Pedido_Mesa               ON PEDIDO(IdMesa);
CREATE INDEX IX_Pedido_Habitacion         ON PEDIDO(IdHabitacion);
CREATE INDEX IX_Pedido_Reserva            ON PEDIDO(IdReserva);
CREATE INDEX IX_DetallePedido_Menu        ON DETALLE_PEDIDO(IdProductoMenu);
CREATE INDEX IX_Movimiento_Usuario        ON MOVIMIENTO_INVENTARIO(IdUsuario);
CREATE INDEX IX_Movimiento_Pedido         ON MOVIMIENTO_INVENTARIO(IdPedido);
CREATE INDEX IX_Factura_Usuario           ON FACTURA(IdUsuario);
CREATE INDEX IX_DetalleFactura_Reserva    ON DETALLE_FACTURA(IdReserva);
CREATE INDEX IX_DetalleFactura_Pedido     ON DETALLE_FACTURA(IdPedido);
CREATE INDEX IX_DetalleFactura_Evento     ON DETALLE_FACTURA(IdEvento);
CREATE INDEX IX_Pago_Usuario              ON PAGO(IdUsuario);
CREATE INDEX IX_ConsultaIA_Usuario        ON CONSULTA_IA(IdUsuario);
GO


INSERT INTO AspNetRoles (Name, NormalizedName, ConcurrencyStamp, Descripcion) VALUES
(N'Administrador', N'ADMINISTRADOR', NEWID(), N'Acceso completo al sistema, usuarios y configuración'),
(N'Recepcionista', N'RECEPCIONISTA', NEWID(), N'Reservas, habitaciones, clientes, eventos y facturación'),
(N'Mesero',        N'MESERO',        NEWID(), N'Mesas, pedidos y punto de venta del restaurante'),
(N'Housekeeping',  N'HOUSEKEEPING',  NEWID(), N'Tareas de limpieza y mantenimiento de habitaciones');

INSERT INTO AspNetUsers (NombreCompleto, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
                         SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount) VALUES
(N'Administrador General', N'admin',      N'ADMIN',      N'admin@colibri.cr',     N'ADMIN@COLIBRI.CR',     1, NEWID(), NEWID(), 0, 0, 1, 0),
(N'María Rodríguez',       N'mrodriguez', N'MRODRIGUEZ', N'recepcion@colibri.cr', N'RECEPCION@COLIBRI.CR', 1, NEWID(), NEWID(), 0, 0, 1, 0),
(N'Carlos Mora',           N'cmora',      N'CMORA',      N'mesero@colibri.cr',    N'MESERO@COLIBRI.CR',    1, NEWID(), NEWID(), 0, 0, 1, 0),
(N'Ana Jiménez',           N'ajimenez',   N'AJIMENEZ',   N'hsk@colibri.cr',       N'HSK@COLIBRI.CR',       1, NEWID(), NEWID(), 0, 0, 1, 0);

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM (VALUES (N'admin', N'Administrador'), (N'mrodriguez', N'Recepcionista'), (N'cmora', N'Mesero'), (N'ajimenez', N'Housekeeping'))
     AS v (UserName, RoleName)
JOIN AspNetUsers u ON u.UserName = v.UserName
JOIN AspNetRoles r ON r.Name = v.RoleName;

INSERT INTO CLIENTE (Identificacion, Nombre, PrimerApellido, SegundoApellido, Telefono, CorreoElectronico, Direccion) VALUES
(N'1-1234-0567', N'Luis',  N'Vargas', N'Solís', N'8888-1111', N'luis.vargas@mail.com',  N'San José'),
(N'2-0456-0789', N'Sofía', N'Castro', N'Rojas', N'8777-2222', N'sofia.castro@mail.com', N'Alajuela');

INSERT INTO HABITACION (NumeroHabitacion, TipoHabitacion, Capacidad, Precio, Piso, Descripcion) VALUES
(N'101', N'Estándar', 2, 45000, 1, N'Cama matrimonial'),
(N'102', N'Doble',    4, 65000, 1, N'Dos camas queen'),
(N'201', N'Suite',    2, 95000, 2, N'Vista al jardín');

INSERT INTO MESA (NumeroMesa, Capacidad) VALUES (1, 2), (2, 4), (3, 6);

INSERT INTO ESPACIO_EVENTO (NombreEspacio, CapacidadMaxima, Descripcion) VALUES
(N'Salón Colibrí', 80, N'Salón principal con proyector'),
(N'Terraza Jardín', 40, N'Espacio al aire libre');

INSERT INTO INVENTARIO (NombreProducto, CategoriaProducto, UnidadMedida, Stock, StockMinimo) VALUES
(N'Arroz',        N'Granos',            N'Kilogramo', 50,  10),
(N'Frijoles',     N'Granos',            N'Kilogramo', 30,   8),
(N'Huevos',       N'Lácteos y huevos',  N'Unidad',   120,  30),
(N'Café molido',  N'Bebidas',           N'Kilogramo', 10,   2);

INSERT INTO MENU (NombreProducto, CategoriaMenu, Precio, Descripcion) VALUES
(N'Gallo pinto con huevo', N'Desayunos', 4500, N'Desayuno típico'),
(N'Café chorreado',        N'Bebidas',   1500, N'Taza de café');

INSERT INTO RECETA_PRODUCTO (IdProductoMenu, IdProducto, CantidadUtilizada) VALUES
(1, 1, 0.150), (1, 2, 0.100), (1, 3, 2), (2, 4, 0.020);

INSERT INTO RESERVA (IdCliente, IdHabitacion, FechaEntrada, FechaSalida, CantidadHuespedes, PrecioNoche, EstadoReserva) VALUES
(1, 1, '2026-10-01', '2026-10-03', 2, 45000, N'Confirmada');

INSERT INTO PEDIDO (IdCliente, IdUsuario, IdMesa, TipoPedido, EstadoPedido) VALUES
(2, 3, 2, N'Mesa', N'Entregado');
INSERT INTO DETALLE_PEDIDO (IdPedido, IdProductoMenu, Cantidad, PrecioUnitario) VALUES
(1, 1, 2, 4500), (1, 2, 2, 1500);

INSERT INTO MOVIMIENTO_INVENTARIO (IdProducto, IdUsuario, IdPedido, TipoMovimiento, Cantidad, StockResultante, Motivo) VALUES
(1, 3, 1, N'Salida', 0.300, 49.700, N'Consumo receta pedido FAC-000001'),
(2, 3, 1, N'Salida', 0.200, 29.800, N'Consumo receta pedido FAC-000001'),
(3, 3, 1, N'Salida', 4.000, 116.000, N'Consumo receta pedido FAC-000001'),
(4, 3, 1, N'Salida', 0.040, 9.960, N'Consumo receta pedido FAC-000001');
UPDATE INVENTARIO SET Stock = 49.700 WHERE IdProducto = 1;
UPDATE INVENTARIO SET Stock = 29.800 WHERE IdProducto = 2;
UPDATE INVENTARIO SET Stock = 116.000 WHERE IdProducto = 3;
UPDATE INVENTARIO SET Stock = 9.960  WHERE IdProducto = 4;

INSERT INTO FACTURA (IdCliente, IdUsuario, TipoFactura, MontoTotal, EstadoFactura) VALUES
(2, 3, N'Restaurante', 12000, N'Pagada');
INSERT INTO DETALLE_FACTURA (IdFactura, TipoConcepto, IdPedido, Descripcion, Cantidad, PrecioUnitario) VALUES
(1, N'Restaurante', 1, N'Gallo pinto con huevo', 2, 4500),
(1, N'Restaurante', 1, N'Café chorreado',        2, 1500);
INSERT INTO PAGO (IdFactura, IdUsuario, MetodoPago, MontoPagado, MontoRecibido, CambioDevuelto) VALUES
(1, 3, N'Efectivo', 12000, 15000, 3000);

INSERT INTO HOUSEKEEPING (IdHabitacion, IdUsuario, TipoTarea, FechaLimite) VALUES
(2, 4, N'Limpieza general', DATEADD(HOUR, 4, SYSDATETIME()));

INSERT INTO EVENTO (IdCliente, IdEspacio, NombreEvento, FechaEvento, HoraInicio, HoraFin, Participantes, MontoAcordado) VALUES
(1, 1, N'Reunión corporativa', '2026-11-15', '09:00', '13:00', 25, 350000);

INSERT INTO FACTURA (IdCliente, IdUsuario, TipoFactura, MontoTotal, EstadoFactura) VALUES
(1, 2, N'Mixta', 440000, N'Pendiente');
INSERT INTO DETALLE_FACTURA (IdFactura, TipoConcepto, IdReserva, IdEvento, Descripcion, Cantidad, PrecioUnitario) VALUES
(2, N'Hospedaje', 1, NULL, N'Habitación 101 — 2 noches', 2, 45000),
(2, N'Evento', NULL, 1, N'Reunión corporativa — Salón Colibrí', 1, 350000);

INSERT INTO BITACORA (IdUsuario, Modulo, IdRegistro, AccionRealizada, Descripcion) VALUES
(3, N'FAC', 1, N'CREAR', N'Factura de restaurante emitida'),
(2, N'FAC', 2, N'CREAR', N'Factura mixta (hospedaje + evento) emitida');
GO

SET NOEXEC OFF;
GO
