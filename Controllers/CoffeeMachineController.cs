using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachineAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CoffeeMachineController : ControllerBase
    {
        [HttpGet("brew-coffee")]
        public IActionResult Get()
        {
            var response = new
            {
                message = "Your piping hot coffee is ready",
                prepared = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")
            };

            return Ok(response);
        }    
    }
}
