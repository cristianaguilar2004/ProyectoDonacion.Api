
namespace ProyectoDonacion.Common.DTOs
{
    public class EntidadBaseDto
    {
        public string Id { get; set; }
        public string UsuarioAgrega { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }
        public bool Activo { get; set; }
    }
}
