using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
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

        public T GetByNameUrl<T>(string name)
        {
            var normalisedName = this.GetNormalisedName(name);

            var post = this.postsRepository.All()
                .Where(x => x.Name == normalisedName)
                .To<T>().FirstOrDefault();

            return post;
        }

        public T GetById<T>(int id)
        {
            var post = this.postsRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();

            return post;
        }

        public IEnumerable<T> GetAll<T>(int? take = null, int skip = 0)
        {
            var query = this.postsRepository.All()
                .OrderByDescending(x => x.CreatedOn).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetTopFive<T>()
        {
            var posts = this.postsRepository.All()
                .OrderByDescending(x => x.Replies.Count)
                .Take(5).To<T>().ToList();

            return posts;
        }

        public IEnumerable<T> GetAllByForumId<T>(int forumId, int? take = null, int skip = 0)
        {
            var query = this.postsRepository.All()
                .Where(x => x.ForumId == forumId).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllByCategoryNameAndForumId<T>(string name, int id, int? take = null, int skip = 0)
        {
            var query = this.postsRepository.All()
                .Where(x => x.ForumId == id && x.Category.Name == name)
                .OrderByDescending(x => x.CreatedOn).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public async Task<int?> CreateAsync(int forumId, int categoryId, string name, string content, string userId)
        {
            var forum = this.forumsRepository
                .All()
                .Include(x => x.ForumCategories)
                .FirstOrDefault(x => x.Id == forumId);

            if (forum == null)
            {
                return null;
            }

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

        public async Task<int?> EditAsync(int id, string name, string content)
        {
            var post = this.postsRepository.All().FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return null;
            }

            post.Name = name;
            post.Content = content;

            this.postsRepository.Update(post);
            await this.postsRepository.SaveChangesAsync();

            return post.Id;
        }

        public async Task<int?> DeleteAsync(int id)
        {
            var post = this.postsRepository.All()
                .Include(x => x.Replies)
                .FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return null;
            }

            foreach (var reply in post.Replies)
            {
                this.repliesRepository.Delete(reply);
            }

            await this.repliesRepository.SaveChangesAsync();

            this.postsRepository.Delete(post);
            await this.postsRepository.SaveChangesAsync();

            return post.Id;;
        }

        public int GetCountByForumId(int forumId)
        {
            return this.postsRepository.All().Count(x => x.ForumId == forumId);
        }

        public int GetCount()
        {
            return this.postsRepository.All().Count();
        }

        public int GetCountByCategoryNameAndForumId(string name, int forumId)
        {
            return this.postsRepository.All().Count(x => x.Category.Name == name && x.ForumId == forumId);
        }


        private string GetNormalisedName(string name)
        {
            var posts = this.postsRepository.All().Select(x => x.Name).ToList();

            var postName = posts.FirstOrDefault(x => UrlParser.ParseToUrl(x).ToLower() == name);

            return postName;
        }
    }
}