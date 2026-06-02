namespace ProyectoDonacion.Common;

/// <summary>
/// Respuesta genérica de la API
/// </summary>
/// <typeparam name="T">Tipo de dato que contiene la respuesta</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Datos de la respuesta
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Tipo de respuesta (Ok, Warn, Error, Info)
    /// </summary>
    public ResponseType Type { get; set; }

    /// <summary>
    /// Mensaje descriptivo de la respuesta
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public ApiResponse()
    {
    }

    /// <summary>
    /// Constructor con parámetros
    /// </summary>
    public ApiResponse(T? data, ResponseType type, string message)
    {
        Data = data;
        Type = type;
        Message = message;
    }

    /// <summary>
    /// Crea una respuesta exitosa
    /// </summary>
    public static ApiResponse<T> Success(T data, string message = "Operación exitosa")
    {
        return new ApiResponse<T>(data, ResponseType.Ok, message);
    }

    /// <summary>
    /// Crea una respuesta de error
    /// </summary>
    public static ApiResponse<T> Failure(string message)
    {
        return new ApiResponse<T>(default, ResponseType.Error, message);
    }

    /// <summary>
    /// Crea una respuesta de advertencia
    /// </summary>
    public static ApiResponse<T> Warning(string message)
    {
        return new ApiResponse<T>(default, ResponseType.Warn, message);
    }

    /// <summary>
    /// Crea una respuesta informativa
    /// </summary>
    public static ApiResponse<T> Information(T? data, string message)
    {
        return new ApiResponse<T>(data, ResponseType.Info, message);
    }
}
