using ProyectoDonacion.Common.DTOs;
using ProyectoDonacion.DTOs.Categorias;
using ProyectoDonacion.DTOs.EstadoArticulos;
using ProyectoDonacion.DTOs.EstadoDonaciones;

namespace ProyectoDonacion.DTOs.Donaciones
{
    public class DonacionDto: EntidadBaseDto
    {
        public string NombreArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public string CategoriaId { get; set; }
        public string EstadoArticuloId { get; set; }
        public string EstadoDonacionId { get; set; }
        public string ZonEntrega { get; set; }
        public string UrlImagen { get; set; }

        public CategoriaDto Categoria { get; set; }
        public EstadoArticuloDto EstadoArticulo { get; set; }
        public EstadoDonacionDto EstadoDonacion { get; set; }
    }
}
