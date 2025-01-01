using CityInfo.API.DbContexts;
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
}
