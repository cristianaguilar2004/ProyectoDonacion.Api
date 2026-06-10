using AutoMapper;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.Categorias;
using ProyectoDonacion.DTOs.Sucursales;
using ProyectoDonacion.Models.Categorias;
using ProyectoDonacion.Models.Sucursales;
using ProyectoDonacion.Services.Auth;
using ProyectoDonacion.Services.FireBase;

namespace ProyectoDonacion.Services.Sucursales
{
    public class SucursalService
    {
        private readonly FirebaseService _firebaseService;
        private readonly IMapper _mapper;
        private readonly UsuarioAutenticadoService _usuarioAutenticadoService;
        public SucursalService(FirebaseService firebaseService, IMapper mapper, UsuarioAutenticadoService usuarioAutenticadoService)
        {
            _firebaseService = firebaseService;
            _mapper = mapper;
            _usuarioAutenticadoService = usuarioAutenticadoService;
        }


        public async Task<ApiResponse<List<SucursalDto>>> GetAllSucursales(bool soloActivos = true)
        {
            try
            {
                var collection = _firebaseService.GetCollection("sucursales");
                var snapshot = await collection.GetSnapshotAsync();

                List<Sucursal> sucursales = new List<Sucursal>();
                foreach (var document in snapshot.Documents)
                {
                    Sucursal sucursal = document.ConvertTo<Sucursal>();
                    sucursales.Add(sucursal);
                }

                if (soloActivos)
                    sucursales = sucursales.Where(s => s.Activo).ToList();

                List<SucursalDto> sucursalesDto = _mapper.Map<List<SucursalDto>>(sucursales);
                return ApiResponse<List<SucursalDto>>.Success(sucursalesDto, $"Se encontraron {sucursalesDto.Count} sucursal(es)");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SucursalDto>>.Failure($"Error al obtener sucursales: {ex.Message}");
            }
        }


        public async Task<ApiResponse<SucursalDto>> CreateSucursal(CreateSucursalDto dto)
        {
            try
            {
                var collection = _firebaseService.GetCollection("sucursales");
                var existing = await collection
                    .WhereEqualTo("Nombre", dto.Nombre)
                    .GetSnapshotAsync();

                if (existing.Count > 0)
                    return ApiResponse<SucursalDto>.Failure($"Ya existe una sucursal con el nombre '{dto.Nombre}'");

                Sucursal sucursal = new Sucursal
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = dto.Nombre,
                    DireccionExacta = dto.DireccionExacta,
                    UsuarioAgrega = _usuarioAutenticadoService.ObtenerUsuarioActual().NameIdentifier,
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                };

                if (!sucursal.IsValid(out string validationMessage))
                    return ApiResponse<SucursalDto>.Failure($"Datos inválidos: {validationMessage}");

                await collection.Document(sucursal.Id).SetAsync(sucursal);

                SucursalDto sucursalDto = _mapper.Map<SucursalDto>(sucursal);
                return ApiResponse<SucursalDto>.Success(sucursalDto, "Sucursal creada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<SucursalDto>.Failure($"Error al crear sucursal: {ex.Message}");
            }
        }
    }
}
