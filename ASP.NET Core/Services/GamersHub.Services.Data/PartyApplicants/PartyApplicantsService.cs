using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GamersHub.Services.Data.PartyApplicants
{
    public class PartyApplicantsService : IPartyApplicantsService
    {
        private readonly IRepository<PartyApplicant> partyApplicantsRepository;
        private readonly IDeletableEntityRepository<Party> partiesRepository;

        public PartyApplicantsService(
            IRepository<PartyApplicant> partyApplicantsRepository,
            IDeletableEntityRepository<Party> partiesRepository)
        {
            this.partyApplicantsRepository = partyApplicantsRepository;
            this.partiesRepository = partiesRepository;
        }

        public async Task<int> ApproveAsync(int partyId, string applicantId)
        {
            var party = this.partiesRepository.All()
                .Include(x => x.PartyApplicants)
                .FirstOrDefault(x => x.Id == partyId);

            var partyApplication = party?.PartyApplicants.FirstOrDefault(x => x.ApplicantId == applicantId);

            if (partyApplication == null)
            {
                return 0;
            }

            partyApplication.ApplicationStatus = ApplicationStatusType.Approved;

            var approvedApplicants =
                party.PartyApplicants.Count(x => x.ApplicationStatus == ApplicationStatusType.Approved);

            if (approvedApplicants == party.Size)
            {
                party.IsFull = true;
            }

            this.partiesRepository.Update(party);
            await this.partiesRepository.SaveChangesAsync();

            return party.Id;
        }

        public async Task<int> DeclineAsync(int partyId, string applicantId)
        {
            var partyApplication = this.partyApplicantsRepository.All()
                .FirstOrDefault(x => x.PartyId == partyId && x.ApplicantId == applicantId);

            if (partyApplication == null)
            {
                return 0;
            }

            partyApplication.ApplicationStatus = ApplicationStatusType.Declined;

            this.partyApplicantsRepository.Update(partyApplication);
            await this.partyApplicantsRepository.SaveChangesAsync();

            return partyId;
        }

        public async Task<int> CancelApplicationAsync(int partyId, string applicantId)
        {
            var partyApplication = this.partyApplicantsRepository.All()
                .FirstOrDefault(x => x.PartyId == partyId && x.ApplicantId == applicantId);

            if (partyApplication == null)
            {
                return 0;
            }

            var party = this.partiesRepository.All()
                .First(x => x.Id == partyId);

            if (party.IsFull && partyApplication.ApplicationStatus == ApplicationStatusType.Approved)
            {
                party.IsFull = false;
                this.partiesRepository.Update(party);
                await this.partiesRepository.SaveChangesAsync();
            }

            this.partyApplicantsRepository.Delete(partyApplication);
            await this.partyApplicantsRepository.SaveChangesAsync();

            return partyApplication.PartyId;
        }

        public IEnumerable<T> GetAllApplicationsByUsername<T>(string username, int? take = null, int skip = 0)
        {
            var query = this.partyApplicantsRepository.All()
                .Where(x => x.Applicant.UserName == username)
                .OrderByDescending(x => x.Party.CreatedOn).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetApplicationsCountByUsername(string username)
        {
            return this.partyApplicantsRepository.All().Count(x => x.Applicant.UserName == username);
        }
    }
}