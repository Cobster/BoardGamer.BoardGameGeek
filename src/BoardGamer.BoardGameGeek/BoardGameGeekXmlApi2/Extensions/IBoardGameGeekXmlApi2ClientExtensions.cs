using System;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2.Extensions
{
    public static class IBoardGameGeekXmlApi2ClientExtensions
    {
        public async static Task<ThingResponse.Item> GetGameAsync(this IBoardGameGeekXmlApi2Client client, int gameId)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            var request = new ThingRequest(new[] { gameId });
            var response = await client.GetThingAsync(request);
            return response.Result.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a players collection from boardgamegeek.com
        /// </summary>
        /// <param name="client">A boardgamegeek client instance.</param>
        /// <param name="username">The username of the player on boardgamegeek.com.</param>
        /// <returns></returns>
        public async static Task<CollectionResponse.ItemCollection> GetCollectionAsync(this IBoardGameGeekXmlApi2Client client, string username)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (username == null) throw new ArgumentNullException(nameof(username));

            var request = new CollectionRequest(username, own: true);
            var response = await client.GetCollectionAsync(request);
            return response.Result;
        }

        public async static Task<UserResponse.User> GetUserAsync(this IBoardGameGeekXmlApi2Client client, string username)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (username == null) throw new ArgumentNullException(nameof(username));

            var request = new UserRequest(username);
            var response = await client.GetUserAsync(request);
            return response.Result;
        }
    }
}
