using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public static class BoardGameGeekXmlApi2ClientExtensions
    {
        public static async Task<ThingResponse.Item> GetBoardGameAsync(this IBoardGameGeekXmlApi2Client bggClient, int id)
            =>  (await GetBoardGamesAsync(bggClient, id)).FirstOrDefault();

        public static async Task<IEnumerable<ThingResponse.Item>> GetBoardGamesAsync(this IBoardGameGeekXmlApi2Client bggClient, params int[] ids) {
            var request = new ThingRequest(
                ids,
                types: new[] { "boardgame" },
                stats: true);

            ThingResponse response = await bggClient.GetThingAsync(request);

            return response.Result;
        }
    }
}
