using System.Collections.Generic;

namespace GamersHub.Services.Data
{
    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>(int? count = null);
    }
}