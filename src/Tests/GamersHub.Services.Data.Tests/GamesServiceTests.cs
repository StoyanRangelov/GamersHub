namespace GamersHub.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Data;
    using GamersHub.Data.Models;
    using GamersHub.Data.Repositories;
    using GamersHub.Services.Data.Games;
    using GamersHub.Services.Data.Tests.TestModels;
    using GamersHub.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    [TestFixture]
    public class GamesServiceTests
    {
        private EfDeletableEntityRepository<Game> gameRepository;
        private EfDeletableEntityRepository<Review> reviewsRepository;
        private GamesService gamesService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.gameRepository = new EfDeletableEntityRepository<Game>(new ApplicationDbContext(options.Options));
            this.reviewsRepository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            this.gamesService = new GamesService(this.gameRepository, this.reviewsRepository);
            AutoMapperConfig.RegisterMappings(typeof(TestGame).Assembly);
        }

        [Test]
        public async Task TestGetTitleUrlAndSubTitleByIdWorksCorrectly()
        {
            await this.gameRepository.AddAsync(new Game { Title = "World of Warcraft", SubTitle = "The Burning Crusade" });
            await this.gameRepository.SaveChangesAsync();

            var routeParams = this.gamesService.GetTitleUrlAndSubTitleById(1);
            var gameUrl = routeParams[0];
            var subTitle = routeParams[1];

            Assert.AreEqual("World-of-Warcraft", gameUrl);
            Assert.AreEqual("The Burning Crusade", subTitle);
        }

        [Test]
        public async Task TestGetByIdWorksCorrectly()
        {
            await this.gameRepository.AddAsync(new Game { Title = "test" });
            await this.gameRepository.AddAsync(new Game { Title = "Dota 2" });
            await this.gameRepository.AddAsync(new Game { Title = "test" });
            await this.gameRepository.SaveChangesAsync();

            var game = this.gamesService.GetById<TestGame>(2);

            Assert.AreEqual("Dota 2", game.Title);
        }

        [Test]
        public async Task TestGetByNameUrlWorksCorrectly()
        {
            await this.gameRepository.AddAsync(new Game
                { Title = "World of Warcraft", SubTitle = "The Burning Crusade" });
            await this.gameRepository.SaveChangesAsync();

            var titleUrl = "World-of-Warcraft";

            var game = this.gamesService.GetByNameUrl<TestGame>(titleUrl, "The Burning Crusade");

            Assert.AreEqual("World of Warcraft", game.Title);
            Assert.AreEqual("The Burning Crusade", game.SubTitle);
        }

        [Test]
        public async Task TestGetAllWorksCorrectly([Values("test", null)]string value)
        {
            for (int i = 0; i < 5; i++)
            {
                await this.gameRepository.AddAsync(new Game { Title = "test" });
            }

            await this.gameRepository.SaveChangesAsync();

            var games = this.gamesService.GetAll<TestGame>(null, 0, value).ToList();

            Assert.AreEqual(5, games.Count);

            foreach (var testGame in games)
            {
                Assert.AreEqual("test", testGame.Title);
            }
        }

        [Test]
        public async Task TestGetAllWithSkipAndTakeValues()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.gameRepository.AddAsync(new Game { Title = "skip" });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.gameRepository.AddAsync(new Game { Title = "test" });
            }

            await this.gameRepository.AddAsync(new Game { Title = "fail" });
            await this.gameRepository.AddAsync(new Game { Title = "fail" });
            await this.gameRepository.SaveChangesAsync();

            var games = this.gamesService.GetAll<TestGame>(5, 3).ToList();

            Assert.AreEqual(5, games.Count);

            foreach (var testGame in games)
            {
                Assert.AreEqual("test", testGame.Title);
            }
        }

        [Test]
        public async Task TestCreateAsyncWorksCorrectly()
        {
            var gameId = await this.gamesService.CreateAsync(
                "title", "subTitle", "description", "img");

            var game = await this.gameRepository.All().FirstAsync();

            Assert.AreEqual(1, gameId);
            Assert.AreEqual("title", game.Title);
            Assert.AreEqual("subTitle", game.SubTitle);
            Assert.AreEqual("description", game.Description);
            Assert.AreEqual("img", game.ImgUrl);
        }

        [Test]
        public async Task TestEditAsyncReturnsNullWhenInvalidGameIdIsGiven()
        {
            var gameId = await this.gamesService.EditAsync(1, "test", "test", "test", "test");

            Assert.Null(gameId);
        }

        [Test]
        public async Task TestEditAsyncWorksCorrectly()
        {
            await this.gameRepository.AddAsync(new Game
            {
                Title = "title",
                SubTitle = "subTitle",
                Description = "description",
                ImgUrl = "image",
            });
            await this.gameRepository.SaveChangesAsync();

            var gameId = await this.gamesService.EditAsync(1, "test", "test", "test", "test");

            var game = await this.gameRepository.All().FirstAsync();

            Assert.AreEqual(1, gameId);
            Assert.AreEqual("test", game.Title);
            Assert.AreEqual("test", game.SubTitle);
            Assert.AreEqual("test", game.Description);
            Assert.AreEqual("test", game.ImgUrl);
        }

        [Test]
        public async Task TestDeleteAsyncReturnsNullWithInvalidId()
        {
            var gameId = await this.gamesService.DeleteAsync(1);

            Assert.Null(gameId);
        }

        [Test]
        public async Task TestDeleteAsyncWorksCorrectly()
        {
            await this.gameRepository.AddAsync(new Game
            {
                Reviews = new List<Review> { new Review(), new Review(), new Review() },
            });
            await this.gameRepository.SaveChangesAsync();

            var gameId = await this.gamesService.DeleteAsync(1);

            var game = await this.gameRepository.AllWithDeleted().FirstAsync();

            Assert.AreEqual(1, gameId);
            Assert.IsTrue(game.IsDeleted);

            foreach (var gameReview in game.Reviews)
            {
                Assert.IsTrue(gameReview.IsDeleted);
            }
        }

        [Test]
        public async Task TestGetCount([Values("test", null)]string value)
        {
            for (int i = 0; i < 5; i++)
            {
                await this.gameRepository.AddAsync(new Game { Title = "test", SubTitle = "title" });
            }

            await this.gameRepository.SaveChangesAsync();

            var count = this.gamesService.GetCount(value);

            Assert.AreEqual(5, count);
        }
    }
}
