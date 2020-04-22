using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GamersHub.Services.Data.Games
{
    public class GamesService : IGamesService
    {
        private readonly IDeletableEntityRepository<Game> gamesRepository;
        private readonly IDeletableEntityRepository<Review> reviewsRepositorty;

        public GamesService(IDeletableEntityRepository<Game> gamesRepository,
            IDeletableEntityRepository<Review> reviewsRepositorty)
        {
            this.gamesRepository = gamesRepository;
            this.reviewsRepositorty = reviewsRepositorty;
        }

        public string[] GetTitleUrlAndSubTitleById(int id)
        {
            var game = this.gamesRepository.All()
                .First(x => x.Id == id);

            var titleUrl = UrlParser.ParseToUrl(game.Title);
            var subTitle = game.SubTitle;

            var routeParams = new[] { titleUrl, subTitle };

            return routeParams;
        }

        public T GetById<T>(int id)
        {
            var game = this.gamesRepository.All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return game;
        }

        public T GetByNameUrl<T>(string name, string subTitle)
        {
            var normalisedName = this.GetNormalisedName(name);
            var game = this.gamesRepository.All()
                .Where(x => x.Title == normalisedName && x.SubTitle == subTitle).To<T>().FirstOrDefault();

            return game;
        }

        public IEnumerable<T> GetAll<T>(int? take = null, string searchString = null, int skip = 0)
        {
            var query = this.gamesRepository.All();

            if (searchString != null)
            {
                query = query.Where(x => x.Title.ToLower().Contains(searchString.ToLower()) ||
                                         x.SubTitle.ToLower().Contains(searchString.ToLower()));
            }

            query = query.OrderByDescending(x => x.Reviews.Count).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public async Task<int> CreateAsync(string title, string subTitle, string description, string imageUrl)
        {
            var game = new Game
            {
                Title = title,
                SubTitle = subTitle,
                Description = description,
                ImgUrl = imageUrl,
            };

            await this.gamesRepository.AddAsync(game);
            await this.gamesRepository.SaveChangesAsync();

            return game.Id;
        }

        public async Task<int?> EditAsync(int id, string title, string subTitle, string description, string imageUrl)
        {
            var game = this.gamesRepository.All()
                .FirstOrDefault(x => x.Id == id);

            if (game == null)
            {
                return null;
            }

            game.Title = title;
            game.SubTitle = subTitle;
            game.Description = description;

            if (imageUrl != string.Empty)
            {
                game.ImgUrl = imageUrl;
            }

            this.gamesRepository.Update(game);
            await this.gamesRepository.SaveChangesAsync();

            return game.Id;
        }

        public async Task<int?> DeleteAsync(int id)
        {
            var game = this.gamesRepository.All()
                .Include(x => x.Reviews)
                .FirstOrDefault(x => x.Id == id);

            if (game == null)
            {
                return null;
            }

            foreach (var review in game.Reviews)
            {
                this.reviewsRepositorty.Delete(review);
            }

            await this.reviewsRepositorty.SaveChangesAsync();

            this.gamesRepository.Delete(game);
            await this.gamesRepository.SaveChangesAsync();

            return game.Id;
        }

        public int GetCount(string searchString = null)
        {
            if (searchString != null)
            {
                return this.gamesRepository.All()
                    .Count(x => x.Title.ToLower().Contains(searchString.ToLower()) ||
                                x.SubTitle.ToLower().Contains(searchString.ToLower()));
            }

            return this.gamesRepository.All().Count();
        }

        /// <summary>
        /// Returns the normalised version of the provided game name after comparing it to all other game names through the UrlParser
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        private string GetNormalisedName(string name)
        {
            var forums = this.gamesRepository.All().Select(x => x.Title).ToList();

            var forumName = forums.FirstOrDefault(x => UrlParser.ParseToUrl(x) == name);

            return forumName;
        }
    }
}