using System;
using System.Threading.Tasks;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public interface IBoardGameGeekXmlApi2Client
    {
        /// <summary>
        /// Gets the basic public profile information about a user by <paramref name="username"/> from boardgamegeek.com
        /// </summary>
        /// <param name="username">The name of the user to find.</param>
        /// <param name="buddies">Returns the users buddies.</param>
        /// <param name="guilds">Returns the users guilds.</param>
        /// <param name="hot">Includes the user's hot 10 list from their profile.</param>
        /// <param name="top">Includes the user's top 10 list from their profile.</param>
        /// <param name="domain">Controls the context of the user's hot and top 10 lists.</param>
        /// <param name="page"></param>
        /// <returns>A user object.</returns>
        Task<User> GetUserAsync(string username,
            bool buddies = false,
            bool guilds = false,
            bool hot = false,
            bool top = false,
            string domain = null,
            int page = 0);
    }

}
