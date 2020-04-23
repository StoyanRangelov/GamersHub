namespace GamersHub.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using GamersHub.Data;
    using GamersHub.Data.Models;
    using GamersHub.Data.Repositories;
    using GamersHub.Services.Data.Pages;
    using GamersHub.Services.Data.Tests.TestModels;
    using GamersHub.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    [TestFixture]
    public class PagesServiceTests
    {
        private EfDeletableEntityRepository<Page> repository;
        private PagesService service;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfDeletableEntityRepository<Page>(new ApplicationDbContext(options.Options));
            this.service = new PagesService(this.repository);
            AutoMapperConfig.RegisterMappings(typeof(TestPage).Assembly);
        }

        [Test]
        public async Task TestGetByName()
        {
            await this.repository.AddAsync(new Page { Name = "test", Content = "test" });
            await this.repository.SaveChangesAsync();

            var page = this.service.GetByName<TestPage>("test");

            Assert.AreEqual("test", page.Name);
            Assert.AreEqual("test", page.Content);
        }

        [Test]
        public async Task TestEditAsyncReturnsNullWithInvalidId()
        {
            var pageId = await this.service.EditAsync("test", "test");

            Assert.Null(pageId);
        }

        [Test]
        public async Task TestEditAsyncWorksCorrectly()
        {
            await this.repository.AddAsync(new Page { Name = "test", Content = "content" });
            await this.repository.SaveChangesAsync();

            var pageId = await this.service.EditAsync("test", "test");

            var page = await this.repository.All().FirstAsync();

            Assert.AreEqual(1, pageId);
            Assert.AreEqual("test", page.Content);
        }
    }
}
