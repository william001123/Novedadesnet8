using Microsoft.AspNetCore.Mvc;

namespace WebAPI_PDF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private const string SessionKeyName = "Name";
        private const string SessionKeyCounter = "Counter";

        // GET: Obtener el valor almacenado en la sesión
        [HttpGet("get")]
        public IActionResult GetSessionData()
        {
            var name = HttpContext.Session.GetString(SessionKeyName) ?? "Sin definir";
            var counter = HttpContext.Session.GetInt32(SessionKeyCounter) ?? 0;

            return Ok(new
            {
                Name = name,
                Counter = counter
            });
        }

        // POST: Guardar datos en la sesión
        [HttpPost("set")]
        public IActionResult SetSessionData(string name)
        {
            HttpContext.Session.SetString(SessionKeyName, name);
            HttpContext.Session.SetInt32(SessionKeyCounter, 1);

            return Ok(new { Message = "Datos guardados en la sesión." });
        }

        // PUT: Incrementar el contador en la sesión
        [HttpPut("increment")]
        public IActionResult IncrementCounter()
        {
            var counter = HttpContext.Session.GetInt32(SessionKeyCounter) ?? 0;
            counter++;
            HttpContext.Session.SetInt32(SessionKeyCounter, counter);

            return Ok(new { Counter = counter });
        }
    }
}
