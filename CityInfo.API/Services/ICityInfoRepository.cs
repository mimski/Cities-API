﻿using CityInfo.API.Entities;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();

    Task<City?> GetCityAsync(int cityId);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId);

    Task<PointOfInterest?> GetPointOfInterestsForCityAsync(int cityId, int pointOfInterestId);
}
