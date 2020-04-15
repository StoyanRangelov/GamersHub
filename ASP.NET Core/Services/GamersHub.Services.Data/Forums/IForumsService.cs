using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Forums
{
    public interface IForumsService
    {
        /// <summary>
        ///  Returns a number of forums from the database, based on the provided take and skip values
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        /// <summary>
        /// Returns all forums by the given forum id
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllByCategoryId<T>(int id);


        /// <summary>
        /// Returns a forum from the database by the specified forum name, otherwise returns null if such a forum does not exist
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetByName<T>(string name);


       /// <summary>
       /// Returns a forum from the database by the specified forum id, otherwise returns null if such a forum does not exist
       /// </summary>
       /// <param name="id"></param>
       /// <typeparam name="T"></typeparam>
       /// <returns></returns>
        T GetById<T>(int id);


       /// <summary>
       /// Creates a new forum with the given name and adds it to the database, returns 0 if the forum name is already taken, otherwise returns the forum id
       /// </summary>
       /// <param name="name"></param>
       /// <returns></returns>
        Task<int> CreateAsync(string name);

        /// <summary>
        ///  Edits a forum by the given id and updates it with the given name and categories to include in it, returns -1 if the forum does not exist, returns 0 if the forum name is already taken, otherwise returns the forum id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="categoryIds"></param>
        /// <param name="areSelected"></param>
        /// <returns></returns>
        Task<int> EditAsync(int id, string name, int[] categoryIds, bool[] areSelected);


        /// <summary>
        ///  Deletes the forum with the given id and all of its relations
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// Returns the count of all forums in the database
        /// </summary>
        /// <returns></returns>
        int GetCount();
    }
}
