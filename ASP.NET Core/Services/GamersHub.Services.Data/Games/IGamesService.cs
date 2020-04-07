﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Games
{
    public interface IGamesService
    {
        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>();

        Task<int> CreateAsync(string title, string subTitle, string description, string imageUrl);
    }
}