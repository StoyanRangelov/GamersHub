using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Data.Games;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GamersHub.Services.Data.Tests
{
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
        public async Task TestGetUrlWorksCorrectly()
        {
            await this.gameRepository.AddAsync(new Game { Title = "Dota 2"});
            await this.gameRepository.SaveChangesAsync();

            var gameUrl = this.gamesService.GetUrlById(1);

            Assert.AreEqual("Dota-2", gameUrl);
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
            await this.gameRepository.AddAsync(new Game { Title = "World of Warcraft: The Burning Crusade"});
            await this.gameRepository.SaveChangesAsync();

            var titleUrl = "World-of-Warcraft-The-Burning-Crusade";

            var game = this.gamesService.GetByNameUrl<TestGame>(titleUrl);

            Assert.AreEqual("World of Warcraft: The Burning Crusade", game.Title);
        }

        [Test]
        public async Task TestGetAllWorksCorrectly()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.gameRepository.AddAsync(new Game { Title = "skip" });
            }

            for (int i = 0; i < 5; i++)
            {
                await this.gameRepository.AddAsync(new Game {Title = "test"});
            }

            await this.gameRepository.AddAsync(new Game {Title = "fail"});
            await this.gameRepository.AddAsync(new Game {Title = "fail"});
            await this.gameRepository.SaveChangesAsync();

            var games = this.gamesService.GetAll<TestGame>(5,3).ToList();

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
        public async Task TestEditAsyncReturnsZeroWhenInvalidGameIdIsGiven()
        {
            var gameId = await this.gamesService.EditAsync(1, "test", "test", "test", "test");

            Assert.AreEqual(0, gameId);
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
        public async Task TestDeleteAsyncWorksCorrectly()
        {
            await this.gameRepository.AddAsync(new Game
            {
                Reviews = new List<Review> {new Review(), new Review(), new Review()},
            });
            await this.gameRepository.SaveChangesAsync();

            await this.gamesService.DeleteAsync(1);

            var game = await this.gameRepository.AllWithDeleted().FirstAsync();

            Assert.IsTrue(game.IsDeleted);

            foreach (var gameReview in game.Reviews)
            {
                Assert.IsTrue(gameReview.IsDeleted);
            }
        }

        [Test]
        public async Task TestGetCount()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.gameRepository.AddAsync(new Game());
            }
            await this.gameRepository.SaveChangesAsync();

            var count = this.gamesService.GetCount();

            Assert.AreEqual(5, count);
        }
    }

    public class TestGame : IMapFrom<Game>
    {
        public string Title { get; set; }
    }
}