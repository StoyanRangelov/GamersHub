using System;
using System.Linq;
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
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            this.reviewsService = new ReviewsService(this.repository);
            AutoMapperConfig.RegisterMappings(typeof(TestReview).Assembly);
        }

        [Test]
        public async Task TestGetByIdReturnsCorrectEntity()
        {
            await this.repository.AddAsync(new Review {Content = "test Content"});
            await this.repository.SaveChangesAsync();

            var review = this.reviewsService.GetById<TestReview>(1);

            var expected = "test Content";
            var actual = review.Content;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGetByIdReturnsNullWhenNonExistentIdIsGiven()
        {
            var review = this.reviewsService.GetById<TestReview>(0);

            Assert.Null(review);
        }

        [Test]
        public async Task TestCreateAsync()
        {
            var reviewId = await this.reviewsService.CreateAsync(1, true, "test Content", "userId");
            var review = this.repository.All().First();

            Assert.AreEqual(reviewId, review.Id);
            Assert.AreEqual(1, review.GameId);
            Assert.AreEqual(true, review.IsPositive);
            Assert.AreEqual("test Content", review.Content);
            Assert.AreEqual("userId", review.UserId);
        }

        [Test]
        public async Task TestGetAllByGameIdReturnsAllReviewsWithGameId1()
        {
            for (int i = 0; i < 10; i++)
            {
                await this.repository.AddAsync(new Review {GameId = 1, Content = "test Content"});
            }

            await this.repository.AddAsync(new Review {GameId = 2, Content = "test Content fail"});
            await this.repository.SaveChangesAsync();

            var reviews = this.reviewsService.GetAllByGameId<TestReview>(1).ToList();

            var expectedGameId = 1;
            var expectedContent = "test Content";

            foreach (var testReview in reviews)
            {
                Assert.AreEqual(expectedGameId, testReview.GameId);
                Assert.AreEqual(expectedContent, testReview.Content);
            }

            Assert.AreEqual(10, reviews.Count);
        }

        [Test]
        public async Task TestGetAllByGameIdReturnsReviewsCorrectlyWithSkipAndTakeValues()
        {
            for (int i = 0; i < 3; i++)
            {
                await this.repository.AddAsync(new Review {GameId = 1, Content = "test Content skip"});
            }

            for (int i = 0; i < 5; i++)
            {
                await this.repository.AddAsync(new Review {GameId = 1, Content = "test Content"});
            }

            await this.repository.AddAsync(new Review {GameId = 1, Content = "test Content fail"});
            await this.repository.AddAsync(new Review {GameId = 2, Content = "test Content fail"});

            await this.repository.SaveChangesAsync();

            var reviews = this.reviewsService.GetAllByGameId<TestReview>(1, 5, 3).ToList();

            var expectedGameId = 1;
            var expectedContent = "test Content";

            foreach (var testReview in reviews)
            {
                Assert.AreEqual(expectedGameId, testReview.GameId);
                Assert.AreEqual(expectedContent, testReview.Content);
            }

            Assert.AreEqual(5, reviews.Count);
        }

        [Test]
        public async Task TestEditAsyncWorksCorrectly()
        {
            await this.repository.AddAsync(new Review {Content = "test Content", IsPositive = true});
            await this.repository.SaveChangesAsync();

            var reviewId = await this.reviewsService.EditAsync(1, "test Content edited", false);

            var actualReview = this.repository.All().First();

            Assert.AreEqual("test Content edited", actualReview.Content);
            Assert.IsFalse(actualReview.IsPositive);
        }


        [Test]
        public async Task TestEditAsyncReturnsNullWithInvalidId()
        {
            var reviewId = await this.reviewsService.EditAsync(2, "test Content", true);

            Assert.Null(reviewId);
        }


        [Test]
        public async Task TestDeleteAsyncReturnsNullWithInvalidId()
        {
            var reviewId = await this.reviewsService.DeleteAsync(1);

            Assert.Null(reviewId);
        }

        [Test]
        public async Task TestDeleteAsyncWorksCorrectly()
        {
            await this.repository.AddAsync(new Review {Content = "test Content"});
            await this.repository.SaveChangesAsync();

            var reviewId =await this.reviewsService.DeleteAsync(1);

            var review = this.repository.AllWithDeleted().First();

            Assert.AreEqual(1, reviewId);
            Assert.IsTrue(review.IsDeleted);
        }

        [Test]
        public async Task TestGetCountByGameId()
        {
            for (int i = 0; i < 6; i++)
            {
                await this.repository.AddAsync(new Review {GameId = 1, Content = "test Content"});
            }

            for (int i = 2; i < 5; i++)
            {
                await this.repository.AddAsync(new Review {GameId = i, Content = "test Content"});
            }

            await this.repository.SaveChangesAsync();

            var actualCount = this.reviewsService.GetCountByGameId(1);

            var expectedCount = 6;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }

    public class TestReview : IMapFrom<Review>
    {
        public int GameId { get; set; }

        public bool IsPositive { get; set; }

        public string Content { get; set; }
    }
}