using System.Collections.Generic;

namespace GamersHub.Services.Data
{
    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        void Create(string name, string description);
    }
}