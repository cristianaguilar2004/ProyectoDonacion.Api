using Google.Cloud.Firestore;
using ProyectoDonacion.Common.Models;
using ProyectoDonacion.Models.Categorias;
using ProyectoDonacion.Models.EstadoArticulos;
using ProyectoDonacion.Models.EstadoDonaciones;

namespace ProyectoDonacion.Models.Donaciones
{
    [FirestoreData]
    public class Donacion : EntidadBase
    {
        [FirestoreProperty("NombreArticulo")]
        public string NombreArticulo { get; set; }

        [FirestoreProperty("DescripcionArticulo")]
        public string DescripcionArticulo { get; set; }

        [FirestoreProperty("CategoriaId")]
        public string CategoriaId { get; set; }

        [FirestoreProperty("EstadoArticuloId")]
        public string EstadoArticuloId { get; set; }

        [FirestoreProperty("EstadoDonacionId")]
        public string EstadoDonacionId { get; set; }

        [FirestoreProperty("ZonaEntrega")]
        public string ZonaEntrega { get; set; }

        [FirestoreProperty("UrlImagen")]
        public string UrlImagen { get; set; }


        public Categoria Categoria { get; set; }
        public EstadoArticulo EstadoArticulo { get; set; }
        public EstadoDonacion EstadoDonacion { get; set; }


        public bool IsValid(out string validationMessage)
        {
            if (string.IsNullOrWhiteSpace(NombreArticulo))
            {
                validationMessage = "El nombre del artículo es requerido";
                return false;
            }

            if (string.IsNullOrWhiteSpace(DescripcionArticulo))
            {
                validationMessage = "La descripcion del artículo es requerida";
                return false;
            }

            if (string.IsNullOrWhiteSpace(CategoriaId))
            {
                validationMessage = "La categoría del artículo es requerida";
                return false;
            }

            if (string.IsNullOrWhiteSpace(EstadoArticuloId))
            {
                validationMessage = "El estado del artículo es requerido";
                return false;
            }

            if (string.IsNullOrWhiteSpace(EstadoDonacionId))
            {
                validationMessage = "El estado de la donación es requerido";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ZonaEntrega))
            {
                validationMessage = "La zona de entrega es requerida";
                return false;
            }

            validationMessage = string.Empty;
            return true;
        }
    }
}
