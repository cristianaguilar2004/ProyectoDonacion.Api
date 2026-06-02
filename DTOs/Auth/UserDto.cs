namespace ProyectoDonacion.DTOs.Auth
{
    public class UserDto
    {
        public string Id { get; set; } 
        public string Nombre { get; set; } 
        public string Email { get; set; }
        public string Rol { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
