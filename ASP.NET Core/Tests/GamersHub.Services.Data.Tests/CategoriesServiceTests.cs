using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GamersHub.Services.Data.Tests
{
    [TestFixture]
    public class CategoriesServiceTests
    {
        private EfDeletableEntityRepository<Category> categoriesRepository;
        private EfRepository<ForumCategory> forumCategoriesRepository;
        private EfDeletableEntityRepository<Post> postRepository;
        private EfDeletableEntityRepository<Reply> repliesRepository;
        private CategoriesService categoriesService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.categoriesRepository =
                new EfDeletableEntityRepository<Category>(new ApplicationDbContext(options.Options));
            this.forumCategoriesRepository = new EfRepository<ForumCategory>(new ApplicationDbContext(options.Options));
            this.postRepository = new EfDeletableEntityRepository<Post>(new ApplicationDbContext(options.Options));
            this.repliesRepository = new EfDeletableEntityRepository<Reply>(new ApplicationDbContext(options.Options));
            this.categoriesService = new CategoriesService(this.categoriesRepository, this.forumCategoriesRepository, this.postRepository, this.repliesRepository);
            AutoMapperConfig.RegisterMappings(typeof(TestCategory).Assembly);
        }

        [Test]
        public async Task TestGetAll()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.categoriesRepository.AddAsync(new Category {Name = "skip"});
            }

            for (int i = 0; i < 5; i++)
            {
                await this.categoriesRepository.AddAsync(new Category {Name = "category"});
            }

            await this.categoriesRepository.AddAsync(new Category {Name = "wrong category"});
            await this.categoriesRepository.AddAsync(new Category {Name = "wrong category"});
            await this.categoriesRepository.SaveChangesAsync();

            var categories = this.categoriesService.GetAll<TestCategory>(5, 3).ToList();

            Assert.AreEqual(5, categories.Count);

            foreach (var testCategory in categories)
            {
                Assert.AreEqual("category", testCategory.Name);
            }
        }

        [Test]
        public async Task TestGetAllMissingByCategoryId()
        {
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "wrong",
                CategoryForums = new List<ForumCategory> { new ForumCategory { ForumId = 1 }, new ForumCategory { ForumId = 2 } },
            });
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "wrong",
                CategoryForums = new List<ForumCategory> { new ForumCategory { ForumId = 1 }, new ForumCategory { ForumId = 2 }, new ForumCategory { ForumId = 3 }, new ForumCategory { ForumId = 4 },  },
            });
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "wrong",
                CategoryForums = new List<ForumCategory> { new ForumCategory { ForumId = 1 }, new ForumCategory { ForumId = 4 } },
            });
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "category",
                CategoryForums = new List<ForumCategory> { new ForumCategory { ForumId = 2 }, new ForumCategory { ForumId = 3 } },
            });
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "category",
                CategoryForums = new List<ForumCategory> { new ForumCategory { ForumId = 2 }, new ForumCategory { ForumId = 3 } },
            });
            await this.categoriesRepository.SaveChangesAsync();

            var categories = this.categoriesService.GetAllMissingByForumId<TestCategory>(1).ToList();

            Assert.AreEqual(2, categories.Count);

            foreach (var category in categories)
            {
                Assert.AreEqual("category", category.Name);
            }
        }
    }

    public class TestCategory : IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}