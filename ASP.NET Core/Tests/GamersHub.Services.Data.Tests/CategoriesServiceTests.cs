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
            this.categoriesService = new CategoriesService(this.categoriesRepository, this.forumCategoriesRepository,
                this.postRepository, this.repliesRepository);
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
        public async Task TestGetAllMissingByForumId()
        {
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "wrong",
                CategoryForums = new List<ForumCategory>
                    {new ForumCategory {ForumId = 1}, new ForumCategory {ForumId = 2}},
            });
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "wrong",
                CategoryForums = new List<ForumCategory>
                {
                    new ForumCategory {ForumId = 1}, new ForumCategory {ForumId = 2}, new ForumCategory {ForumId = 3},
                    new ForumCategory {ForumId = 4},
                },
            });
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "wrong",
                CategoryForums = new List<ForumCategory>
                    {new ForumCategory {ForumId = 1}, new ForumCategory {ForumId = 4}},
            });
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "category",
                CategoryForums = new List<ForumCategory>
                    {new ForumCategory {ForumId = 2}, new ForumCategory {ForumId = 3}},
            });
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "category",
                CategoryForums = new List<ForumCategory>
                    {new ForumCategory {ForumId = 2}, new ForumCategory {ForumId = 3}},
            });
            await this.categoriesRepository.SaveChangesAsync();

            var categories = this.categoriesService.GetAllMissingByForumId<TestCategory>(1).ToList();

            Assert.AreEqual(2, categories.Count);

            foreach (var category in categories)
            {
                Assert.AreEqual("category", category.Name);
            }
        }

        [Test]
        public async Task TestGetById()
        {
            await this.categoriesRepository.AddAsync(new Category {Name = "wrong"});
            await this.categoriesRepository.AddAsync(new Category {Name = "wrong"});
            await this.categoriesRepository.AddAsync(new Category {Name = "category"});
            await this.categoriesRepository.AddAsync(new Category {Name = "wrong"});
            await this.categoriesRepository.SaveChangesAsync();

            var category = this.categoriesService.GetById<TestCategory>(3);

            Assert.AreEqual("category", category.Name);
        }

        [Test]
        public async Task TestCreateAsyncReturnsZeroWhenCategoryNameAlreadyExists()
        {
            await this.categoriesRepository.AddAsync(new Category {Name = "category"});
            await this.categoriesRepository.SaveChangesAsync();

            var categoryId = await this.categoriesService.CreateAsync("category", "description");

            Assert.Zero(categoryId);
        }

        [Test]
        public async Task TestCreateAsyncWorksCorrectly()
        {
            var categoryId = await this.categoriesService.CreateAsync("category", "description");

            var category = await this.categoriesRepository.All().FirstAsync();

            Assert.AreEqual(1, categoryId);
            Assert.AreEqual("category", category.Name);
            Assert.AreEqual("description", category.Description);
        }

        [Test]
        public async Task TestEditAsyncReturnsNullIfCategoryDoesNotExist()
        {
            var categoryId = await this.categoriesService.EditAsync(1, "name", "description", null, null);

            Assert.Null(categoryId);
        }

        [Test]
        public async Task TestEditAsyncReturnsNullIfCategoryNameIsAlreadyTaken()
        {
            await this.categoriesRepository.AddAsync(new Category {Name = "category"});
            await this.categoriesRepository.AddAsync(new Category {Name = "name"});
            await this.categoriesRepository.SaveChangesAsync();

            var categoryId = await this.categoriesService.EditAsync(2, "category", "description", null, null);

            Assert.Zero((int) categoryId);
        }

        [Test]
        public async Task TestEditAsyncWorksCorrectlyAndAlsoAddsForumsToTheCategory()
        {
            await this.categoriesRepository.AddAsync(new Category {Name = "name", Description = "description"});
            await this.categoriesRepository.SaveChangesAsync();

            var categoryId =
                await this.categoriesService.EditAsync(1, "category", "test", new[] {1, 2, 3},
                    new[] {true, true, false});

            var category = await this.categoriesRepository.All().FirstAsync();

            Assert.AreEqual(1, categoryId);
            Assert.AreEqual("category", category.Name);
            Assert.AreEqual("test", category.Description);

            var categoryForums = category.CategoryForums.ToArray();

            Assert.AreEqual(2, categoryForums.Length);

            Assert.AreEqual(1, categoryForums[0].ForumId);
            Assert.AreEqual(2, categoryForums[1].ForumId);
        }

        [Test]
        public async Task TestDeleteAsyncReturnsNullWithInvalidId()
        {
            var categoryId = await this.categoriesService.DeleteAsync(1);

            Assert.Null(categoryId);
        }

        [Test]
        public async Task TestDeleteAsyncWorksCorrectly()
        {
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = "category",
                CategoryForums = new List<ForumCategory> {new ForumCategory()},
                Posts = new List<Post> {new Post {Replies = new List<Reply> {new Reply(), new Reply()}}},
            });

            await this.categoriesRepository.SaveChangesAsync();

            var categoryId = await this.categoriesService.DeleteAsync(1);

            var category = this.categoriesRepository.AllWithDeleted().First();

            Assert.AreEqual(1, categoryId);
            Assert.IsTrue(category.IsDeleted);
            Assert.IsEmpty(category.CategoryForums);

            foreach (var categoryPost in category.Posts)
            {
                foreach (var reply in categoryPost.Replies)
                {
                    Assert.IsTrue(reply.IsDeleted);
                }

                Assert.IsTrue(category.IsDeleted);
            }
        }

        [Test]
        public async Task TestGetCountWorksCorrectly()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.categoriesRepository.AddAsync(new Category());
            }

            await this.categoriesRepository.SaveChangesAsync();

            var count = this.categoriesService.GetCount();

            Assert.AreEqual(5, count);

        }

        [Test]
        public async Task TestGetNormalisedNameWorksCorrectly()
        {
            await this.categoriesRepository.AddAsync(new Category {Name = "General Discussion"});
            await this.categoriesRepository.SaveChangesAsync();

            var categoryNameUrl = "General-Discussion";

            var normalisedName = this.categoriesService.GetNormalisedName(categoryNameUrl);

            Assert.AreEqual("General Discussion", normalisedName);
        }
    }

    public class TestCategory : IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}