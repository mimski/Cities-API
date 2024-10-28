using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly CitiesDataStore citiesDataStore;

    public CitiesController(CitiesDataStore citiesDataStore)
    {
        this.citiesDataStore = citiesDataStore;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCities() 
    {
        return Ok(citiesDataStore.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id) 
    {
        var city = citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);

        if (city == null)
        {
            return NotFound();
        }

        return Ok(city);
    }
}
