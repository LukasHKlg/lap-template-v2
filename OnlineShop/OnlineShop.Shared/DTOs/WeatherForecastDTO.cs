namespace OnlineShop.Shared.DTOs
{
    public class WeatherForecastDTO
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string? Summary { get; set; }

        public WeatherForecastDTO(DateOnly date, int temperatureC, int temperatureF, string? summary)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
            TemperatureF = temperatureF;
        }
    }
}
