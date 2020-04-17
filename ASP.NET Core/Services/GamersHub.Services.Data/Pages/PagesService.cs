using System.Linq;
using System.Threading.Tasks;
using GamersHub.Data.Common.Repositories;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Services.Data.Pages
{
    public class PagesService : IPagesService
    {
        private readonly IDeletableEntityRepository<Page> pagesRepository;

        public PagesService(IDeletableEntityRepository<Page> pagesRepository)
        {
            this.pagesRepository = pagesRepository;
        }

        public T GetByName<T>(string name)
        {
            var page = this.pagesRepository.All()
                .Where(x => x.Name == name)
                .To<T>().FirstOrDefault();

            return page;
        }

        public async Task<int> EditAsync(string name, string content)
        {
            var page = this.pagesRepository.All().FirstOrDefault(x => x.Name == name);
            if (page == null)
            {
                return 0;
            }

            page.Content = content;
            this.pagesRepository.Update(page);
            await this.pagesRepository.SaveChangesAsync();

            return page.Id;
        }
    }
}