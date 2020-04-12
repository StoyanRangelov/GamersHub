using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GamersHub.Services.Data.Parties
{
    public class PartiesService : IPartiesService
    {
        private readonly IDeletableEntityRepository<Party> partiesRepository;
        private readonly IRepository<PartyUser> partyUsersRepository;

        public PartiesService(IDeletableEntityRepository<Party> partiesRepository,
            IRepository<PartyUser> partyUsersRepository)
        {
            this.partiesRepository = partiesRepository;
            this.partyUsersRepository = partyUsersRepository;
        }

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0)
        {
            var query = this.partiesRepository.All()
                .OrderByDescending(x => x.CreatedOn).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllByUsername<T>(string username, int? take = null, int skip = 0)
        {
            var query = this.partiesRepository.All()
                .Where(x => x.Creator.UserName == username)
                .OrderByDescending(x => x.CreatedOn).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetCount()
        {
            return this.partiesRepository.All().Count();
        }

        public int GetCountByUsername(string username)
        {
            return this.partiesRepository.All().Count(x => x.Creator.UserName == username);
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

        public async Task<int> ApplyAsync(int partyId, string userId)
        {
            var party = this.partiesRepository.All()
                .Include(x => x.PartyApplicants)
                .FirstOrDefault(x => x.Id == partyId);

            if (party == null)
            {
                return 0;
            }

            var applicant = party.PartyApplicants
                .FirstOrDefault(x => x.PartyId == partyId && x.ApplicantId == userId);

            if (applicant != null)
            {
                return -1;
            }

            var partyApplicant = new PartyUser
            {
                PartyId = partyId,
                ApplicantId = userId,
            };

            party.PartyApplicants.Add(partyApplicant);

            this.partiesRepository.Update(party);
            await this.partiesRepository.SaveChangesAsync();

            return partyId;
        }

        public async Task<int> ApproveAsync(int partyId, string applicantId)
        {
            var party = this.partiesRepository.All()
                .Include(x=>x.PartyApplicants)
                .FirstOrDefault(x => x.Id == partyId);

            var partyApplication = party?.PartyApplicants
                .FirstOrDefault(x => x.ApplicantId == applicantId);

            if (partyApplication == null)
            {
                return 0;
            }

            partyApplication.IsApproved = true;

            var approvedApplicants = party.PartyApplicants.Count(x => x.IsApproved);

            if (approvedApplicants == party.Size)
            {
                party.IsFull = true;
            }

            this.partiesRepository.Update(party);
            await this.partiesRepository.SaveChangesAsync();

            return partyId;
        }
    }
}