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
        private readonly IBoardGameGeekXmlApi2Client bgg;

        public BoardGameGeekClientTests()
        {
            bgg = new BoardGameGeekXmlApi2Client(new HttpClient());
        }

        [Fact]
        public async Task Should_retrieve_user_by_boardgamegeek_username()
        {
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
            CollectionResponse response = await bgg.GetCollectionAsync(new CollectionRequest(
                "jakefromstatefarm",
                stats: true));

            Assert.True(response.Succeeded);

            CollectionResponse.ItemCollection items = response.Items;

            Assert.NotNull(items);
            Assert.Equal(55, items.Count);
        }

        [Fact]
        public async Task Should_retrieve_a_boardgame_by_id()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, versions: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Items.FirstOrDefault();
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
        public async Task Should_retrieve_videos()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, videos: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Items.First();

            Assert.Equal(15, game.Videos.Count);
            Assert.Equal(94, game.Videos.Total);

            ThingResponse.Video video = game.Videos[5];

            Assert.Equal("How to Play Above and Below", video.Title);
        }

        [Fact]
        public async Task Should_retrieve_boardgame_statistics()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, stats: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Items.First();

            Assert.NotNull(game.Statistics);

            // values are subject to change - manually test this
        }

        [Fact]
        public async Task Should_retrieve_boardgame_comments()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, comments: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Items.First();

            Assert.NotNull(game.Comments);

            Assert.Equal(1, game.Comments.Page);
            Assert.Equal(1970, game.Comments.Total); // subject to change
            Assert.Equal(100, game.Comments.Count);

            // Not 100% sure of the sort order on comments. 
            // They seem to be oldest to newest, and if so the following assertions shouldn't change over time.

            ThingResponse.Comment comment = game.Comments[0];
            Assert.Equal("051276", comment.Username);
            Assert.Equal((double?)null, comment.Rating);
            Assert.Equal("Kickstarter version", comment.Value);
        }

        [Fact]
        public async Task Should_retrieve_boardgame_rating_comments()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, ratingComments: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Items.First();

            Assert.NotNull(game.RatingComments);

            Assert.Equal(1, game.RatingComments.Page);
            Assert.Equal(9825, game.RatingComments.Total); // subject to change
            Assert.Equal(100, game.RatingComments.Count);

            // Not 100% sure of the sort order on comments. 
            // They seem to be oldest to newest, and if so the following assertions shouldn't change over time.

            ThingResponse.RatingComment comment = game.RatingComments[0];
            Assert.Equal("artfuldodgr42", comment.Username);
            Assert.Equal(10, comment.Rating);
            Assert.Equal("", comment.Value);
        }

        [Fact]
        public async Task Should_retrieve_a_videogame_by_id()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 69327 }, versions: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Items.FirstOrDefault();
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

            // could be interesting.
        }

        [Fact]
        public async Task Should_retrieve_an_rpg_by_id()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 234669 }, versions: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Items.FirstOrDefault();
            Assert.NotNull(game);

            Assert.Equal(234669, game.Id);
            Assert.Equal("Legacy of Dragonholt", game.Name);
            Assert.Equal(2, game.Versions.Count);

            // could do more asserts, but i'm not all the interested in the rpg items.
        }
    }
}
