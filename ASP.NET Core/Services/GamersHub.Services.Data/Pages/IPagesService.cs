using System.Threading.Tasks;

namespace GamersHub.Services.Data.Pages
{
    public interface IPagesService
    {
        /// <summary>
        /// returns the page with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetByName<T>(string name);


        /// <summary>
        /// edits the content of the page with the given name, returns null if the page does not exist, return page id if successful
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<int?> EditAsync(string name, string content);
    }
}