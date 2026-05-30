namespace ProyectoDonacion.DTOs.Auth;

/// <summary>
/// DTO para crear un nuevo rol
/// </summary>
public class CreateRolDto
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Menu { get; set; } = string.Empty;
}
