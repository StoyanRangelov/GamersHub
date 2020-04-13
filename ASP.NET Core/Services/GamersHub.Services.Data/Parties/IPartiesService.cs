﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Parties
{
    public interface IPartiesService
    {
        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetAllByUsername<T>(string username, int? take = null, int skip = 0);

        IEnumerable<T> GetAllApplicationsByUsername<T>(string username, int? take = null, int skip = 0);

        int GetCount();

        int GetCountByUsername(string username);

        Task<int> CreateAsync(string userId, string game, string activity, string description, int size);

        Task<int> ApplyAsync(int partyId, string userId);

        Task<int> ApproveAsync(int partyId, string applicantId);

        Task<int> DeclineAsync(int partyId, string applicantId);
    }
}