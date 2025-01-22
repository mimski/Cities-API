using Asp.Versioning;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
[Authorize(Policy = "MustBeFromAntwerp")]
[ApiController]
[ApiVersion(2)]
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
        // for demo purpose: we are looking for city claim. It would have been better if we have an ID istead of city name, but for demo purposes this will do
        var cityName = User.Claims.FirstOrDefault(city => city.Type == "city")?.Value;

        if (!await this.cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
        {
            return Forbid();
        }

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

    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
    {
        if (!await this.cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var finalPointOfInterest = this.mapper.Map<Entities.PointOfInterest>(pointOfInterest);

        await this.cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
        await this.cityInfoRepository.SaveChangesAsync();

        var createPointOfInterestToReturn = this.mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, pointOfInterestId = createPointOfInterestToReturn.Id }, createPointOfInterestToReturn);
    }

    [HttpPut("{pointofinterestid}")]
    public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
    {
        if (!await this.cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await this.cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        this.mapper.Map(pointOfInterest, pointOfInterestEntity);

        await this.cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{pointofinterestid}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        if (!await this.cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await this.cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        var pointOfInterestToPatch = this.mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (!TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(ModelState);
        }

        this.mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
        await this.cityInfoRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{pointOfInterestId}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await this.cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await this.cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestId);

        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        this.cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
        await this.cityInfoRepository.SaveChangesAsync();

        mailService.Send("Point of interst deleted.", $"Name: {pointOfInterestEntity.Name}, ID: {pointOfInterestEntity.Id}");

        return NoContent();
    }
}
