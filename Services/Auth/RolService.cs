using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.Auth;
using ProyectoDonacion.Models.Auth;
using ProyectoDonacion.Services.FireBase;

namespace ProyectoDonacion.Services.Auth;

public class RolService
{
    private readonly FirebaseService _firebaseService;

    public RolService(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async Task<ApiResponse<List<Rol>>> GetAllRoles()
    {
        try
        {
            var collection = _firebaseService.GetCollection("roles");
            var snapshot = await collection.GetSnapshotAsync();

            var roles = new List<Rol>();
            foreach (var document in snapshot.Documents)
            {
                var rol = document.ConvertTo<Rol>();
                roles.Add(rol);
            }

            return ApiResponse<List<Rol>>.Success(roles, $"Se encontraron {roles.Count} rol(es)");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<Rol>>.Failure($"Error al obtener roles: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Rol>> CreateRol(CreateRolDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Id))
                return ApiResponse<Rol>.Warning("El ID del rol es requerido");

            var collection = _firebaseService.GetCollection("roles");

            var existingById = await collection.Document(dto.Id).GetSnapshotAsync();
            if (existingById.Exists)
                return ApiResponse<Rol>.Warning($"Ya existe un rol con el ID '{dto.Id}'");

            var existingByName = await collection
                .WhereEqualTo("Nombre", dto.Nombre)
                .GetSnapshotAsync();

            if (existingByName.Count > 0)
                return ApiResponse<Rol>.Warning($"Ya existe un rol con el nombre '{dto.Nombre}'");

            var rol = new Rol
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Menu = dto.Menu
            };

            if (!rol.IsValid(out string mensaje))
                return ApiResponse<Rol>.Warning(mensaje);

            await collection.Document(rol.Id).SetAsync(rol);

            return ApiResponse<Rol>.Success(rol, "Rol creado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<Rol>.Failure($"Error al crear rol: {ex.Message}");
        }
    }
}
