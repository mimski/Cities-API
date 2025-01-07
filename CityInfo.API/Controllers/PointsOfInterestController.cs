using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController : ControllerBase
{
    private readonly ILogger<PointsOfInterestController> logger;
    private readonly IMailService mailService;
    private readonly ICityInfoRepository cityInfoRepository;
    private readonly IMapper mapper;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        this.cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        ;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
    {
        if (!await this.cityInfoRepository.CityExistsAsync(cityId))
        {
            this.logger.LogInformation($"City with id {cityId} was not found when accessing points of interest.");

            return NotFound();
        }

        var pointsOfInterestForCity = this.cityInfoRepository.GetPointsOfInterestsForCityAsync(cityId);

        return Ok(this.mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
    }

    [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await this.cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterest = await this.cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(this.mapper.Map<PointOfInterestDto>(pointOfInterest));
    }

    //[HttpPost]
    //public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
    //{
    //    var city = citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

    //    if (city == null)
    //    {
    //        return NotFound();
    //    }

    //    // concept code
    //    var maxPointOfInterestId = citiesDataStore.Cities.SelectMany(city => city.PointsOfInterest)
    //                                                             .Max(point => point.Id);

    //    var finalPointOfInterest = new PointOfInterestDto
    //    {
    //        Id = ++maxPointOfInterestId,
    //        Name = pointOfInterest.Name,
    //        Description = pointOfInterest.Description,
    //    };

    //    city.PointsOfInterest.Add(finalPointOfInterest);

    //    return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, pointOfInterestId = finalPointOfInterest.Id }, finalPointOfInterest);
    //}

    //[HttpPut("{pointofinterestid}")]
    //public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
    //{
    //    var city = citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

    //    if (city == null)
    //    {
    //        return NotFound();
    //    }

    //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(city => city.Id == pointOfInterestId);

    //    if (pointOfInterestFromStore == null)
    //    {
    //        return NotFound();
    //    }

    //    pointOfInterestFromStore.Name = pointOfInterest.Name;
    //    pointOfInterestFromStore.Description = pointOfInterest.Description;

    //    return NoContent();
    //}

    //[HttpPatch("{pointofinterestid}")]
    //public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    //{
    //    var city = citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

    //    if (city == null)
    //    {
    //        return NotFound();
    //    }

    //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(city => city.Id == pointOfInterestId);

    //    if (pointOfInterestFromStore == null)
    //    {
    //        return NotFound();
    //    }

    //    var pointOfInterestToPatch = new PointOfInterestForUpdateDto
    //    {
    //        Name = pointOfInterestFromStore.Name,
    //        Description = pointOfInterestFromStore.Description,
    //    };

    //    patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest();
    //    }

    //    pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
    //    pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

    //    return NoContent();
    //}

    //[HttpDelete("{pointOfInterestId}")]
    //public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
    //{
    //    var city = citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);

    //    if (city == null)
    //    {
    //        return NotFound();
    //    }

    //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(city => city.Id == pointOfInterestId);

    //    if (pointOfInterestFromStore == null)
    //    {
    //        return NotFound();
    //    }

    //    city.PointsOfInterest.Remove(pointOfInterestFromStore);

    //    mailService.Send("Point of interst deleted.", $"Name: {pointOfInterestFromStore.Name}, ID: {pointOfInterestFromStore.Id}");

    //    return NoContent();
    //}
}
