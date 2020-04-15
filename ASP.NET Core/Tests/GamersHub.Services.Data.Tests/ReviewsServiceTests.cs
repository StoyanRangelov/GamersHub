using System;
using System.Threading.Tasks;
using GamersHub.Data;
using GamersHub.Data.Models;
using GamersHub.Data.Repositories;
using GamersHub.Services.Data.Reviews;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GamersHub.Services.Data.Tests
{
    [TestFixture]
    public class ReviewsServiceTests
    {
        private EfDeletableEntityRepository<Review> repository;
        private ReviewsService reviewsService;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            await this.repository.AddAsync(new Review {Content = "test Content 1"});
            await this.repository.AddAsync(new Review {Content = "test Content 2"});
            await this.repository.AddAsync(new Review {Content = "test Content 3"});
            await this.repository.AddAsync(new Review {Content = "test Content 4"});
            await this.repository.SaveChangesAsync();
            this.reviewsService = new ReviewsService(this.repository);
            AutoMapperConfig.RegisterMappings(typeof(TestReview).Assembly);
        }

        [Test]
        public void TestGetByIdReturnsCorrectEntity()
        {
            var review = this.reviewsService.GetById<TestReview>(1);

            Assert.AreEqual("test Content 1", review.Content);
        }

        [Test]
        public void TestGetByIdReturnsNullWhenNonExistentIdIsGiven()
        {
            var review = this.reviewsService.GetById<TestReview>(0);

            Assert.Null(review);
        }

        public class TestReview : IMapFrom<Review>
        {
            public string Content { get; set; }
        }
    }
}