using System.Threading.Tasks;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public interface IBoardGameGeekXmlApi2Client
    {
        /// <summary>
        /// Gets the basic public profile information about a user by username from boardgamegeek.com
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A response object containing the user profile information.</returns>
        Task<UserResponse> GetUserAsync(UserRequest request);
        

        
    }

}
