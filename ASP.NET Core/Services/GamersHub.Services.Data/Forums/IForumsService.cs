﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Forums
{
    public interface IForumsService
    {
        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetAllByCategoryId<T>(int id);

        T GetByName<T>(string name);

        T GetById<T>(int id);

        Task<int> CreateAsync(string name);

        Task<int> EditAsync(int id, string name, int[] categoryIds, bool[] areSelected);

        Task DeleteAsync(int id);

        int GetCount();
    }
}
