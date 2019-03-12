using System.Threading.Tasks;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public interface IBoardGameGeekXmlApi2Client
    {
        /// <summary>
        /// Gets the details about a user's collection.
        /// </summary>
        /// <param name="request">An object representing the request to get the user's collection from boardgamegeek.com</param>
        /// <returns>An object containing the user's collection details.</returns>
        Task<CollectionResponse> GetCollectionAsync(CollectionRequest request);

        Task<ForumListResponse> GetForumListAsync(ForumListRequest request);
        Task<ForumsResponse> GetForumsAsync(ForumsRequest request);

        /// <summary>
        /// Gets the details about a guild.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A <see cref="GuildResponse"/> object</returns>
        Task<GuildResponse> GetGuildAsync(GuildRequest request);

        Task<HotItemsResponse> GetHotItemsAsync(HotItemsRequest request);

        /// <summary>
        /// Get the details about a user's logged plays.
        /// </summary>
        /// <param name="request">An object representing the request to get a user's logged plays from boardgamegeek.com</param>
        /// <returns>A collection of logged plays.</returns>
        Task<PlaysResponse> GetPlaysAsync(PlaysRequest request);

        /// <summary>
        /// Gets the details about a board game, board game expansion, board game accessory, video game, rpg item, or rpg issue
        /// from the boardgamegeek.com database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A single or set of things.</returns>
        Task<ThingResponse> GetThingAsync(ThingRequest request);

        Task<ThreadsResponse> GetThreadsAsync(ThreadsRequest request);

        /// <summary>
        /// Gets the basic public profile information about a user by username from boardgamegeek.com
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A response object containing the user profile information.</returns>
        Task<UserResponse> GetUserAsync(UserRequest request);

        /// <summary>
        /// Search for items in the boardgamegeek.com database.
        /// </summary>
        /// <param name="request">An object that represents the search request.</param>
        /// <returns>An object containing the search results.</returns>
        Task<SearchResponse> SearchAsync(SearchRequest request);
    }
}
