using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Services.Data.Games
{
    public class GamesService : IGamesService
    {
        private readonly IDeletableEntityRepository<Game> gamesRepository;

        public GamesService(IDeletableEntityRepository<Game> gamesRepository)
        {
            this.gamesRepository = gamesRepository;
        }

        public T GetById<T>(int id)
        {
            var game = this.gamesRepository.All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return game;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var games = this.gamesRepository.All()
                .To<T>().ToList();

            return games;
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
    }
}