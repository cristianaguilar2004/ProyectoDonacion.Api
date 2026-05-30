namespace ProyectoDonacion.Common;

/// <summary>
/// Tipos de respuesta para la API
/// </summary>
public enum ResponseType
{
    /// <summary>
    /// Operación exitosa
    /// </summary>
    Ok = 1,

    /// <summary>
    /// Advertencia - operación completada con observaciones
    /// </summary>
    Warn,

    /// <summary>
    /// Error en la operación
    /// </summary>
    Error,

    /// <summary>
    /// Información general
    /// </summary>
    Info
}
