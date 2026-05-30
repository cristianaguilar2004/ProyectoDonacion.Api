using Google.Cloud.Firestore;

namespace ProyectoDonacion.Models.Auth;

[FirestoreData]
public class User
{
    // Representa un usuario dentro del sistema
    // Esta clase es lo que vamos a guarda en FS

    [FirestoreProperty("Id")]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty("Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [FirestoreProperty("Email")]
    public string Email { get; set; } = string.Empty;

    // La contraseña siempre va "encriptada" hasheada 
    [FirestoreProperty("Password")]
    public string PasswordHash { get; set; } = string.Empty;

    // Por defecto un usuario nuevo sea solo user
    [FirestoreProperty("Rol")]
    public string Rol { get; set; } = "user";

    [FirestoreProperty("FechaCreacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    [FirestoreProperty("Activo")]
    public bool Activo { get; set; } = true;
}