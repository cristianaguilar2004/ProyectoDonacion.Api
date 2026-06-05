using AutoMapper;
using ProyectoDonacion.DTOs.Auth;
using ProyectoDonacion.DTOs.Categorias;
using ProyectoDonacion.DTOs.EstadoArticulos;
using ProyectoDonacion.Models.Auth;
using ProyectoDonacion.Models.Categorias;
using ProyectoDonacion.Models.EstadoArticulos;

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

        // Mapeo para Estados de Artículo
        CreateMap<EstadoArticulo, EstadoArticuloDto>().ReverseMap();
        CreateMap<CreateEstadoArticuloDto, EstadoArticulo>();
        CreateMap<UpdateEstadoArticuloDto, EstadoArticulo>();
    }


    public static IMapper CreateMapper()
    {
        var profile = new MappingProfile();
        return null!;
    }
}
