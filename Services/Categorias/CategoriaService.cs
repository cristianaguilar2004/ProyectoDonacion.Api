using AutoMapper;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.Categorias;
using ProyectoDonacion.Models.Categorias;
using ProyectoDonacion.Services.Auth;
using ProyectoDonacion.Services.FireBase;

namespace ProyectoDonacion.Services.Categorias;

public class CategoriaService
{
    private readonly FirebaseService _firebaseService;
    private readonly IMapper _mapper;
    private readonly UsuarioAutenticadoService _usuarioAutenticadoService;

    public CategoriaService(FirebaseService firebaseService, IMapper mapper, UsuarioAutenticadoService usuarioAutenticadoService)
    {
        _firebaseService = firebaseService;
        _mapper = mapper;
        _usuarioAutenticadoService = usuarioAutenticadoService;
    }

    public async Task<ApiResponse<List<CategoriaDto>>> GetAllCategorias(bool soloActivos = true)
    {
        try
        {
            var collection = _firebaseService.GetCollection("categorias");
            var snapshot = await collection.GetSnapshotAsync();

            List<Categoria> categorias = new List<Categoria>();
            foreach (var document in snapshot.Documents)
            {
                Categoria categoria = document.ConvertTo<Categoria>();
                categorias.Add(categoria);
            }

            if (soloActivos)
                categorias = categorias.Where(c => c.Activo).ToList();

            List<CategoriaDto> categoriasDto = _mapper.Map<List<CategoriaDto>>(categorias);
            return ApiResponse<List<CategoriaDto>>.Success(categoriasDto, $"Se encontraron {categoriasDto.Count} categoría(s)");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<CategoriaDto>>.Failure($"Error al obtener categorías: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CategoriaDto>> GetCategoriaById(string id)
    {
        try
        {
            var collection = _firebaseService.GetCollection("categorias");
            var docSnapshot = await collection.Document(id).GetSnapshotAsync();

            if (!docSnapshot.Exists)
                return ApiResponse<CategoriaDto>.Failure("No se encontró la categoría");

            Categoria categoria = docSnapshot.ConvertTo<Categoria>();
            CategoriaDto categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            return ApiResponse<CategoriaDto>.Success(categoriaDto, "Categoría encontrada");
        }
        catch (Exception ex)
        {
            return ApiResponse<CategoriaDto>.Failure($"Error al obtener categoría: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CategoriaDto>> CreateCategoria(CreateCategoriaDto dto)
    {
        try
        {
            var collection = _firebaseService.GetCollection("categorias");

            var existing = await collection
                .WhereEqualTo("Nombre", dto.Nombre)
                .GetSnapshotAsync();

            if (existing.Count > 0)
                return ApiResponse<CategoriaDto>.Failure($"Ya existe una categoría con el nombre '{dto.Nombre}'");

            Categoria categoria = new Categoria
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = dto.Nombre,
                UsuarioAgrega = _usuarioAutenticadoService.ObtenerUsuarioActual().NameIdentifier,
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            };

            if(!categoria.IsValid(out string validationMessage))
                return ApiResponse<CategoriaDto>.Failure($"Datos inválidos: {validationMessage}");

            await collection.Document(categoria.Id).SetAsync(categoria);

            CategoriaDto categoriaDto = _mapper.Map<CategoriaDto>(categoria);
            return ApiResponse<CategoriaDto>.Success(categoriaDto, "Categoría creada exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<CategoriaDto>.Failure($"Error al crear categoría: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CategoriaDto>> UpdateCategoria(UpdateCategoriaDto dto)
    {
        try
        {
            var collection = _firebaseService.GetCollection("categorias");
            var docSnapshot = await collection.Document(dto.Id).GetSnapshotAsync();

            if (!docSnapshot.Exists)
                return ApiResponse<CategoriaDto>.Failure("No se encontró la categoría");

            var existing = await collection
                .WhereEqualTo("Nombre", dto.Nombre)
                .GetSnapshotAsync();

            if (existing.Count > 0 && existing.Documents[0].Id != dto.Id)
                return ApiResponse<CategoriaDto>.Failure($"Ya existe otra categoría con el nombre '{dto.Nombre}'");

            Categoria categoria = docSnapshot.ConvertTo<Categoria>();
            categoria.Nombre = dto.Nombre;
            categoria.UsuarioModifica = _usuarioAutenticadoService.ObtenerUsuarioActual().NameIdentifier;
            categoria.FechaModifica = DateTime.UtcNow;

            await collection.Document(categoria.Id).SetAsync(categoria);

            CategoriaDto categoriaDto = _mapper.Map<CategoriaDto>(categoria);
            return ApiResponse<CategoriaDto>.Success(categoriaDto, "Categoría actualizada exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<CategoriaDto>.Failure($"Error al actualizar categoría: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CategoriaDto>> ChangeStateCategoria(string id)
    {
        try
        {
            var collection = _firebaseService.GetCollection("categorias");
            var docSnapshot = await collection.Document(id).GetSnapshotAsync();

            if (!docSnapshot.Exists)
                return ApiResponse<CategoriaDto>.Failure("No se encontró la categoría");

            Categoria categoria = docSnapshot.ConvertTo<Categoria>();
            categoria.Activo = !categoria.Activo;
            categoria.UsuarioModifica = _usuarioAutenticadoService.ObtenerUsuarioActual().NameIdentifier;
            categoria.FechaModifica = DateTime.UtcNow;

            await collection.Document(id).SetAsync(categoria);

            CategoriaDto categoriaDto = _mapper.Map<CategoriaDto>(categoria);

            string mensaje = categoria.Activo ? "Categoría activada exitosamente" : "Categoría desactivada exitosamente";
            return ApiResponse<CategoriaDto>.Success(categoriaDto, mensaje);
        }
        catch (Exception ex)
        {
            return ApiResponse<CategoriaDto>.Failure($"Error al actualizar estado de categoría: {ex.Message}");
        }
    }
}
