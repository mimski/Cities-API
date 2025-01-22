using CityInfo.API.Entities;
using CityInfo.API.Services;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();

    Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

    Task<bool> CityExistsAsync(int cityId);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId);

    Task<PointOfInterest?> GetPointOfInterestsForCityAsync(int cityId, int pointOfInterestId);

    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

    Task<bool> CityNameMatchesCityId(string? cityName, int cityId);

    void DeletePointOfInterest(PointOfInterest pointOfInterest);

    Task<bool> SaveChangesAsync();
}
