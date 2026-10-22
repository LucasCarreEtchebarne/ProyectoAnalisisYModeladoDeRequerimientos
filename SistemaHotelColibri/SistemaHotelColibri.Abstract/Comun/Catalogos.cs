namespace SistemaHotelColibri.Abstract.Comun;

public static class Catalogos
{
    public static readonly string[] UnidadesMedida =
        ["Unidad", "Kilogramo", "Gramo", "Litro", "Mililitro", "Paquete", "Caja", "Botella"];

    public static readonly string[] TiposHabitacion =
        ["Estándar", "Doble", "Suite", "Familiar"];

        
    public static readonly string[] TiposTareaHousekeeping =
        ["Limpieza general", "Limpieza profunda", "Cambio de ropa de cama", "Reposición de amenidades", "Desinfección", "Mantenimiento", "Revisión de habitación"];

    public static readonly string[] TiposCama = ["Individual", "Matrimonial", "Queen", "King"];

    public static readonly string[] VistasHabitacion = ["Jardín", "Piscina", "Montaña", "Ciudad", "Interior"];

    public static readonly string[] AreasInventario = ["Restaurante", "Habitaciones"];
}
