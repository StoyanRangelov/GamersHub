using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamersHub.Services.Data.Games
{
    public interface IGamesService
    {
        /// <summary>
        /// Finds the game title of a by the provided game id and returns its name after running it through the UrlParser
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetUrl(int id);

        /// <summary>
        /// Returns a game from the database by the specified game id, otherwise returns null if such a game does not exist
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetById<T>(int id);

        /// <summary>
        /// Returns a game from the database by the specified game title, otherwise returns null if such a game does not exist
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetByName<T>(string name);

        /// <summary>
        ///  Returns a number of games from the database, based on the provided take and skip values
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(int? take = null, int skip = 0);

        /// <summary>
        /// Creates a new game with the given name and adds it to the database, returns 0 if the forum name is already taken, otherwise returns the forum id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<int> CreateAsync(string title, string subTitle, string description, string imageUrl);

        /// <summary>
        /// Edits a game by the given id and updates it with the given title, sub title, description and image url, returns 0 if the game does not exist, otherwise returns the game id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="subTitle"></param>
        /// <param name="description"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        Task<int> EditAsync(int id, string title, string subTitle, string description, string imageUrl);

        /// <summary>
        /// Deletes the game with the given id and all of its relations
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task DeleteAsync(int id);

        /// <summary>
        /// Returns the count of all the games in the database
        /// </summary>
        /// <returns></returns>
        int GetCount();
    }
}