using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BoardGamer.BoardGameGeek.Tests
{
    public class BoardGameGeekClientTests
    {
        [Fact]
        public async Task Should_retrieve_user_by_boardgamegeek_username()
        {
            HttpClient http = new HttpClient();

            IBoardGameGeekXmlApi2Client bgg = new BoardGameGeekXmlApi2Client(http);

            UserResponse response = await bgg.GetUserAsync(new UserRequest("jakefromstatefarm", buddies: true, hot: true, top: true));

            Assert.True(response.Succeeded);

            User user = response.User;

            Assert.NotNull(user);
            Assert.Equal("1266617", user.Id);
            Assert.Equal("jakefromstatefarm", user.Name);
            Assert.Equal("Jake", user.FirstName);
            Assert.Equal("Bruun", user.LastName);
            Assert.Equal("2016", user.YearRegistered);
            Assert.Equal("Oregon", user.StateOrProvince);
            Assert.Equal("United States", user.Country);
            Assert.Equal(4, user.Buddies.Count);
            Assert.Equal(3, user.Top.Count);
            Assert.Single(user.Hot);
        }

        [Fact]
        public async Task Should_retrieve_users_game_collection()
        {
            HttpClient http = new HttpClient();

            IBoardGameGeekXmlApi2Client bgg = new BoardGameGeekXmlApi2Client(http);

            CollectionResponse response = await bgg.GetCollectionAsync(new CollectionRequest(
                "jakefromstatefarm",
                stats: true));

            Assert.True(response.Succeeded);

            Collection collection = response.Collection;

            Assert.NotNull(collection);

            Assert.Equal(55, collection.Items.Count);

        }

        [Fact]
        public async Task Should_retrieve_a_boardgame_by_id()
        {
            HttpClient http = new HttpClient();

            IBoardGameGeekXmlApi2Client bgg = new BoardGameGeekXmlApi2Client(http);

            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, versions: true));
            Assert.True(response.Succeeded);

            Thing game = response.Things.FirstOrDefault();
            Assert.NotNull(game);

            Assert.Equal(172818, game.Id);
            Assert.Equal("boardgame", game.Type);
            Assert.NotNull(game.Thumbnail);
            Assert.NotNull(game.Image);
            Assert.Equal("Above and Below", game.Name);
            Assert.Equal(4, game.AlternateNames.Count);
            Assert.StartsWith("Your last village was ransacked by barbarians.", game.Description);
            Assert.Equal(2015, game.YearPublished);
            Assert.Equal(2, game.MinPlayers);
            Assert.Equal(4, game.MaxPlayers);
            Assert.Equal(90, game.PlayingTime);
            Assert.Equal(90, game.MinPlayingTime);
            Assert.Equal(90, game.MaxPlayingTime);
            Assert.Equal(13, game.MinAge);
            Assert.Equal(3, game.Polls.Count);
            Assert.Equal(38, game.Links.Count);
            Assert.Equal(7, game.Versions.Count);
        }

        [Fact]
        public async Task Should_retrieve_a_videogame_by_id()
        {
            HttpClient http = new HttpClient();

            IBoardGameGeekXmlApi2Client bgg = new BoardGameGeekXmlApi2Client(http);

            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 69327 }, versions: true));
            Assert.True(response.Succeeded);

            Thing game = response.Things.FirstOrDefault();
            Assert.NotNull(game);

            Assert.Equal(69327, game.Id);
            Assert.Equal("videogame", game.Type);
            Assert.NotNull(game.Thumbnail);
            Assert.NotNull(game.Image);
            Assert.Equal("The Legend of Zelda", game.Name);
            Assert.Equal(2, game.AlternateNames.Count);
            Assert.StartsWith("From the back of the \"Classic NES Series\"", game.Description);
            Assert.Equal(1, game.MinPlayers);
            Assert.Equal(1, game.MaxPlayers);
            Assert.Equal(15, game.Links.Count);
            Assert.Equal(16, game.Versions.Count);
        }

        [Fact]
        public async Task Should_retrieve_an_rpg_by_id()
        {
            HttpClient http = new HttpClient();

            IBoardGameGeekXmlApi2Client bgg = new BoardGameGeekXmlApi2Client(http);

            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 234669 }, versions: true));
            Assert.True(response.Succeeded);

            Thing game = response.Things.FirstOrDefault();
            Assert.NotNull(game);

            Assert.Equal(234669, game.Id);
            Assert.Equal("Legacy of Dragonholt", game.Name);
            Assert.Equal(2, game.Versions.Count);
        }
    }
}
