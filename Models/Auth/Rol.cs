using Google.Cloud.Firestore;

namespace ProyectoDonacion.Models.Auth
{
    [FirestoreData]
    public class Rol
    {
        [FirestoreProperty("Id")]
        public string Id { get; set; }

        [FirestoreProperty("Nombre")]
        public string Nombre { get; set; }

        [FirestoreProperty("Menu")]
        public string Menu { get; set; }

        public bool IsValid(out string mensaje)
        {
            if (string.IsNullOrEmpty(Nombre))
            {
                mensaje = "El nombre del rol no puede estar vacío.";
                return false;
            }

            if (string.IsNullOrEmpty(Menu))
            {
                mensaje = "El menú del rol no puede estar vacío.";
                return false;
            }

            mensaje = string.Empty;
            return true;
        }
    }
}
