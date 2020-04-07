using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;

namespace GamersHub.Services.Data.Reviews
{
    public class ReviewsService : IReviewsService
    {
        private readonly IDeletableEntityRepository<Review> reviewsRepository;

        public ReviewsService(IDeletableEntityRepository<Review> reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public async Task<int> CreateAsync(int gameId, bool isPositive, string content, string userId)
        {
            var review = new Review
            {
                Content = content,
                UserId = userId,
                GameId = gameId,
                IsPositive = isPositive,
            };

            await this.reviewsRepository.AddAsync(review);
            await this.reviewsRepository.SaveChangesAsync();

            return review.Id;
        }
    }
}