using Forum.QueryServices.DTOs;

namespace Forum.QueryServices
{
    public interface IAccountQueryService
    {
        /// <summary>根据账号名获取账号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AccountInfo Find(string name);
    }
}
