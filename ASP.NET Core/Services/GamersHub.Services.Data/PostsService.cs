﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GamersHub.Services.Data
{
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<Forum> forumsRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<Forum> forumsRepository,
            IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.postsRepository = postsRepository;
            this.forumsRepository = forumsRepository;
            this.categoriesRepository = categoriesRepository;
        }

        public T GetById<T>(int id)
        {
            var post = this.postsRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();

            return post;
        }

        public async Task<int> CreateAsync(string forumName, string categoryName, string name, string content, string userId)
        {
            var forum = this.forumsRepository.All()
                .Include(x => x.ForumCategories)
                .First(x => x.Name == forumName);
            var category = this.categoriesRepository.All().First(x => x.Name == categoryName);

            if (!forum.ForumCategories.Select(fc => fc.CategoryId).Contains(category.Id))
            {
                var forumCategory = new ForumCategory
                {
                    ForumId = forum.Id,
                    CategoryId = category.Id,
                };

                forum.ForumCategories.Add(forumCategory);

                this.forumsRepository.Update(forum);
                await this.forumsRepository.SaveChangesAsync();
            }

            var post = new Post
            {
                Name = name,
                Content = content,
                UserId = userId,
                ForumId = forum.Id,
                CategoryId = category.Id,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();

            return post.Id;
        }
    }
}