using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository cityInfoRepository;

    public CitiesController(ICityInfoRepository cityInfoRepository)
    {
        this.cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities() 
    {
        var cityEntities = await this.cityInfoRepository.GetCitiesAsync();

        var results = new List<CityWithoutPointsOfInterestDto>();

        foreach (var cityEntity in cityEntities)
        {
            results.Add(new CityWithoutPointsOfInterestDto 
            {
                Id = cityEntity.Id,
                Name = cityEntity.Name,
                Description = cityEntity.Description
            });
        }
        
        return Ok(results);
    }

    //[HttpGet("{id}")]
    //public ActionResult<CityDto> GetCity(int id) 
    //{
    //    var city = citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);

    //    if (city == null)
    //    {
    //        return NotFound();
    //    }

    //    return Ok(city);
    //}
}
