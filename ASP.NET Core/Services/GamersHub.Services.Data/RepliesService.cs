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
    }
}