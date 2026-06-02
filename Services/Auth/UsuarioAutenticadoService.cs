using ProyectoDonacion.Models.Auth;
using System.Security.Claims;

namespace ProyectoDonacion.Services.Auth;

public class UsuarioAutenticadoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioAutenticadoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UsuarioAutenticado ObtenerUsuarioActual()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return new UsuarioAutenticado();
        }

        return new UsuarioAutenticado
        {
            NameIdentifier = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
            EmailAddress = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            Role = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty
        };
    }
}
