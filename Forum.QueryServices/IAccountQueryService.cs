using Forum.QueryServices.DTOs;

namespace Forum.QueryServices
{
    public interface IAccountQueryService
    {
        /// <summary>Find a single account by account name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AccountInfo Find(string name);
    }
}
