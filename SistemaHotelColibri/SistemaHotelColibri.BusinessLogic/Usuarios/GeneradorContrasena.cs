using System.Security.Cryptography;

namespace SistemaHotelColibri.BusinessLogic.Usuarios;

internal static class GeneradorContrasena
{
    private const string Mayusculas = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const string Minusculas = "abcdefghijkmnpqrstuvwxyz";
    private const string Numeros = "23456789";
    private const int Longitud = 10;

    public static string Generar()
    {
        var todos = Mayusculas + Minusculas + Numeros;
        var caracteres = new List<char>
        {
            Mayusculas[RandomNumberGenerator.GetInt32(Mayusculas.Length)],
            Minusculas[RandomNumberGenerator.GetInt32(Minusculas.Length)],
            Numeros[RandomNumberGenerator.GetInt32(Numeros.Length)]
        };

        while (caracteres.Count < Longitud)
        {
            caracteres.Add(todos[RandomNumberGenerator.GetInt32(todos.Length)]);
        }

        return new string(caracteres.OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue)).ToArray());
    }
}
