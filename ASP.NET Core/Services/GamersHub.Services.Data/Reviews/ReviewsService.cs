﻿using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Services.Data.Reviews
{
    public class ReviewsService : IReviewsService
    {
        private readonly IDeletableEntityRepository<Review> reviewsRepository;

        public ReviewsService(IDeletableEntityRepository<Review> reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public T GetById<T>(int id)
        {
            var review = this.reviewsRepository.All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return review;
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

        public async Task<int> EditAsync(int id, string content, bool isPositive)
        {
            var review = this.reviewsRepository.All()
                .FirstOrDefault(x => x.Id == id);

            if (review == null)
            {
                return 0;
            }

            review.Content = content;
            review.IsPositive = isPositive;

            this.reviewsRepository.Update(review);
            await this.reviewsRepository.SaveChangesAsync();

            return review.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var review = this.reviewsRepository.All()
                .FirstOrDefault(x => x.Id == id);

            this.reviewsRepository.Delete(review);
            await this.reviewsRepository.SaveChangesAsync();
        }
    }
}