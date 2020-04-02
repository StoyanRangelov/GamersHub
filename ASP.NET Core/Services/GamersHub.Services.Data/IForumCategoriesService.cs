namespace GamersHub.Services.Data
{
    public interface IForumCategoriesService
    {
        T GetByNameAndForumId<T>(string name, int id);
    }
}
