﻿using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoContext cityInfoContext;

    public CityInfoRepository(CityInfoContext cityInfoContext)
    {
        this.cityInfoContext = cityInfoContext ?? throw new ArgumentNullException(nameof(cityInfoContext));
    }

    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await this.cityInfoContext.Cities.OrderBy(city => city.Name)
                                                .ToListAsync();
    }

    public async Task<IEnumerable<City>> GetCitiesAsync(string? name)
    {
        if(string.IsNullOrEmpty(name))
        {
            return await this.GetCitiesAsync();
        }

        name = name.Trim();

        return await this.cityInfoContext.Cities.Where(city => city.Name == name)
                                                .OrderBy(city => city.Name)
                                                .ToListAsync();
    }

    public async Task<City?> GetCityAsync(int cityId, bool includePointOfInterest)
    {
        if (includePointOfInterest)
        {
            return await this.cityInfoContext.Cities.Include(city => city.PointOfInterests)
                                                    .Where(city => city.Id == cityId)
                                                    .FirstOrDefaultAsync();
        }

        return await this.cityInfoContext.Cities.Where(city => city.Id == cityId)
                                                .FirstOrDefaultAsync();
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await this.cityInfoContext.Cities.AnyAsync(city => city.Id == cityId);
    }

    public async Task<PointOfInterest?> GetPointOfInterestsForCityAsync(int cityId, int pointOfInterestId)
    {
        return await this.cityInfoContext.PointOfInterests.Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                                                          .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId)
    {
        return await this.cityInfoContext.PointOfInterests.Where(p => p.CityId == cityId)
                                                          .ToListAsync();
    }

    public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityAsync(cityId, false);

        if (city != null) 
        {
            city.PointOfInterests.Add(pointOfInterest);
        }
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        this.cityInfoContext.Remove(pointOfInterest);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await this.cityInfoContext.SaveChangesAsync() >= 0);
    }
}
