using CityInfo.API.Models;

namespace CityInfo.API;

public class CitiesDataStore
{
    public CitiesDataStore()
    {
        Cities = new List<CityDto>
        {
            new()
            {
                Id = 1,
                Name = "New York",
                Description = "The one with that big park."
            },
            new()
            {
                Id = 2,
                Name = "Paris",
                Description = "The one with that big tower."
            },
            new()
            {
                Id = 3,
                Name = "Antwerp",
                Description = "The one with that unfinished cathedral."
            }
        };
    }

    public List<CityDto> Cities { get; set; }

    public static CitiesDataStore Current { get; } = new CitiesDataStore();
}
