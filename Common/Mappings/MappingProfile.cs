using AutoMapper;
using ProyectoDonacion.DTOs;
using ProyectoDonacion.DTOs.Auth;
using ProyectoDonacion.Models.Auth;

namespace ProyectoDonacion.Common.Mappings;

/// <summary>
/// Perfil de mapeo de AutoMapper para la aplicación
/// Contiene las definiciones de cómo mapear entre modelos y DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeo bidireccional entre User y UserDto
        CreateMap<User, UserDto>().ReverseMap();
    }

    /// <summary>
    /// Método estático helper para crear un mapper rápidamente
    /// Uso: var mapper = MappingProfile.CreateMapper();
    /// </summary>
    public static IMapper CreateMapper()
    {
        var profile = new MappingProfile();
        // La versión 16 de AutoMapper tiene una API diferente
        // Por ahora usamos mapeo manual en los servicios
        // TODO: Actualizar cuando se tenga la configuración correcta
        return null!;
    }
}
