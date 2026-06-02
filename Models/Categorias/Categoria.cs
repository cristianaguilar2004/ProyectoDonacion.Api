using Google.Cloud.Firestore;
using ProyectoDonacion.Common.Models;

namespace ProyectoDonacion.Models.Categorias
{
    [FirestoreData]
    public class Categoria : EntidadBase
    {
        [FirestoreProperty("Nombre")]
        public string Nombre { get; set; }

        public bool IsValid(out string message)
        {
            if(string.IsNullOrEmpty(Nombre)) {
                message = "El nombre de la categoría es obligatorio.";
                return false;
            }

            message = "";
            return true;
        }
    }
}
