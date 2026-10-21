namespace SistemaHotelColibri.Abstract.Comun;

public static class Roles
{
    public const string Administrador = "Administrador";
    public const string Recepcionista = "Recepcionista";
    public const string Mesero = "Mesero";
    public const string Housekeeping = "Housekeeping";

    public static readonly string[] Todos = [Administrador, Recepcionista, Mesero, Housekeeping];

    public const string AccesoHospedaje = Administrador + "," + Recepcionista;
    public const string AccesoInventario = Administrador + "," + Mesero;
    public const string AccesoHousekeeping = Administrador + "," + Recepcionista + "," + Housekeeping;
}
