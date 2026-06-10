using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ProyectoDonacion.Common;
using System.Net;

namespace ProyectoDonacion.Services.Donaciones
{
    public class FileService
    {
        private readonly Cloudinary _cloudinary;
        public FileService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<ApiResponse<string>> SubirAdjuntos(IFormFileCollection files)
        {
            try
            {
                IFormFile file = files.FirstOrDefault();

                if (file == null)
                    return ApiResponse<string>.Warning("No se encontro ningún archivo");

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };

                ImageUploadResult uploadResult = _cloudinary.Upload(uploadParams);
                if (uploadResult.StatusCode != HttpStatusCode.OK)
                    return ApiResponse<string>.Warning("Error al cargar subir el archivo");

                return ApiResponse<string>.Success(uploadResult.Url.ToString(), "Archivo subido exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure($"Error al subir el archivo: {ex.Message}");   
            }
        }

    }
}
