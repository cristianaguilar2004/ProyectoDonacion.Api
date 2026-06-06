using AutoMapper;
using Google.Cloud.Firestore;
using ProyectoDonacion.Common;
using ProyectoDonacion.DTOs.Donaciones;
using ProyectoDonacion.Services.Auth;
using ProyectoDonacion.Services.FireBase;

namespace ProyectoDonacion.Services.Donaciones
{
    public class DonacionService
    {
        private readonly FirebaseService _firebaseService;
        private readonly IMapper _mapper;
        private readonly UsuarioAutenticadoService _usuarioAutenticadoService;

        public DonacionService(FirebaseService firebaseService, 
                               IMapper mapper, 
                               UsuarioAutenticadoService usuarioAutenticadoService)
        {
            _firebaseService = firebaseService;
            _mapper = mapper;
            _usuarioAutenticadoService = usuarioAutenticadoService;
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


                

                return ApiResponse<List<DonacionDto>>.Success(_mapper.Map<List<DonacionDto>>(donaciones));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DonacionDto>>.Failure($"Error al obtener las donaciones: {ex.Message}");
            }
        }

    }
}
