namespace Forum.QueryServices
{
    public interface IReplyQueryService
    {
        /// <summary>Find a single reply, returns the dynamic data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        dynamic FindDynamic(string id, string option);
    }
}