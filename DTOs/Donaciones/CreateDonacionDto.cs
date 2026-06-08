namespace ProyectoDonacion.DTOs.Donaciones
{
    public class CreateDonacionDto
    {
        public string NombreArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public string CategoriaId { get; set; }
        public string EstadoArticuloId { get; set; }
        public string SucursalId { get; set; }
    }
}
