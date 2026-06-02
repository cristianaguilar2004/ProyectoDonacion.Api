using AutoMapper;
using ProyectoDonacion.DTOs.Auth;
using ProyectoDonacion.DTOs.Categorias;
using ProyectoDonacion.Models.Auth;
using ProyectoDonacion.Models.Categorias;

namespace ProyectoDonacion.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeo bidireccional entre User y UserDto
        CreateMap<User, UserDto>().ReverseMap();

        // Mapeo para Categorías
        CreateMap<Categoria, CategoriaDto>().ReverseMap();
        CreateMap<CreateCategoriaDto, Categoria>();
        CreateMap<UpdateCategoriaDto, Categoria>();
    }


    public static IMapper CreateMapper()
    {
        var profile = new MappingProfile();
        return null!;
    }
}
