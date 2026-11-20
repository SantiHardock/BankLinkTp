using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankLink.Filters;

// Filtro personalizado para proteger nuestros endpoints de transferencias entrantes.
// Si el grupo que nos consume no manda el header "X-API-Key" con nuestra clave, lo rebotamos.
public class ApiKeyAttribute : Attribute, IActionFilter
{
    private const string NombreHeader = "X-API-Key";

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Traemos la configuración para leer la clave que definimos en el appsettings
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var keyEsperada = config["ApiExterna:ApiKey"];

        // Nos fijamos si la petición trae el header correcto
        var tieneHeader = context.HttpContext.Request.Headers.TryGetValue(NombreHeader, out var keyRecibida);

        // Si no trae el header o la clave es incorrecta, clavamos un 401 Unauthorized
        if (!tieneHeader || string.IsNullOrEmpty(keyEsperada) || keyRecibida.ToString() != keyEsperada)
        {
            context.Result = new UnauthorizedObjectResult(new { mensaje = "Acceso denegado: API Key faltante o incorrecta." });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) 
    { 
        // No necesitamos hacer nada después de que se ejecute la acción
    }
}