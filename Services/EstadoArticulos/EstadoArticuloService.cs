using AutoMapper;
using Google.Cloud.Firestore;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.EstadoArticulos;
using ProyectoDonacion.Models.Auth;
using ProyectoDonacion.Models.EstadoArticulos;
using ProyectoDonacion.Services.Auth;
using ProyectoDonacion.Services.FireBase;

namespace ProyectoDonacion.Services.EstadoArticulos;

public class EstadoArticuloService
{
    private readonly FirebaseService _firebaseService;
    private readonly IMapper _mapper;
    private readonly UsuarioAutenticadoService _usuarioAutenticadoService;

    public EstadoArticuloService(FirebaseService firebaseService, IMapper mapper, UsuarioAutenticadoService usuarioAutenticadoService)
    {
        _firebaseService = firebaseService;
        _mapper = mapper;
        _usuarioAutenticadoService = usuarioAutenticadoService;
    }

    public async Task<ApiResponse<List<EstadoArticuloDto>>> GetAllEstadosArticulo(bool soloActivos = true)
    {
        try
        {
            CollectionReference collection = _firebaseService.GetCollection("estados_articulo");
            QuerySnapshot snapshot = await collection.GetSnapshotAsync();

            List<EstadoArticulo> estados = new List<EstadoArticulo>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                EstadoArticulo estado = document.ConvertTo<EstadoArticulo>();
                estados.Add(estado);
            }

            if (soloActivos)
                estados = estados.Where(e => e.Activo).ToList();

            List<EstadoArticuloDto> estadosDto = _mapper.Map<List<EstadoArticuloDto>>(estados);
            return ApiResponse<List<EstadoArticuloDto>>.Success(estadosDto, $"Se encontraron {estadosDto.Count} estado(s) de artículo");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<EstadoArticuloDto>>.Failure($"Error al obtener estados de artículo: {ex.Message}");
        }
    }

    public async Task<ApiResponse<EstadoArticuloDto>> GetEstadoArticuloById(string id)
    {
        try
        {
            CollectionReference collection = _firebaseService.GetCollection("estados_articulo");
            DocumentSnapshot docSnapshot = await collection.Document(id).GetSnapshotAsync();

            if (!docSnapshot.Exists)
                return ApiResponse<EstadoArticuloDto>.Failure("No se encontró el estado de artículo");

            EstadoArticulo estado = docSnapshot.ConvertTo<EstadoArticulo>();
            EstadoArticuloDto estadoDto = _mapper.Map<EstadoArticuloDto>(estado);

            return ApiResponse<EstadoArticuloDto>.Success(estadoDto, "Estado de artículo encontrado");
        }
        catch (Exception ex)
        {
            return ApiResponse<EstadoArticuloDto>.Failure($"Error al obtener estado de artículo: {ex.Message}");
        }
    }

    public async Task<ApiResponse<EstadoArticuloDto>> CreateEstadoArticulo(CreateEstadoArticuloDto dto)
    {
        try
        {
            UsuarioAutenticado usuarioActual = _usuarioAutenticadoService.ObtenerUsuarioActual();

            CollectionReference collection = _firebaseService.GetCollection("estados_articulo");

            QuerySnapshot existing = await collection
                .WhereEqualTo("Descripcion", dto.Descripcion)
                .GetSnapshotAsync();

            if (existing.Count > 0)
                return ApiResponse<EstadoArticuloDto>.Failure($"Ya existe un estado de artículo con la descripción '{dto.Descripcion}'");

            EstadoArticulo estado = new EstadoArticulo
            {
                Id = Guid.NewGuid().ToString(),
                Descripcion = dto.Descripcion,
                UsuarioAgrega = usuarioActual.NameIdentifier,
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            };

            if (!estado.IsValid(out string validationMessage))
                return ApiResponse<EstadoArticuloDto>.Failure($"Datos inválidos: {validationMessage}");

            await collection.Document(estado.Id).SetAsync(estado);

            EstadoArticuloDto estadoDto = _mapper.Map<EstadoArticuloDto>(estado);
            return ApiResponse<EstadoArticuloDto>.Success(estadoDto, "Estado de artículo creado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<EstadoArticuloDto>.Failure($"Error al crear estado de artículo: {ex.Message}");
        }
    }

    public async Task<ApiResponse<EstadoArticuloDto>> UpdateEstadoArticulo(UpdateEstadoArticuloDto dto)
    {
        try
        {
            UsuarioAutenticado usuarioActual = _usuarioAutenticadoService.ObtenerUsuarioActual();

            CollectionReference collection = _firebaseService.GetCollection("estados_articulo");
            DocumentSnapshot docSnapshot = await collection.Document(dto.Id).GetSnapshotAsync();

            if (!docSnapshot.Exists)
                return ApiResponse<EstadoArticuloDto>.Failure("No se encontró el estado de artículo");

            QuerySnapshot existing = await collection
                .WhereEqualTo("Descripcion", dto.Descripcion)
                .GetSnapshotAsync();

            if (existing.Count > 0 && existing.Documents[0].Id != dto.Id)
                return ApiResponse<EstadoArticuloDto>.Failure($"Ya existe otro estado de artículo con la descripción '{dto.Descripcion}'");

            EstadoArticulo estado = docSnapshot.ConvertTo<EstadoArticulo>();
            estado.Descripcion = dto.Descripcion;
            estado.UsuarioModifica = usuarioActual.NameIdentifier;
            estado.FechaModifica = DateTime.UtcNow;

            if (!estado.IsValid(out string validationMessage))
                return ApiResponse<EstadoArticuloDto>.Failure($"Datos inválidos: {validationMessage}");

            await collection.Document(estado.Id).SetAsync(estado);

            EstadoArticuloDto estadoDto = _mapper.Map<EstadoArticuloDto>(estado);
            return ApiResponse<EstadoArticuloDto>.Success(estadoDto, "Estado de artículo actualizado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<EstadoArticuloDto>.Failure($"Error al actualizar estado de artículo: {ex.Message}");
        }
    }

    public async Task<ApiResponse<EstadoArticuloDto>> ChangeStateEstadoArticulo(string id)
    {
        try
        {
            UsuarioAutenticado usuarioActual = _usuarioAutenticadoService.ObtenerUsuarioActual();

            CollectionReference collection = _firebaseService.GetCollection("estados_articulo");
            DocumentSnapshot docSnapshot = await collection.Document(id).GetSnapshotAsync();

            if (!docSnapshot.Exists)
                return ApiResponse<EstadoArticuloDto>.Failure("No se encontró el estado de artículo");

            EstadoArticulo estado = docSnapshot.ConvertTo<EstadoArticulo>();
            estado.Activo = !estado.Activo;
            estado.UsuarioModifica = usuarioActual.NameIdentifier;
            estado.FechaModifica = DateTime.UtcNow;

            await collection.Document(id).SetAsync(estado);

            EstadoArticuloDto estadoDto = _mapper.Map<EstadoArticuloDto>(estado);
            string mensaje = estado.Activo ? "Estado de artículo activado exitosamente" : "Estado de artículo desactivado exitosamente";
            return ApiResponse<EstadoArticuloDto>.Success(estadoDto, mensaje);
        }
        catch (Exception ex)
        {
            return ApiResponse<EstadoArticuloDto>.Failure($"Error al actualizar estado de artículo: {ex.Message}");
        }
    }
}
