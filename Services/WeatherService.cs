using System.Text.Json;

namespace CoffeeMachineAPI.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _city = "Auckland,NZ";
        private readonly string _apiKey = "85cd8853c7d61add90fd07118f824bfc";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double?> GetCurrentTemperatureAsync()
        {
            try
            {
                // Fetch Open Weather API
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={_city}&appid={_apiKey}&units=metric";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                // Check if the API response was successful and return the temp value
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        JsonElement root = doc.RootElement;
                        JsonElement main = root.GetProperty("main");
                        return main.GetProperty("temp").GetDouble();
                    }
                }

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

    }
}
