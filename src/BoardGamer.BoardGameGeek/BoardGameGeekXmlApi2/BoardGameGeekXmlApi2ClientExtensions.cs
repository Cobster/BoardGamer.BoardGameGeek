using System.Linq;
using System.Threading.Tasks;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public static class BoardGameGeekXmlApi2ClientExtensions
    {
        public static async Task<ThingResponse.Item> GetBoardGameAsync(this IBoardGameGeekXmlApi2Client bggClient, int id)
        {
            var request = new ThingRequest(
                new[] { id },
                types: new[] { "boardgame" }, 
                stats: true);

            ThingResponse response = await bggClient.GetThingAsync(request);

            return response.Result.FirstOrDefault();
        }
    }
}
