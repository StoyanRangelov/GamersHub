﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GamersHub.Services.Data.Posts
{
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<Forum> forumsRepository;
        private readonly IDeletableEntityRepository<Reply> repliesRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<Forum> forumsRepository,
            IDeletableEntityRepository<Reply> repliesRepository)
        {
            this.postsRepository = postsRepository;
            this.forumsRepository = forumsRepository;
            this.repliesRepository = repliesRepository;
        }

        public T GetById<T>(int id)
        {
            var post = this.postsRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();

            return post;
        }

        public IEnumerable<T> GetAllByCategoryNameAndForumId<T>(string name, int id)
        {
            var posts = this.postsRepository.All()
                .Where(x => x.ForumId == id && x.Category.Name == name)
                .To<T>().ToList();

            return posts;
        }

        public async Task<int> CreateAsync(int forumId, int categoryId, string name, string content, string userId)
        {
            var forum = this.forumsRepository
                .All()
                .Include(x=>x.ForumCategories)
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

        public async Task<int> EditAsync(int id, string name, string content)
        {
            var post = this.postsRepository.All().FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return 0;
            }

            post.Name = name;
            post.Content = content;

            this.postsRepository.Update(post);
            await this.postsRepository.SaveChangesAsync();

            return post.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var post = this.postsRepository.All()
                .FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return;
            }

            var replies = this.repliesRepository.All()
                .Where(x => x.PostId == id).ToList();

            foreach (var reply in replies)
            {
                this.repliesRepository.Delete(reply);
            }

            await this.repliesRepository.SaveChangesAsync();

            this.postsRepository.Delete(post);
            await this.postsRepository.SaveChangesAsync();
        }
    }
}