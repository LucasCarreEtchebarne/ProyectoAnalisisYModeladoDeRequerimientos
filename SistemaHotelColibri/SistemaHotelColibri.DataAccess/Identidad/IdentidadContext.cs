using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SistemaHotelColibri.DataAccess.Identidad;

public class IdentidadContext : IdentityDbContext<Usuario, Rol, int>
{
    public IdentidadContext(DbContextOptions<IdentidadContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Usuario>(entidad =>
        {
            entidad.Property(u => u.NombreCompleto).HasMaxLength(150);
            entidad.Property(u => u.Estado).HasMaxLength(20).HasDefaultValue("Activo");
            entidad.Property(u => u.FechaCreacion).HasDefaultValueSql("SYSDATETIME()");

            entidad.ToTable(t => t.HasCheckConstraint("CK_AspNetUsers_Estado", "[Estado] IN ('Activo','Inactivo')"));
        });

        builder.Entity<Rol>(entidad =>
        {
            entidad.Property(r => r.Descripcion).HasMaxLength(250);
            entidad.Property(r => r.EstadoRol).HasMaxLength(20).HasDefaultValue("Activo");

            entidad.ToTable(t => t.HasCheckConstraint("CK_AspNetRoles_EstadoRol", "[EstadoRol] IN ('Activo','Inactivo')"));
        });
    }
}
