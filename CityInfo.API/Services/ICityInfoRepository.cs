using CityInfo.API.Entities;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();

    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

    Task<bool> CityExistsAsync(int cityId);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId);

    Task<PointOfInterest?> GetPointOfInterestsForCityAsync(int cityId, int pointOfInterestId);
}
