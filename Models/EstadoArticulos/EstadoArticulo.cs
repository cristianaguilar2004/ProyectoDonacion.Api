using Google.Cloud.Firestore;
using ProyectoDonacion.Common.Models;

namespace ProyectoDonacion.Models.EstadoArticulos;

[FirestoreData]
public class EstadoArticulo : EntidadBase
{
    [FirestoreProperty("Descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    public bool IsValid(out string validationMessage)
    {
        if (string.IsNullOrWhiteSpace(Descripcion))
        {
            validationMessage = "La descripción es requerida";
            return false;
        }

        if (Descripcion.Length < 3 || Descripcion.Length > 100)
        {
            validationMessage = "La descripción debe tener entre 3 y 100 caracteres";
            return false;
        }

        validationMessage = string.Empty;
        return true;
    }
}
