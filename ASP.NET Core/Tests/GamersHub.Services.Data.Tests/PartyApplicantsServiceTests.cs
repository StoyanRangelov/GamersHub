namespace GamersHub.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Data;
    using GamersHub.Data.Models;
    using GamersHub.Data.Repositories;
    using GamersHub.Services.Data.PartyApplicants;
    using GamersHub.Services.Data.Tests.TestModels;
    using GamersHub.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    [TestFixture]
    public class PartyApplicantsServiceTests
    {
        private EfRepository<PartyApplicant> partyApplicantsRepository;
        private EfDeletableEntityRepository<Party> partiesRepository;
        private PartyApplicantsService partyApplicantsService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.partyApplicantsRepository = new EfRepository<PartyApplicant>(new ApplicationDbContext(options.Options));
            this.partiesRepository = new EfDeletableEntityRepository<Party>(new ApplicationDbContext(options.Options));
            this.partyApplicantsService = new PartyApplicantsService(this.partyApplicantsRepository, this.partiesRepository);
            AutoMapperConfig.RegisterMappings(typeof(TestPartyApplicant).Assembly);
        }

        [Test]
        public async Task TestApproveAsyncReturnsNullIfPartyOrApplicantDoesNotExist()
        {
            var partyId = await this.partyApplicantsService.ApproveAsync(1, "id");

            Assert.Null(partyId);
        }

        [Test]
        public async Task TestApproveAsyncWhenPartyWontBecomeFull()
        {
            await this.partiesRepository.AddAsync(new Party
            {
                Size = 5,
                PartyApplicants = new List<PartyApplicant>
                {
                    new PartyApplicant
                {
                    PartyId = 1,
                    ApplicantId = "id",
                    ApplicationStatus = ApplicationStatusType.Pending,
                },
                },
            });
            await this.partiesRepository.SaveChangesAsync();

            var partyId = await this.partyApplicantsService.ApproveAsync(1, "id");

            var partyApplication = await this.partyApplicantsRepository.All().FirstAsync();

            Assert.AreEqual(1, partyId);
            Assert.AreEqual(2, (int)partyApplication.ApplicationStatus);
        }

        [Test]
        public async Task TestApproveAsyncWhenPartyWillBecomeFull()
        {
            await this.partiesRepository.AddAsync(new Party
            {
                Size = 1,
                PartyApplicants = new List<PartyApplicant>
                {
                    new PartyApplicant
                    {
                        PartyId = 1,
                        ApplicantId = "id",
                        ApplicationStatus = ApplicationStatusType.Pending,
                    },
                },
            });
            await this.partiesRepository.SaveChangesAsync();

            var partyId = await this.partyApplicantsService.ApproveAsync(1, "id");

            var partyApplication = await this.partyApplicantsRepository.All().FirstAsync();

            var party = await this.partiesRepository.All().FirstAsync();

            Assert.AreEqual(1, partyId);
            Assert.AreEqual(2, (int)partyApplication.ApplicationStatus);
            Assert.IsTrue(party.IsFull);
        }

        [Test]
        public async Task TestDeclineAsyncReturnsNullWhenPartyOrApplicantDoesNotExist()
        {
            var partyId = await this.partyApplicantsService.DeclineAsync(1, "id");

            Assert.Null(partyId);
        }

        [Test]
        public async Task TestDeclineAsyncWorksCorrectly()
        {
            await this.partyApplicantsRepository.AddAsync(new PartyApplicant
            {
                ApplicantId = "id",
                ApplicationStatus = ApplicationStatusType.Pending,
            });
            await this.partyApplicantsRepository.SaveChangesAsync();

            var partyId = await this.partyApplicantsService.DeclineAsync(1, "id");

            var partyApplication = await this.partyApplicantsRepository.All().FirstAsync();

            Assert.AreEqual(1, partyId);
            Assert.AreEqual(1, (int)partyApplication.ApplicationStatus);
        }

        [Test]
        public async Task TestCancelApplicationAsyncReturnsNullIfApplicationDoesNotExist()
        {
            var partyId = await this.partyApplicantsService.CancelApplicationAsync(1, "id");

            Assert.Null(partyId);
        }

        [Test]
        public async Task TestCancelApplicationAsyncWithPendingApplicationAndNotFullParty()
        {
            await this.partiesRepository.AddAsync(new Party
            {
                Size = 5,
                PartyApplicants = new List<PartyApplicant>
                {
                    new PartyApplicant
                    {
                        PartyId = 1,
                        ApplicantId = "id",
                        ApplicationStatus = ApplicationStatusType.Pending,
                    },
                },
            });
            await this.partiesRepository.SaveChangesAsync();

            var partyId = await this.partyApplicantsService.CancelApplicationAsync(1, "id");

            var partyApplications = this.partyApplicantsRepository.All().ToList();

            Assert.AreEqual(1, partyId);
            Assert.IsEmpty(partyApplications);
        }

        [Test]
        public async Task TestCancelApplicationWithApprovedApplicationAndFullParty()
        {
            await this.partiesRepository.AddAsync(new Party
            {
                Size = 1,
                IsFull = true,
                PartyApplicants = new List<PartyApplicant>
                {
                    new PartyApplicant
                    {
                        PartyId = 1,
                        ApplicantId = "id",
                        ApplicationStatus = ApplicationStatusType.Approved,
                    },
                },
            });
            await this.partiesRepository.SaveChangesAsync();

            var partyId = await this.partyApplicantsService.CancelApplicationAsync(1, "id");

            var party = await this.partiesRepository.All().FirstAsync();
            var partyApplications = this.partyApplicantsRepository.All().ToList();

            Assert.AreEqual(1, partyId);
            Assert.IsFalse(party.IsFull);
            Assert.IsEmpty(partyApplications);
        }

        [Test]
        public async Task TestGetAllApplicationsByUsername()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.partyApplicantsRepository.AddAsync(new PartyApplicant
                {
                    Party = new Party { Game = "fail" },
                    Applicant = new ApplicationUser { UserName = "test", CreatedOn = new DateTime(2020, 4, 17) },
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.partyApplicantsRepository.AddAsync(new PartyApplicant
                {
                    Party = new Party { Game = "test", CreatedOn = new DateTime(2020, 4, 16) },
                    Applicant = new ApplicationUser { UserName = "test" },
                });
            }

            await this.partyApplicantsRepository.AddAsync(new PartyApplicant
            {
                Applicant = new ApplicationUser { UserName = "fail" },
            });
            await this.partyApplicantsRepository.AddAsync(new PartyApplicant
            {
                Applicant = new ApplicationUser { UserName = "fail" },
            });
            await this.partyApplicantsRepository.SaveChangesAsync();

            var partyApplicants = this.partyApplicantsService
                .GetAllApplicationsByUsername<TestPartyApplicant>("test", 5, 3).ToList();

            Assert.AreEqual(5, partyApplicants.Count);

            foreach (var partyApplicantTest in partyApplicants)
            {
                Assert.AreEqual("test", partyApplicantTest.ApplicantUsername);
                Assert.AreEqual("test", partyApplicantTest.PartyGame);
            }
        }

        [Test]
        public async Task TestGetApplicationsCountByUsername()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.partyApplicantsRepository.AddAsync(new PartyApplicant
                {
                    Applicant = new ApplicationUser { UserName = "test" },
                });
            }

            await this.partyApplicantsRepository.AddAsync(new PartyApplicant
            {
                Applicant = new ApplicationUser { UserName = "fail" },
            });
            await this.partyApplicantsRepository.AddAsync(new PartyApplicant
            {
                Applicant = new ApplicationUser { UserName = "fail" },
            });
            await this.partyApplicantsRepository.SaveChangesAsync();

            var count = this.partyApplicantsService.GetApplicationsCountByUsername("test");

            Assert.AreEqual(5, count);
        }
    }
}
