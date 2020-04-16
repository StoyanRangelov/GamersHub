using System;
using System.Threading.Tasks;
using GamersHub.Data;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Data.ForumCategories;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
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
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.forumsRepository = new EfDeletableEntityRepository<Forum>(new ApplicationDbContext(options.Options));
            this.postsRepository = new EfDeletableEntityRepository<Post>(new ApplicationDbContext(options.Options));
            this.repliesRepository = new EfDeletableEntityRepository<Reply>(new ApplicationDbContext(options.Options));
            this.forumCategoriesRepository = new EfRepository<ForumCategory>(new ApplicationDbContext(options.Options));
            this.forumsService = new ForumsService(this.forumsRepository, this.forumCategoriesRepository,
                this.postsRepository, this.repliesRepository);
            AutoMapperConfig.RegisterMappings(typeof(TestForum).Assembly);
        }
    }

    public class TestForum : IMapFrom<Forum>
    {
        
    }
}