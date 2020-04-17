using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Data.ForumCategories;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Moq;
using NUnit.Framework;

namespace GamersHub.Services.Data.Tests
{
    [TestFixture]
    public class ForumsServiceTests
    {
        private EfDeletableEntityRepository<Forum> forumsRepository;
        private EfDeletableEntityRepository<Post> postsRepository;
        private EfDeletableEntityRepository<Reply> repliesRepository;
        private EfRepository<ForumCategory> forumCategoriesRepository;
        private ForumsService forumsService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableDetailedErrors();
            this.forumsRepository = new EfDeletableEntityRepository<Forum>(new ApplicationDbContext(options.Options));
            this.postsRepository = new EfDeletableEntityRepository<Post>(new ApplicationDbContext(options.Options));
            this.repliesRepository = new EfDeletableEntityRepository<Reply>(new ApplicationDbContext(options.Options));
            this.forumCategoriesRepository = new EfRepository<ForumCategory>(new ApplicationDbContext(options.Options));
            this.forumsService = new ForumsService(this.forumsRepository, this.forumCategoriesRepository,
                this.postsRepository, this.repliesRepository);
            AutoMapperConfig.RegisterMappings(typeof(TestForum).Assembly);
        }

        [Test]
        public async Task TestGetAll()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.forumsRepository.AddAsync(new Forum {Name = "skip"});
            }

            for (int i = 0; i < 5; i++)
            {
                await this.forumsRepository.AddAsync(new Forum {Name = "forum"});
            }

            await this.forumsRepository.AddAsync(new Forum {Name = "fail"});
            await this.forumsRepository.AddAsync(new Forum {Name = "fail"});
            await this.forumsRepository.SaveChangesAsync();

            var forums = this.forumsService.GetAll<TestForum>(5, 3).ToList();

            foreach (var testForum in forums)
            {
                Assert.AreEqual("forum", testForum.Name);
            }

            Assert.AreEqual(5, forums.Count);
        }

        [Test]
        public async Task TestGetAllMissingByCategoryId()
        {
            await this.forumsRepository.AddAsync(new Forum
            {
                Name = "wrong",
                ForumCategories = new List<ForumCategory> { new ForumCategory { CategoryId = 1 }, new ForumCategory { CategoryId = 2 } },
            });
            await this.forumsRepository.AddAsync(new Forum
            {
                Name = "wrong",
                ForumCategories = new List<ForumCategory> { new ForumCategory { CategoryId = 1 }, new ForumCategory { CategoryId = 2 }, new ForumCategory { CategoryId = 3 }, new ForumCategory { CategoryId = 4 },  },
            });
            await this.forumsRepository.AddAsync(new Forum
            {
                Name = "wrong",
                ForumCategories = new List<ForumCategory> { new ForumCategory { CategoryId = 1 }, new ForumCategory { CategoryId = 4 } },
            });
            await this.forumsRepository.AddAsync(new Forum
            {
                Name = "forum",
                ForumCategories = new List<ForumCategory> { new ForumCategory { CategoryId = 2 }, new ForumCategory { CategoryId = 3 } },
            });
            await this.forumsRepository.AddAsync(new Forum
            {
                Name = "forum",
                ForumCategories = new List<ForumCategory> { new ForumCategory { CategoryId = 2 }, new ForumCategory { CategoryId = 3 } },
            });
            await this.forumsRepository.SaveChangesAsync();

            var forums = this.forumsService.GetAllMissingByCategoryId<TestForum>(1).ToList();

            Assert.AreEqual(2, forums.Count);

            foreach (var forum in forums)
            {
                Assert.AreEqual("forum", forum.Name);
            }
        }

        [Test]
        public async Task TestGetByNameUrl()
        {
            await this.forumsRepository.AddAsync(new Forum {Name = "World of Warcraft: The Frozen Throne"});
            await this.forumsRepository.SaveChangesAsync();

            var forumNameUrl = "World-of-Warcraft-The-Frozen-Throne";

            var forum = this.forumsService.GetByNameUrl<TestForum>(forumNameUrl);

            Assert.AreEqual("World of Warcraft: The Frozen Throne", forum.Name);
        }

        [Test]
        public async Task TestGetById()
        {
            await this.forumsRepository.AddAsync(new Forum {Name = "forum"});
            await this.forumsRepository.AddAsync(new Forum {Name = "forum 2"});
            await this.forumsRepository.AddAsync(new Forum {Name = "forum 3"});
            await this.forumsRepository.SaveChangesAsync();

            var forum = this.forumsService.GetById<TestForum>(1);

            Assert.AreEqual("forum", forum.Name);
        }

        [Test]
        public async Task TestCreateAsyncReturns0IfGivenForumNameIsAlreadyTaken()
        {
            await this.forumsRepository.AddAsync(new Forum {Name = "forum"});
            await this.forumsRepository.SaveChangesAsync();

            var forumId = await this.forumsService.CreateAsync("forum");

            Assert.Zero(forumId);
        }

        [Test]
        public async Task TestCreateAsyncWorksCorrectly()
        {
            var forumId = await this.forumsService.CreateAsync("forum");

            var forum = await this.forumsRepository.All().FirstAsync();

            Assert.AreEqual("forum", forum.Name);
        }

        [Test]
        public async Task TestEditAsyncReturnsMinusOneIfForumDoesNotExist()
        {
            var forumId = await this.forumsService.EditAsync(1, "forum", null, null);

            Assert.AreEqual(-1, forumId);
        }

        [Test]
        public async Task TestEditAsyncReturns0IfForumWithTheGivenNameAlreadyExists()
        {
            await this.forumsRepository.AddAsync(new Forum {Name = "forum"});
            await this.forumsRepository.AddAsync(new Forum {Name = "test forum"});
            await this.forumsRepository.SaveChangesAsync();

            var forumId = await this.forumsService.EditAsync(1, "test forum", null, null);

            Assert.Zero(forumId);
        }

        [Test]
        public async Task TestEditAsyncEditsForumNameCorrectly()
        {
            await this.forumsRepository.AddAsync(new Forum {Name = "forum"});
            await this.forumsRepository.SaveChangesAsync();

            var forumId = await this.forumsService.EditAsync(1, "edit", null, null);

            var forum = await this.forumsRepository.All().FirstAsync();

            Assert.AreEqual("edit", forum.Name);
        }

        [Test]
        public async Task TestEditAsyncAlsoAddsForumCategoriesToTheForum()
        {
            await this.forumsRepository.AddAsync(new Forum {Name = "forum"});
            await this.forumsRepository.SaveChangesAsync();

            var forumId = await this.forumsService.EditAsync(1, "edit", new[] {1, 2, 3}, new[] {true, false, false});

            var forum = await this.forumsRepository.All().FirstAsync();

            var forumCategories = forum.ForumCategories;

            Assert.AreEqual(1, forumCategories.Count);

            foreach (var forumCategory in forumCategories)
            {
                Assert.AreEqual(1, forumCategory.ForumId);
                Assert.AreEqual(1, forumCategory.CategoryId);
            }
        }

        [Test]
        public async Task TestDeleteAsyncWorksCorrectly()
        {
            await this.forumsRepository.AddAsync(new Forum
            {
                Name = "forum",
                ForumCategories = new List<ForumCategory> {new ForumCategory()},
                Posts = new List<Post> {new Post {Replies = new List<Reply> {new Reply(), new Reply()}}},
            });

            await this.forumsRepository.SaveChangesAsync();

            await this.forumsService.DeleteAsync(1);

            var forum = this.forumsRepository.AllWithDeleted().First();

            Assert.IsTrue(forum.IsDeleted);
            Assert.IsEmpty(forum.ForumCategories);

            foreach (var forumPost in forum.Posts)
            {
                foreach (var reply in forumPost.Replies)
                {
                    Assert.IsTrue(reply.IsDeleted);
                }

                Assert.IsTrue(forumPost.IsDeleted);
            }
        }

        [Test]
        public async Task TestGetCount()
        {
            for (int i = 0; i < 10; i++)
            {
                await this.forumsRepository.AddAsync(new Forum());
            }

            await this.forumsRepository.SaveChangesAsync();

            var count = this.forumsService.GetCount();

            Assert.AreEqual(10, count);
        }
    }

    public class TestForum : IMapFrom<Forum>
    {
        public string Name { get; set; }
    }
}