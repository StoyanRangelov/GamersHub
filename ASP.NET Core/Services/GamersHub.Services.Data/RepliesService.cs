using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Services.Data
{
    public class RepliesService : IRepliesService
    {
        private readonly IDeletableEntityRepository<Reply> repliesRepository;

        public RepliesService(IDeletableEntityRepository<Reply> repliesRepository)
        {
            this.repliesRepository = repliesRepository;
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

        public async Task<int> EditAsync(int id, string content)
        {
            var reply = this.repliesRepository.All()
                .FirstOrDefault(x => x.Id == id);

            if (reply == null)
            {
                return 0;
            }

            reply.Content = content;

            this.repliesRepository.Update(reply);
            await this.repliesRepository.SaveChangesAsync();

            return reply.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var reply = this.repliesRepository.All()
                .FirstOrDefault(x => x.Id == id);

            this.repliesRepository.Delete(reply);
            await this.repliesRepository.SaveChangesAsync();
        }
    }
}