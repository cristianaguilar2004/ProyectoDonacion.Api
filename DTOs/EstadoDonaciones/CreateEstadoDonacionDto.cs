using System.ComponentModel.DataAnnotations;

namespace ProyectoDonacion.DTOs.EstadoDonaciones;

public class CreateEstadoDonacionDto
{
    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 100 caracteres")]
    public string Descripcion { get; set; } = string.Empty;
}
