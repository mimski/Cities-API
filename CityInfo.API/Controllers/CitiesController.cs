﻿using Asp.Versioning;
using AutoMapper;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/cities")]
[ApiVersion(1)]
[ApiVersion(2)]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository cityInfoRepository;
    private readonly IMapper mapper;
    const int MAX_CITIES_PAGE_SIZE = 20;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        this.cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10) 
    {
        if (pageSize > MAX_CITIES_PAGE_SIZE)
        {
            pageSize = MAX_CITIES_PAGE_SIZE;
        }

        var (cityEntities, paginationMetadata) = await this.cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(this.mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    /// <summary>
    /// Get a city by id.
    /// </summary>
    /// <param name="id">The id of the city to get.</param>
    /// <param name="includePointsOfInterest">Whether or not to include the points of interest.</param>
    /// <returns>A city with or without point of interest.</returns>
    /// <response code="200">Returns the requsted city.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {
        var city = await this.cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

        if (city == null)
        {
            return NotFound();
        }

        if (includePointsOfInterest) 
        { 
            return Ok(this.mapper.Map<CityDto>(city));
        }

        return Ok(city);
    }
}
