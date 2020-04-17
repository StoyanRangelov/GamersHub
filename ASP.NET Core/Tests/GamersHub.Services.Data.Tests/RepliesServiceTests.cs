using System;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data;
using GamersHub.Data.Common.Repositories;
using GamersHub.Services.Data.Replies;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GamersHub.Services.Data.Tests
{
    [TestFixture]
    public class RepliesServiceTests
    {
        private EfDeletableEntityRepository<Reply> repository;
        private RepliesService repliesService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfDeletableEntityRepository<Reply>(new ApplicationDbContext(options.Options));
            this.repliesService = new RepliesService(this.repository);
            AutoMapperConfig.RegisterMappings(typeof(TestReply).Assembly);
        }

        [Test]
        public async Task TestGetAllByPostId()
        {
            for (int i = 0; i < 5; i++)
            {
                await this.repository.AddAsync(new Reply {PostId = 1, Content = "test Content"});
            }

            await this.repository.AddAsync(new Reply {PostId = 2, Content = "test Content fail"});
            await this.repository.SaveChangesAsync();

            var replies = this.repliesService.GetAllByPostId<TestReply>(1).ToList();

            var expectedPostId = 1;
            var expectedContent = "test Content";

            foreach (var testReply in replies)
            {
                Assert.AreEqual(expectedPostId, testReply.PostId);
                Assert.AreEqual(expectedContent, testReply.Content);
            }

            Assert.AreEqual(5, replies.Count);
        }

        [Test]
        public async Task TestGetAllByPostIdWithTakeAndSkipValues()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.repository.AddAsync(new Reply {PostId = 1, Content = "test Content skip"});
            }

            for (int i = 0; i < 5; i++)
            {
                await this.repository.AddAsync(new Reply {PostId = 1, Content = "test Content"});
            }

            await this.repository.AddAsync(new Reply {PostId = 1, Content = "test Content fail"});
            await this.repository.AddAsync(new Reply {PostId = 2, Content = "test Content fail"});
            await this.repository.SaveChangesAsync();

            var replies = this.repliesService.GetAllByPostId<TestReply>(1, 5, 3).ToList();

            var expectedPostId = 1;
            var expectedContent = "test Content";

            foreach (var testReply in replies)
            {
                Assert.AreEqual(expectedPostId, testReply.PostId);
                Assert.AreEqual(expectedContent, testReply.Content);
            }

            Assert.AreEqual(5, replies.Count);
        }

        [Test]
        public async Task TestGetById()
        {
            await this.repository.AddAsync(new Reply {Content = "test Content 1"});
            await this.repository.AddAsync(new Reply {Content = "test Content 2"});
            await this.repository.SaveChangesAsync();

            var reply = this.repliesService.GetById<TestReply>(2);

            var expectedContent = "test Content 2";

            Assert.AreEqual(expectedContent, reply.Content);
        }

        [Test]
        public async Task TestCreateAsync()
        {
            var replyId = await this.repliesService.CreateAsync(1, "userId", "test Content");
            var reply = this.repository.All().First();

            Assert.AreEqual(1, replyId);
            Assert.AreEqual(1, reply.PostId);
            Assert.AreEqual("userId", reply.UserId);
            Assert.AreEqual("test Content", reply.Content);
        }

        [Test]
        public async Task TestEditAsync()
        {
            await this.repository.AddAsync(new Reply {Content = "test Content"});
            await this.repository.SaveChangesAsync();

            var replyId = await this.repliesService.EditAsync(1, "edited test Content");

            var reply = this.repository.All().First();

            Assert.AreEqual("edited test Content", reply.Content);
            Assert.AreEqual(1, replyId);
        }

        [Test]
        public async Task TestEditAsyncReturns0WithInvalidId()
        {
            var replyId = await this.repliesService.EditAsync(2, "test Content");

            Assert.Zero(replyId);
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            await this.repository.AddAsync(new Reply {Content = "test Content"});
            await this.repository.SaveChangesAsync();

            await this.repliesService.DeleteAsync(1);

            var reply = await this.repository.AllWithDeleted().FirstAsync();

            Assert.IsTrue(reply.IsDeleted);
        }

        [Test]
        public async Task GetCountByPostId()
        {
            for (int i = 0; i < 4; i++)
            {
                await this.repository.AddAsync(new Reply {PostId = 1, Content = "test Content"});
            }

            for (int i = 2; i < 5; i++)
            {
                await this.repository.AddAsync(new Reply {PostId = i, Content = "test Content"});
            }

            await this.repository.SaveChangesAsync();

            var actualCount = this.repliesService.GetCountByPostId(1);
            var expectedCount = 4;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }

    public class TestReply : IMapFrom<Reply>
    {
        public int PostId { get; set; }

        public string Content { get; set; }
    }
}