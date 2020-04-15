using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamersHub.Data;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GamersHub.Services.Data.Tests
{
    [TestFixture]
    public class PostsServiceTests
    {
        private EfDeletableEntityRepository<Post> postsRepository;
        private EfDeletableEntityRepository<Forum> forumsRepository;
        private EfDeletableEntityRepository<Reply> repliesRepository;
        private PostsService postsService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.postsRepository = new EfDeletableEntityRepository<Post>(new ApplicationDbContext(options.Options));
            this.forumsRepository = new EfDeletableEntityRepository<Forum>(new ApplicationDbContext(options.Options));
            this.repliesRepository = new EfDeletableEntityRepository<Reply>(new ApplicationDbContext(options.Options));
            this.postsService = new PostsService(this.postsRepository, this.forumsRepository, this.repliesRepository);
            AutoMapperConfig.RegisterMappings(typeof(TestPost).Assembly);
        }

        [Test]
        public async Task TestGetByNameUrl()
        {
            await this.postsRepository.AddAsync(new Post {Name = "name: test name", Content = "test content"});
            await this.postsRepository.SaveChangesAsync();

            var nameAsUrl = "name-test-name";

            var post = this.postsService.GetByNameUrl<TestPost>(nameAsUrl);

            Assert.AreEqual("name: test name", post.Name);
            Assert.AreEqual("test content", post.Content);
        }

        [Test]
        public async Task TestById()
        {
            await this.postsRepository.AddAsync(new Post {Name = "name", Content = "content"});
            await this.postsRepository.SaveChangesAsync();

            var post = this.postsService.GetById<TestPost>(1);

            Assert.AreEqual("name", post.Name);
            Assert.AreEqual("content", post.Content);
        }

        [Test]
        public async Task TestGetAll()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post {Name = "name", Content = "content"});
            }

            await this.postsRepository.SaveChangesAsync();

            var posts = this.postsService.GetAll<TestPost>();

            foreach (var testPost in posts)
            {
                Assert.AreEqual("name", testPost.Name);
                Assert.AreEqual("content", testPost.Content);
            }
        }

        [Test]
        public async Task TestGetAllWithSkipAndTakeValues()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.postsRepository.AddAsync(new Post
                    {Name = "skip name", Content = "skip", CreatedOn = new DateTime(2020, 04, 15)});
            }

            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                    {Name = "name", Content = "take", CreatedOn = new DateTime(2020, 04, 14)});
            }

            await this.postsRepository.AddAsync(new Post
                {Name = "test name", Content = "content", CreatedOn = new DateTime(2020, 04, 13)});
            await this.postsRepository.AddAsync(new Post
                {Name = "test name", Content = "content", CreatedOn = new DateTime(2020, 04, 12)});
            await this.postsRepository.SaveChangesAsync();

            var posts = this.postsService.GetAll<TestPost>(5, 3);

            foreach (var testPost in posts)
            {
                Assert.AreEqual("name", testPost.Name);
                Assert.AreEqual("take", testPost.Content);
            }
        }

        [Test]
        public async Task TestGetTopFive()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.postsRepository.AddAsync(new Post
                    { Name = "Post with five Replies", Replies = new List<Reply> { new Reply(), new Reply(), new Reply(), new Reply(), new Reply() } });
            }

            await this.postsRepository.AddAsync(new Post{ Name = "no replies" });
            await this.postsRepository.AddAsync(new Post { Name = "no replies" });
            await this.postsRepository.SaveChangesAsync();

            var posts = this.postsService.GetTopFive<TestPost>();

            foreach (var testPost in posts)
            {
                Assert.AreEqual("Post with five Replies", testPost.Name);
            }
        }
    }

    public class TestPost : IMapFrom<Post>
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}