using Google.Cloud.Firestore;
using ProyectoDonacion.Common.Models;

namespace ProyectoDonacion.Models.Sucursales
{
    [FirestoreData]
    public class Sucursal : EntidadBase
    {
        [FirestoreProperty("Nombre")]
        public string Nombre { get; set; }

        [FirestoreProperty("DireccionExacta")]
        public string DireccionExacta { get; set; }


        public bool IsValid(out string validationMessage)
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                validationMessage = "El nombre de la sucursal es obligatorio.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(DireccionExacta))
            {
                validationMessage = "La dirección exacta de la sucursal es obligatoria.";
                return false;
            }
            validationMessage = null;
            return true;
        }
    }
}
