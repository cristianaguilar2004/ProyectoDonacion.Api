using ProyectoDonacion.Common.DTOs;

namespace ProyectoDonacion.DTOs.EstadoDonaciones;

public class EstadoDonacionDto : EntidadBaseDto
{
    public string Descripcion { get; set; } = string.Empty;
}
