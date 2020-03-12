using System.Collections.Generic;

namespace GamersHub.Services.Data
{
    public interface IForumsService
    {
        IEnumerable<T> GetAll<T>(int? count = null);
    }
}