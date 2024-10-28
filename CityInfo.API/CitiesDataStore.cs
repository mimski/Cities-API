using CityInfo.API.Models;

namespace CityInfo.API;

public class CitiesDataStore
{
    public CitiesDataStore()
    {
        Cities =
        [
            new()
            {
                Id = 1,
                Name = "New York",
                Description = "The one with that big park.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Central Park",
                        Description = "The most visited urban park."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Empire State Building",
                        Description = "A 102-story skyscraper located in Midtown Manhattan."
                    }
                ]
            },
            new()
            {
                Id = 2,
                Name = "Paris",
                Description = "The one with that big tower.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 3,
                        Name = "Eiffel Tower",
                        Description = "An iron tower."
                    },
                    new()
                    {
                        Id = 4,
                        Name = "The Louvre",
                        Description = "The world's largest museum."
                    }
                ]
            },
            new()
            {
                Id = 3,
                Name = "Antwerp",
                Description = "The one with that unfinished cathedral.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 5,
                        Name = "Cathedral of Our Lady",
                        Description = "A Gothic style cathedral."
                    },
                    new()
                    {
                        Id = 6,
                        Name = "Antwerp Central Station",
                        Description = "Railway in Belgium."
                    }
                ]
            }
        ];
    }

    public List<CityDto> Cities { get; set; }

    //public static CitiesDataStore Current { get; } = new CitiesDataStore();
}
