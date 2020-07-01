namespace GamersHub.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using GamersHub.Data;
    using GamersHub.Data.Models;
    using GamersHub.Data.Repositories;
    using GamersHub.Services.Data.ForumCategories;
    using GamersHub.Services.Data.Tests.TestModels;
    using GamersHub.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    [TestFixture]
    public class ForumCategoriesServiceTests
    {
        private EfRepository<ForumCategory> repository;
        private ForumCategoriesService forumCategoriesService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfRepository<ForumCategory>(new ApplicationDbContext(options.Options));
            this.forumCategoriesService = new ForumCategoriesService(this.repository);
            AutoMapperConfig.RegisterMappings(typeof(TestForumCategory).Assembly);
        }

        [Test]
        public async Task TestGetByNameAndForumId()
        {
            await this.repository.AddAsync(new ForumCategory
                { ForumId = 1, Category = new Category { Name = "category" } });

            for (int i = 2; i < 5; i++)
            {
                await this.repository.AddAsync(new ForumCategory
                    { ForumId = i, Category = new Category { Name = Guid.NewGuid().ToString() } });
            }

            await this.repository.SaveChangesAsync();

            var forumCategory = this.forumCategoriesService.GetByCategoryNameAndForumId<TestForumCategory>("category", 1);

            Assert.AreEqual(1, forumCategory.ForumId);
            Assert.AreEqual("category", forumCategory.CategoryName);
        }
    }
}
