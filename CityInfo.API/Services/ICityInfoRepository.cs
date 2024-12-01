﻿using CityInfo.API.Entities;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
}