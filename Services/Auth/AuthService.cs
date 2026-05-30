using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs;
using ProyectoDonacion.DTOs.Auth;
using ProyectoDonacion.Models.Auth;
using ProyectoDonacion.Services.FireBase;

namespace ProyectoDonacion.Services.Auth;

public class AuthService
{
    private readonly FirebaseService _firebaseService;
    private readonly IMapper _mapper;

    public AuthService(FirebaseService firebaseService, IMapper mapper)
    {
        _firebaseService = firebaseService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<User>> Register(RegisterDto dto)
    {
        try
        {
            //  Primero verificar que no exista un usuario con ese correo
            var collection = _firebaseService.GetCollection("users");
            var existing = await collection
                .WhereEqualTo("Email", dto.Email)
                .GetSnapshotAsync();

            if (existing.Count > 0)
                return ApiResponse<User>.Warning("Ya existe un usuario con ese correo");

            // construir el objeto User a partir del DTO
            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Rol = dto.Rol ?? "user",
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            };

            // Guardamos el objeto en FS directamente sin diccionarios
            await collection.Document(user.Id).SetAsync(user);

            return ApiResponse<User>.Success(user, "Usuario registrado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<User>.Failure($"Error al registrar usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<UserDto>>> GetAllUsers(bool activos = true)
    {
        try
        {
            var collection = _firebaseService.GetCollection("users");
            var snapshot = await collection.GetSnapshotAsync();

            List<User> users = new List<User>();
            foreach (var document in snapshot.Documents)
            {
                // Convertimos directamente el documento a User usando FirestoreData
                User user = document.ConvertTo<User>();
                users.Add(user);
            }  

            if(activos)
                users = users.Where(u => u.Activo).ToList();

            return ApiResponse<List<UserDto>>.Success(_mapper.Map<List<UserDto>>(users), $"Se encontraron {users.Count} usuario(s)");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<UserDto>>.Failure($"Error al obtener usuarios: {ex.Message}");
        }
    }

    public async Task<ApiResponse<User>> GetUserById(string id)
    {
        try
        {
            var collection = _firebaseService.GetCollection("users");
            var snapshot = await collection
                .WhereEqualTo("Id", id)
                .GetSnapshotAsync();
            if (snapshot.Count == 0)
                return ApiResponse<User>.Warning("No se encontró un usuario con ese ID");

            User user = snapshot.Documents[0].ConvertTo<User>();
            return ApiResponse<User>.Success(user, "Usuario encontrado");
        }
        catch (Exception ex)
        {
            return ApiResponse<User>.Failure($"Error al obtener usuario: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DesactivarUsuario(string id)
    {
        try
        {
            var collection = _firebaseService.GetCollection("users");
            var snapshot = await collection
                .WhereEqualTo("Id", id)
                .GetSnapshotAsync();
            if (snapshot.Count == 0)
                return ApiResponse<bool>.Warning("No se encontró un usuario con ese ID");
            var docRef = snapshot.Documents[0].Reference;
            await docRef.UpdateAsync("Activo", false);

            return ApiResponse<bool>.Success(true, "Usuario desactivado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Failure($"Error al desactivar usuario: {ex.Message}");
        }
    }


    public async Task<ApiResponse<User>> Login(LoginDto dto)
    {
        try
        {
            var collection = _firebaseService.GetCollection("users");
            var snapshot = await collection
                .WhereEqualTo("Email", dto.Email)
                .GetSnapshotAsync();
            if (snapshot.Count == 0)
                return ApiResponse<User>.Warning("No se encontró un usuario con ese correo");

            User user = snapshot.Documents[0].ConvertTo<User>();
            if (user.PasswordHash != HashPassword(dto.Password))
                return ApiResponse<User>.Warning("Contraseña incorrecta");

            if (!user.Activo)
                return ApiResponse<User>.Warning("El usuario no está activo");

            return ApiResponse<User>.Success(user, "Login exitoso");
        }
        catch (Exception ex)
        {
            return ApiResponse<User>.Failure($"Error al iniciar sesión: {ex.Message}");
        }
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}