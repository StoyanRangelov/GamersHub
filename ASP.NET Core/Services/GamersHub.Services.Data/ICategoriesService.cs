using System.Collections.Generic;
using System.Threading.Tasks;
using GamersHub.Data.Models;
using GamersHub.Web.ViewModels.Categories;

namespace GamersHub.Services.Data
{
    public interface ICategoriesService
    {
        CategoryByNameViewModel GetByNameAndForumId(string name, int id);

        IEnumerable<T> GetAll<T>(int? count = null);

        Task<int> CreateAsync(string name, string description);
    }
}