using Google.Cloud.Firestore;

namespace ProyectoDonacion.Common.Models
{
    [FirestoreData]
    public class EntidadBase
    {
        [FirestoreProperty("Id")]
        public string Id { get; set; } = string.Empty;

        [FirestoreProperty("UsuarioAgrega")]
        public string UsuarioAgrega { get; set; }


        [FirestoreProperty("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }


        [FirestoreProperty("UsuarioModifica")]
        public string UsuarioModifica { get; set; }


        [FirestoreProperty("FechaModifica")]
        public DateTime? FechaModifica { get; set; }


        [FirestoreProperty("Activo")]
        public bool Activo { get; set; }

    }
}
