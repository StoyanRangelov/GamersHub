namespace GamersHub.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Data;
    using GamersHub.Data.Models;
    using GamersHub.Data.Repositories;
    using GamersHub.Services.Data.Parties;
    using GamersHub.Services.Data.Tests.TestModels;
    using GamersHub.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    [TestFixture]
    public class PartiesServiceTests
    {
        private EfDeletableEntityRepository<Party> partiesRepository;
        private EfRepository<PartyApplicant> partyApplicantsRepository;
        private PartiesService partiesService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.partiesRepository = new EfDeletableEntityRepository<Party>(new ApplicationDbContext(options.Options));
            this.partyApplicantsRepository = new EfRepository<PartyApplicant>(new ApplicationDbContext(options.Options));
            this.partiesService = new PartiesService(this.partiesRepository, this.partyApplicantsRepository);
            AutoMapperConfig.RegisterMappings(typeof(TestParty).Assembly);
        }

        [Test]
        public async Task TestGetById()
        {
            await this.partiesRepository.AddAsync(new Party
            {
                Game = "fail", Creator = new ApplicationUser { UserName = "fail" },
            });
            await this.partiesRepository.AddAsync(new Party
            {
                Game = "test", Creator = new ApplicationUser { UserName = "test" },
            });
            await this.partiesRepository.AddAsync(new Party
            {
                Game = "fail", Creator = new ApplicationUser { UserName = "fail" },
            });
            await this.partiesRepository.SaveChangesAsync();

            var party = this.partiesService.GetById<TestParty>(2);

            Assert.AreEqual("test", party.Game);
            Assert.AreEqual("test", party.CreatorUsername);
        }

        [Test]
        public async Task TestGetAll([Values("test", null)]string value)
        {
            for (int i = 0; i < 5; i++)
            {
                await this.partiesRepository.AddAsync(new Party
                {
                    Game = "test",
                    Creator = new ApplicationUser { UserName = "test" },
                });
            }

            await this.partiesRepository.SaveChangesAsync();

            var parties = this.partiesService.GetAll<TestParty>(null, 0, value).ToList();

            Assert.AreEqual(5, parties.Count);

            foreach (var partyTest in parties)
            {
                Assert.AreEqual("test", partyTest.CreatorUsername);
                Assert.AreEqual("test", partyTest.Game);
            }
        }

        [Test]
        public async Task TestGetAllWithSkipAndTakeValues()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.partiesRepository.AddAsync(new Party
                {
                    Game = "skip", CreatedOn = new DateTime(2020, 4, 17),
                    Creator = new ApplicationUser { UserName = "skip" },
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.partiesRepository.AddAsync(new Party
                {
                    Game = "test", CreatedOn = new DateTime(2020, 4, 16),
                    Creator = new ApplicationUser { UserName = "test" },
                });
            }

            await this.partiesRepository.AddAsync(new Party
            {
                Game = "fail", CreatedOn = new DateTime(2020, 4, 15),
                Creator = new ApplicationUser { UserName = "fail" },
            });
            await this.partiesRepository.SaveChangesAsync();

            var parties = this.partiesService.GetAll<TestParty>(5, 3).ToList();

            Assert.AreEqual(5, parties.Count);

            foreach (var partyTest in parties)
            {
                Assert.AreEqual("test", partyTest.CreatorUsername);
                Assert.AreEqual("test", partyTest.Game);
            }
        }

        [Test]
        public async Task GetTopFive()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.partiesRepository.AddAsync(new Party
                {
                    Game = "test", PartyApplicants = new List<PartyApplicant>
                    {
                        new PartyApplicant { ApplicantId = "firstId" },
                        new PartyApplicant { ApplicantId = "secondId" },
                        new PartyApplicant { ApplicantId = "thirdId" },
                    },
                });
            }

            await this.partiesRepository.AddAsync(new Party
            {
                Game = "fail", PartyApplicants = new List<PartyApplicant>
                {
                    new PartyApplicant { ApplicantId = "fourthId" },
                    new PartyApplicant { ApplicantId = "fifthId" },
                },
            });
            await this.partiesRepository.AddAsync(new Party
            {
                Game = "fail", PartyApplicants = new List<PartyApplicant>
                {
                    new PartyApplicant { ApplicantId = "sixthId" },
                },
            });
            await this.partiesRepository.SaveChangesAsync();

            var parties = this.partiesService.GetTopFive<TestParty>().ToList();

            foreach (var partyTest in parties)
            {
                Assert.AreEqual("test", partyTest.Game);
            }
        }

        [Test]
        public async Task TestGetAllByUsername()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.partiesRepository.AddAsync(new Party
                {
                    Game = "skip", CreatedOn = new DateTime(2020, 4, 17),
                    Creator = new ApplicationUser { UserName = "test" },
                });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.partiesRepository.AddAsync(new Party
                {
                    Game = "test", CreatedOn = new DateTime(2020, 4, 16),
                    Creator = new ApplicationUser { UserName = "test" },
                });
            }

            await this.partiesRepository.AddAsync(new Party
            {
                Game = "fail", CreatedOn = new DateTime(2020, 4, 15),
                Creator = new ApplicationUser { UserName = "fail" },
            });
            await this.partiesRepository.AddAsync(new Party
            {
                Game = "fail", CreatedOn = new DateTime(2020, 4, 15),
                Creator = new ApplicationUser { UserName = "fail" },
            });
            await this.partiesRepository.SaveChangesAsync();

            var parties = this.partiesService.GetAllByUsername<TestParty>("test", 5, 3).ToList();

            Assert.AreEqual(5, parties.Count);

            foreach (var partyTest in parties)
            {
                Assert.AreEqual("test", partyTest.CreatorUsername);
                Assert.AreEqual("test", partyTest.Game);
            }
        }

        [Test]
        public async Task TestGetCount([Values("test", null)]string value)
        {
            for (int i = 0; i < 5; i++)
            {
                await this.partiesRepository.AddAsync(new Party { Game = "test", Creator = new ApplicationUser { UserName = "name" } });
            }

            await this.partiesRepository.SaveChangesAsync();

            var count = this.partiesService.GetCount(value);

            Assert.AreEqual(5, count);
        }

        [Test]
        public async Task TestGetCountByUsername()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.partiesRepository.AddAsync(new Party
                {
                    Creator = new ApplicationUser { UserName = "test" },
                });
            }

            await this.partiesRepository.AddAsync(new Party
            {
                Creator = new ApplicationUser { UserName = "fail" },
            });
            await this.partiesRepository.AddAsync(new Party
            {
                Creator = new ApplicationUser { UserName = "fail" },
            });
            await this.partiesRepository.SaveChangesAsync();

            var count = this.partiesService.GetCountByUsername("test");

            Assert.AreEqual(5, count);
        }

        [Test]
        public async Task TestCreateAsync()
        {
            var partyId = await this.partiesService.CreateAsync(
                "userId", "game", ActivityType.Dungeon, "description", 1);

            var party = await this.partiesRepository.All().FirstAsync();

            Assert.AreEqual(1, partyId);
            Assert.AreEqual("userId", party.CreatorId);
            Assert.AreEqual("game", party.Game);
            Assert.AreEqual(ActivityType.Dungeon, party.Activity);
            Assert.AreEqual("description", party.Description);
            Assert.AreEqual(1, party.Size);
        }

        [Test]
        public async Task TestApplyAsyncReturnsNullIfPartyDoesntExist()
        {
            var partyId = await this.partiesService.ApplyAsync(1, "userId");

            Assert.Null(partyId);
        }

        [Test]
        public async Task TestApplyAsyncReturnsZeroIfApplicationAlreadyExists()
        {
            await this.partiesRepository.AddAsync(new Party
            {
                PartyApplicants = new List<PartyApplicant> { new PartyApplicant { PartyId = 1, ApplicantId = "userId" } },
            });
            await this.partiesRepository.SaveChangesAsync();

            var partyId = await this.partiesService.ApplyAsync(1, "userId");

            Assert.Zero((int)partyId);
        }

        [Test]
        public async Task TestApplyAsyncAddsApplicationCorrectly()
        {
            await this.partiesRepository.AddAsync(new Party());
            await this.partiesRepository.SaveChangesAsync();

            var partyId = await this.partiesService.ApplyAsync(1, "userId");

            var party = await this.partiesRepository.All().FirstAsync();

            var applications = party.PartyApplicants;

            Assert.AreEqual(1, partyId);
            Assert.AreEqual(1, applications.Count);

            foreach (var partyApplicant in applications)
            {
                Assert.AreEqual(1, partyApplicant.PartyId);
                Assert.AreEqual("userId", partyApplicant.ApplicantId);
            }
        }

        [Test]
        public async Task TestEditAsyncReturnsNullIfPartyDoesNotExist()
        {
            var partyId = await this.partiesService.EditAsync(1, "test", ActivityType.Other, "test");

            Assert.Null(partyId);
        }

        [Test]
        public async Task TestEditAsyncWorksCorrectly()
        {
            await this.partiesRepository.AddAsync(new Party
            {
                Game = "game",
                Activity = ActivityType.Dungeon,
                Description = "description",
            });
            await this.partiesRepository.SaveChangesAsync();

            var partyId = await this.partiesService.EditAsync(1, "test", ActivityType.Exploration, "test");

            var party = await this.partiesRepository.All().FirstAsync();

            Assert.AreEqual(1, partyId);
            Assert.AreEqual("test", party.Game);
            Assert.AreEqual(ActivityType.Exploration, party.Activity);
            Assert.AreEqual("test", party.Description);
        }

        [Test]
        public async Task TestDeleteAsyncReturnsNullWithInvalidId()
        {
            var partyId = await this.partiesService.DeleteAsync(1);

            Assert.Null(partyId);
        }

        [Test]
        public async Task TestDeleteAsyncWorksCorrectly()
        {
            await this.partiesRepository.AddAsync(new Party
            {
                PartyApplicants = new List<PartyApplicant>
                {
                    new PartyApplicant { PartyId = 1, ApplicantId = "firstId" },
                    new PartyApplicant { PartyId = 1, ApplicantId = "secondId" },
                    new PartyApplicant { PartyId = 1, ApplicantId = "thirdId" },
                },
            });
            await this.partiesRepository.SaveChangesAsync();

            var partyId = await this.partiesService.DeleteAsync(1);

            var party = await this.partiesRepository.AllWithDeleted().FirstAsync();

            Assert.AreEqual(1, partyId);
            Assert.IsTrue(party.IsDeleted);
            Assert.IsEmpty(party.PartyApplicants);
        }
    }
}
