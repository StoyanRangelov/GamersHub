namespace GamersHub.Services.Data.Replies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Data.Common.Repositories;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class RepliesService : IRepliesService
    {
        private readonly IDeletableEntityRepository<Reply> repliesRepository;

        public RepliesService(IDeletableEntityRepository<Reply> repliesRepository)
        {
            this.repliesRepository = repliesRepository;
        }

        public IEnumerable<T> GetAllByPostId<T>(int postId, int? take = null, int skip = 0)
        {
            var query = this.repliesRepository.All()
                .Where(x => x.PostId == postId).Skip(skip);
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            var reply = this.repliesRepository.All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return reply;
        }

        public async Task<int> CreateAsync(int postId, string userId, string content)
        {
            var reply = new Reply
            {
                PostId = postId,
                Content = content,
                UserId = userId,
            };

            await this.repliesRepository.AddAsync(reply);
            await this.repliesRepository.SaveChangesAsync();

            return reply.Id;
        }

        public async Task<int?> EditAsync(int id, string content)
        {
            var reply = this.repliesRepository.All()
                .FirstOrDefault(x => x.Id == id);

            if (reply == null)
            {
                return null;
            }

            reply.Content = content;

            this.repliesRepository.Update(reply);
            await this.repliesRepository.SaveChangesAsync();

            return reply.Id;
        }

        public async Task<int?> DeleteAsync(int id)
        {
            var reply = this.repliesRepository.All()
                .FirstOrDefault(x => x.Id == id);
            if (reply == null)
            {
                return null;
            }

            this.repliesRepository.Delete(reply);
            await this.repliesRepository.SaveChangesAsync();

            return reply.Id;
        }

        public int GetCountByPostId(int postId)
        {
            return this.repliesRepository.All().Count(x => x.PostId == postId);
        }
    }
}
