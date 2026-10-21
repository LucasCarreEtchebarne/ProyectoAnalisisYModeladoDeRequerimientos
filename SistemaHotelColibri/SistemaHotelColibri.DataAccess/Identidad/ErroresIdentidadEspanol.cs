using Microsoft.AspNetCore.Identity;

namespace SistemaHotelColibri.DataAccess.Identidad;

public class ErroresIdentidadEspanol : IdentityErrorDescriber
{
    public override IdentityError DefaultError() =>
        Error(nameof(DefaultError), "Ocurrió un error inesperado.");

    public override IdentityError PasswordMismatch() =>
        Error(nameof(PasswordMismatch), "La contraseña actual no es correcta.");

    public override IdentityError InvalidToken() =>
        Error(nameof(InvalidToken), "El enlace o código ya no es válido.");

    public override IdentityError InvalidUserName(string? userName) =>
        Error(nameof(InvalidUserName), "El nombre de usuario solo admite letras, números, punto, guion y guion bajo.");

    public override IdentityError InvalidEmail(string? email) =>
        Error(nameof(InvalidEmail), "El correo electrónico no tiene un formato válido.");

    public override IdentityError DuplicateUserName(string userName) =>
        Error(nameof(DuplicateUserName), "Ya existe un usuario con ese nombre de usuario.");

    public override IdentityError DuplicateEmail(string email) =>
        Error(nameof(DuplicateEmail), "Ya existe un usuario con ese correo electrónico.");

    public override IdentityError UserAlreadyInRole(string role) =>
        Error(nameof(UserAlreadyInRole), $"El usuario ya tiene el rol {role}.");

    public override IdentityError PasswordTooShort(int length) =>
        Error(nameof(PasswordTooShort), $"La contraseña debe tener al menos {length} caracteres.");

    public override IdentityError PasswordRequiresDigit() =>
        Error(nameof(PasswordRequiresDigit), "La contraseña debe incluir al menos un número.");

    public override IdentityError PasswordRequiresLower() =>
        Error(nameof(PasswordRequiresLower), "La contraseña debe incluir al menos una letra minúscula.");

    public override IdentityError PasswordRequiresUpper() =>
        Error(nameof(PasswordRequiresUpper), "La contraseña debe incluir al menos una letra mayúscula.");

    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        Error(nameof(PasswordRequiresNonAlphanumeric), "La contraseña debe incluir al menos un símbolo.");

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) =>
        Error(nameof(PasswordRequiresUniqueChars), $"La contraseña debe tener al menos {uniqueChars} caracteres distintos.");

    private static IdentityError Error(string codigo, string descripcion) =>
        new() { Code = codigo, Description = descripcion };
}
