using AutoMapper;
using Google.Cloud.Firestore;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.EstadoDonaciones;
using ProyectoDonacion.Models.Auth;
using ProyectoDonacion.Models.EstadoDonaciones;
using ProyectoDonacion.Services.Auth;
using ProyectoDonacion.Services.FireBase;

namespace ProyectoDonacion.Services.EstadoDonaciones;

public class EstadoDonacionService
{
    private readonly FirebaseService _firebaseService;
    private readonly IMapper _mapper;
    private readonly UsuarioAutenticadoService _usuarioAutenticadoService;

    public EstadoDonacionService(FirebaseService firebaseService, IMapper mapper, UsuarioAutenticadoService usuarioAutenticadoService)
    {
        _firebaseService = firebaseService;
        _mapper = mapper;
        _usuarioAutenticadoService = usuarioAutenticadoService;
    }

    public async Task<ApiResponse<List<EstadoDonacionDto>>> GetAllEstadosDonacion(bool soloActivos = true)
    {
        try
        {
            CollectionReference collection = _firebaseService.GetCollection("estados_donacion");
            QuerySnapshot snapshot = await collection.GetSnapshotAsync();

            List<EstadoDonacion> estados = new List<EstadoDonacion>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                EstadoDonacion estado = document.ConvertTo<EstadoDonacion>();
                estados.Add(estado);
            }

            if (soloActivos)
                estados = estados.Where(e => e.Activo).ToList();

            List<EstadoDonacionDto> estadosDto = _mapper.Map<List<EstadoDonacionDto>>(estados);
            return ApiResponse<List<EstadoDonacionDto>>.Success(estadosDto, $"Se encontraron {estadosDto.Count} estado(s) de donación");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<EstadoDonacionDto>>.Failure($"Error al obtener estados de donación: {ex.Message}");
        }
    }

    public async Task<ApiResponse<EstadoDonacionDto>> GetEstadoDonacionById(string id)
    {
        try
        {
            CollectionReference collection = _firebaseService.GetCollection("estados_donacion");
            DocumentSnapshot docSnapshot = await collection.Document(id).GetSnapshotAsync();

            if (!docSnapshot.Exists)
                return ApiResponse<EstadoDonacionDto>.Failure("No se encontró el estado de donación");

            EstadoDonacion estado = docSnapshot.ConvertTo<EstadoDonacion>();
            EstadoDonacionDto estadoDto = _mapper.Map<EstadoDonacionDto>(estado);

            return ApiResponse<EstadoDonacionDto>.Success(estadoDto, "Estado de donación encontrado");
        }
        catch (Exception ex)
        {
            return ApiResponse<EstadoDonacionDto>.Failure($"Error al obtener estado de donación: {ex.Message}");
        }
    }

    public async Task<ApiResponse<EstadoDonacionDto>> CreateEstadoDonacion(CreateEstadoDonacionDto dto)
    {
        try
        {
            UsuarioAutenticado usuarioActual = _usuarioAutenticadoService.ObtenerUsuarioActual();

            CollectionReference collection = _firebaseService.GetCollection("estados_donacion");

            QuerySnapshot existing = await collection
                .WhereEqualTo("Descripcion", dto.Descripcion)
                .GetSnapshotAsync();

            if (existing.Count > 0)
                return ApiResponse<EstadoDonacionDto>.Failure($"Ya existe un estado de donación con la descripción '{dto.Descripcion}'");

            EstadoDonacion estado = new EstadoDonacion
            {
                Id = Guid.NewGuid().ToString(),
                Descripcion = dto.Descripcion,
                UsuarioAgrega = usuarioActual.EmailAddress,
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            };

            if (!estado.IsValid(out string validationMessage))
                return ApiResponse<EstadoDonacionDto>.Failure($"Datos inválidos: {validationMessage}");

            await collection.Document(estado.Id).SetAsync(estado);

            EstadoDonacionDto estadoDto = _mapper.Map<EstadoDonacionDto>(estado);
            return ApiResponse<EstadoDonacionDto>.Success(estadoDto, "Estado de donación creado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<EstadoDonacionDto>.Failure($"Error al crear estado de donación: {ex.Message}");
        }
    }

    public async Task<ApiResponse<EstadoDonacionDto>> UpdateEstadoDonacion(UpdateEstadoDonacionDto dto)
    {
        try
        {
            UsuarioAutenticado usuarioActual = _usuarioAutenticadoService.ObtenerUsuarioActual();

            CollectionReference collection = _firebaseService.GetCollection("estados_donacion");
            DocumentSnapshot docSnapshot = await collection.Document(dto.Id).GetSnapshotAsync();

            if (!docSnapshot.Exists)
                return ApiResponse<EstadoDonacionDto>.Failure("No se encontró el estado de donación");

            QuerySnapshot existing = await collection
                .WhereEqualTo("Descripcion", dto.Descripcion)
                .GetSnapshotAsync();

            if (existing.Count > 0 && existing.Documents[0].Id != dto.Id)
                return ApiResponse<EstadoDonacionDto>.Failure($"Ya existe otro estado de donación con la descripción '{dto.Descripcion}'");

            EstadoDonacion estado = docSnapshot.ConvertTo<EstadoDonacion>();
            estado.Descripcion = dto.Descripcion;
            estado.UsuarioModifica = usuarioActual.EmailAddress;
            estado.FechaModifica = DateTime.UtcNow;

            if (!estado.IsValid(out string validationMessage))
                return ApiResponse<EstadoDonacionDto>.Failure($"Datos inválidos: {validationMessage}");

            await collection.Document(estado.Id).SetAsync(estado);

            EstadoDonacionDto estadoDto = _mapper.Map<EstadoDonacionDto>(estado);
            return ApiResponse<EstadoDonacionDto>.Success(estadoDto, "Estado de donación actualizado exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<EstadoDonacionDto>.Failure($"Error al actualizar estado de donación: {ex.Message}");
        }
    }

    public async Task<ApiResponse<EstadoDonacionDto>> ChangeStateEstadoDonacion(string id)
    {
        try
        {
            UsuarioAutenticado usuarioActual = _usuarioAutenticadoService.ObtenerUsuarioActual();

            CollectionReference collection = _firebaseService.GetCollection("estados_donacion");
            DocumentSnapshot docSnapshot = await collection.Document(id).GetSnapshotAsync();

            if (!docSnapshot.Exists)
                return ApiResponse<EstadoDonacionDto>.Failure("No se encontró el estado de donación");

            EstadoDonacion estado = docSnapshot.ConvertTo<EstadoDonacion>();
            estado.Activo = !estado.Activo;
            estado.UsuarioModifica = usuarioActual.EmailAddress;
            estado.FechaModifica = DateTime.UtcNow;

            await collection.Document(id).SetAsync(estado);

            EstadoDonacionDto estadoDto = _mapper.Map<EstadoDonacionDto>(estado);
            string mensaje = estado.Activo ? "Estado de donación activado exitosamente" : "Estado de donación desactivado exitosamente";
            return ApiResponse<EstadoDonacionDto>.Success(estadoDto, mensaje);
        }
        catch (Exception ex)
        {
            return ApiResponse<EstadoDonacionDto>.Failure($"Error al actualizar estado de donación: {ex.Message}");
        }
    }
}
