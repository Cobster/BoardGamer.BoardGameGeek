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

        /// <summary>
        /// Gets the basic public profile information about a user by username from boardgamegeek.com
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A response object containing the user profile information.</returns>
        Task<UserResponse> GetUserAsync(UserRequest request);
    }
}
