﻿using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;

namespace GamersHub.Services.Data.Games
{
    public class GamesService : IGamesService
    {
        private readonly IDeletableEntityRepository<Game> gamesRepository;

        public GamesService(IDeletableEntityRepository<Game> gamesRepository)
        {
            this.gamesRepository = gamesRepository;
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