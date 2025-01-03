using AutoMapper;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository cityInfoRepository;
    private readonly IMapper mapper;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        this.cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities() 
    {
        var cityEntities = await this.cityInfoRepository.GetCitiesAsync();

        return Ok(this.mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
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
