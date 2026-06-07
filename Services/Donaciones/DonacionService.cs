using AutoMapper;
using Google.Api;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.Categorias;
using ProyectoDonacion.DTOs.Donaciones;
using ProyectoDonacion.DTOs.EstadoArticulos;
using ProyectoDonacion.DTOs.EstadoDonaciones;
using ProyectoDonacion.DTOs.Sucursales;
using ProyectoDonacion.Services.Auth;
using ProyectoDonacion.Services.Categorias;
using ProyectoDonacion.Services.EstadoArticulos;
using ProyectoDonacion.Services.EstadoDonaciones;
using ProyectoDonacion.Services.FireBase;
using ProyectoDonacion.Services.Sucursales;

namespace ProyectoDonacion.Services.Donaciones
{
    public class DonacionService
    {
        private readonly FirebaseService _firebaseService;
        private readonly IMapper _mapper;
        private readonly UsuarioAutenticadoService _usuarioAutenticadoService;
        private readonly EstadoArticuloService _estadoArticuloService;
        private readonly EstadoDonacionService _estadoDonacionService;
        private readonly CategoriaService _categoriaService;
        private readonly SucursalService _sucursalService;

        public DonacionService(FirebaseService firebaseService, 
                               IMapper mapper, 
                               UsuarioAutenticadoService usuarioAutenticadoService,
                               EstadoArticuloService estadoArticuloService,
                               EstadoDonacionService estadoDonacionService,
                               CategoriaService categoriaService,
                               SucursalService sucursalService)
        {
            _firebaseService = firebaseService;
            _mapper = mapper;
            _usuarioAutenticadoService = usuarioAutenticadoService;
            _estadoArticuloService = estadoArticuloService;
            _estadoDonacionService = estadoDonacionService;
            _categoriaService = categoriaService;
            _sucursalService = sucursalService;
        }

        public async Task<ApiResponse<List<DonacionDto>>> GetDonaciones(bool soloActivos = true)
        {
            try
            {
                CollectionReference collection = _firebaseService.GetCollection("donaciones");
                QuerySnapshot snapshot = await collection.GetSnapshotAsync();

                List<DonacionDto> donaciones = new List<DonacionDto>();
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    DonacionDto donacion = document.ConvertTo<DonacionDto>();
                    donaciones.Add(donacion);
                }

                if(soloActivos)
                    donaciones  = donaciones.Where(d => d.Activo).ToList();

                ApiResponse<List<EstadoArticuloDto>> estadosResponse = await _estadoArticuloService.GetAllEstadosArticulo();
                ApiResponse<List<EstadoDonacionDto>> estadosDonacionResponse = await _estadoDonacionService.GetAllEstadosDonacion();
                ApiResponse<List<CategoriaDto>> categoriasResponse = await _categoriaService.GetAllCategorias();
                ApiResponse<List<SucursalDto>> sucursalesResponse = await _sucursalService.GetAllSucursales();

                List<DonacionDto> donacionesQuery =
                    (from donacion in donaciones
                     join estadoArticulo in estadosResponse.Data on donacion.EstadoArticuloId equals estadoArticulo.Id
                     join estadoDonacion in estadosDonacionResponse.Data on donacion.EstadoDonacionId equals estadoDonacion.Id
                     join categoria in categoriasResponse.Data on donacion.CategoriaId equals categoria.Id
                     join sucursal in sucursalesResponse.Data on donacion.SucursalId equals sucursal.Id
                     select new DonacionDto
                     {
                         Id = donacion.Id,
                         NombreArticulo = donacion.NombreArticulo,
                         DescripcionArticulo = donacion.DescripcionArticulo,
                         CategoriaId = donacion.CategoriaId,
                         EstadoArticuloId = donacion.EstadoArticuloId,
                         EstadoDonacionId = donacion.EstadoDonacionId,
                         SucursalId = donacion.SucursalId,
                         UrlImagen = donacion.UrlImagen,
                         Categoria = _mapper.Map<CategoriaDto>(categoria),
                         EstadoArticulo = _mapper.Map<EstadoArticuloDto>(estadoArticulo),
                         EstadoDonacion = _mapper.Map<EstadoDonacionDto>(estadoDonacion),
                         Sucursal = _mapper.Map<SucursalDto>(sucursal)
                     }).ToList();


                return ApiResponse<List<DonacionDto>>.Success(donacionesQuery, "Donaciones cargadas exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DonacionDto>>.Failure($"Error al obtener las donaciones: {ex.Message}");
            }
        }


        public async Task<ApiResponse<DonacionDto>> CreateDonacion(IFormCollection form)
        {
            try
            {
                DonacionDto donacionDto = JsonConvert.DeserializeObject<DonacionDto>(form["datos"]);


                return ApiResponse<DonacionDto>.Success(new DonacionDto(), "Donacion creada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<DonacionDto>.Failure($"Error al crear la donacion: {ex.Message}");
            }
        }

    }
}
