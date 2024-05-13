using CoffeeMachineAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachineAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CoffeeMachineController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public CoffeeMachineController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("brew-coffee")]
        public async Task<IActionResult> Get()
        {
            double? temperature = await _weatherService.GetCurrentTemperatureAsync();
            string responseMessage = "Your piping hot coffee is ready";

            // Check if temperature has a value and return error response if null
            if (temperature == null)            
                return StatusCode(500);            

            // Change the response message if the temperature is greater than 30 degree celsius
            else if (temperature > 30)            
                responseMessage = "Your refreshing iced coffee is ready";            

            var response = new
            {
                message = responseMessage,
                prepared = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")
            };

            return Ok(response);
        }    
    }
}
