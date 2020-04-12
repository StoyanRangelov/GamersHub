using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Services.Data.Parties
{
    public class PartiesService : IPartiesService
    {
        private readonly IDeletableEntityRepository<Party> partiesRepository;

        public PartiesService(IDeletableEntityRepository<Party> partiesRepository)
        {
            this.partiesRepository = partiesRepository;
        }

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0)
        {
            var query = this.partiesRepository.All()
                .Where(x => x.IsFull == false)
                .OrderBy(x => x.CreatedOn).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetCount()
        {
            return this.partiesRepository.All().Count(x => x.IsFull == false);
        }

        public async Task<int> CreateAsync(string userId, string game, string activity, string description, int size)
        {
            var party = new Party
            {
                Game = game,
                Activity = activity,
                Description = description,
                Size = size,
                CreatorId = userId,
            };

            await this.partiesRepository.AddAsync(party);
            await this.partiesRepository.SaveChangesAsync();

            return party.Id;
        }
    }
}