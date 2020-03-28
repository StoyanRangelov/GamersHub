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

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<Forum> forumsRepository)
        {
            this.postsRepository = postsRepository;
            this.forumsRepository = forumsRepository;
        }

        public T GetById<T>(int id)
        {
            var post = this.postsRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();

            return post;
        }

        public async Task<int> CreateAsync(int forumId, int categoryId, string name, string content, string userId)
        {
            var forum = this.forumsRepository.All()
                .Include(x => x.ForumCategories)
                .First(x => x.Id == forumId);

            if (!forum.ForumCategories.Select(fc => fc.CategoryId).Contains(categoryId))
            {
                var forumCategory = new ForumCategory
                {
                    ForumId = forumId,
                    CategoryId = categoryId,
                };

                forum.ForumCategories.Add(forumCategory);

                this.forumsRepository.Update(forum);
                await this.forumsRepository.SaveChangesAsync();
            }

            var post = new Post
            {
                ForumId = forumId,
                CategoryId = categoryId,
                Name = name,
                Content = content,
                UserId = userId,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();

            return post.Id;
        }

        public async Task<int> Edit(int id, string name, string content)
        {
            var post = this.postsRepository.All().FirstOrDefault(x => x.Id == id);

            post.Name = name;
            post.Content = content;

            this.postsRepository.Update(post);
            await this.postsRepository.SaveChangesAsync();

            return post.Id;
        }
    }
}