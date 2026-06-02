namespace ProyectoDonacion.Models.Auth;

public class UsuarioAutenticado
{
    public string NameIdentifier { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public bool IsAuthenticated => !string.IsNullOrEmpty(NameIdentifier);
}
