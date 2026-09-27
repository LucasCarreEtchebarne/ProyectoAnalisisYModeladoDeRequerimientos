using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaHotelColibri.DataAccess.Identidad;

public class IdentidadContextFactory : IDesignTimeDbContextFactory<IdentidadContext>
{
    public IdentidadContext CreateDbContext(string[] args)
    {
        var opciones = new DbContextOptionsBuilder<IdentidadContext>()
            .UseSqlServer("Server=localhost\\sqlexpress;Database=HotelColibri;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        return new IdentidadContext(opciones);
    }
}
