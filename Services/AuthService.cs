using System.Security.Cryptography;
using System.Text;
using Google.Cloud.Firestore;
using Microsoft.JSInterop;
using ProyectoDonacion.DTOs;
using ProyectoDonacion.Models;

namespace ProyectoDonacion.Services;

// En esta clase maneja lo relacionado al registro
// e inicio de sesion 

public class AuthService
{
    private readonly FirebaseService _firebaseService;
    private readonly IConfiguration _configuration;

    public AuthService(FirebaseService firebaseService, IConfiguration configuration)
    {
        _firebaseService = firebaseService;
        _configuration = configuration;
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
         FullName = dto.FullName,
         Email = dto.Email,
         PasswordHash = HashPassword(dto.Password),
         Role = "user",
         CreatedAt =  DateTime.UtcNow
     };
     
     // Guardamos el objeto en FS
     await collection.Document(user.Id).SetAsync(new Dictionary<string, object>
     {
         { "Id", user.Id },
         { "FullName", user.FullName },
         { "Email", user.Email },
         { "Password", user.PasswordHash },
         { "Role", user.Role },
         { "CreatedAt", user.CreatedAt },
     });
     return user;
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}