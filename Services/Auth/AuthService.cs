using System.Security.Cryptography;
using System.Text;
using ProyectoDonacion.DTOs;
using ProyectoDonacion.Models.Auth;
using ProyectoDonacion.Services.FireBase;

namespace ProyectoDonacion.Services.Auth;

public class AuthService
{
    private readonly FirebaseService _firebaseService;

    public AuthService(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async Task<User> Register(RegisterDto dto)
    {
        //  Primero verificar que no exista un usuario con ese correo
        var collection = _firebaseService.GetCollection("users");
        var existing = await collection
            .WhereEqualTo("Email", dto.Email)
            .GetSnapshotAsync();

        if (existing.Count > 0)
            throw new Exception("Ya existe un usuario con ese correo");

        // Darle permiso de que se pueda crear el nuevo usuario
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password),
            Rol = "user",
            FechaCreacion =  DateTime.UtcNow
        };

        // Guardamos el objeto en FS directamente sin diccionarios
        await collection.Document(user.Id).SetAsync(user);
        return user;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var collection = _firebaseService.GetCollection("users");
        var snapshot = await collection.GetSnapshotAsync();

        var users = new List<User>();
        foreach (var document in snapshot.Documents)
        {
            // Convertimos directamente el documento a User usando FirestoreData
            var user = document.ConvertTo<User>();
            users.Add(user);
        }

        return users;
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}