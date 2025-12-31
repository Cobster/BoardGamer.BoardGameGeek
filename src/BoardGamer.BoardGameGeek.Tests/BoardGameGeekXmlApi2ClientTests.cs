using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BoardGamer.BoardGameGeek.Tests
{
    public class BoardGameGeekClientTests
    {
        private static readonly string USERNAME = "jakefromstatefarm";
        private readonly IBoardGameGeekXmlApi2Client bgg;

        public BoardGameGeekClientTests()
        {
            bgg = new BoardGameGeekXmlApi2Client(new HttpClient(), new BoardGameGeekXmlApi2ClientOptions { AuthorizationToken = Environment.GetEnvironmentVariable("BGG_AUTH_TOKEN") });
        }

        [Fact]
        public async Task Should_retrieve_user_by_boardgamegeek_username()
        {
            UserResponse response = await bgg.GetUserAsync(new UserRequest(USERNAME, buddies: true, hot: true, top: true));

            Assert.True(response.Succeeded);

            UserResponse.User user = response.Result;

            Assert.NotNull(user);
            Assert.Equal(1266617, user.Id);
            Assert.Equal(USERNAME, user.Name);
            Assert.Equal("Jake", user.FirstName);
            Assert.Equal("Bruun", user.LastName);
            Assert.Equal(2016, user.YearRegistered);
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
                USERNAME,
                stats: true));

            Assert.True(response.Succeeded);

            CollectionResponse.ItemCollection items = response.Result;

            Assert.NotNull(items);
            Assert.Equal(56, items.Count);
        }

        [Fact]
        public async Task Should_retrieve_a_boardgame_by_id()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, versions: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Result.FirstOrDefault();
            Assert.NotNull(game);

            Assert.Equal(172818, game.Id);
            Assert.Equal("boardgame", game.Type);
            Assert.NotNull(game.Thumbnail);
            Assert.NotNull(game.Image);
            Assert.Equal("Above and Below", game.Name);
            Assert.Equal(6, game.AlternateNames.Count);
            Assert.StartsWith("Your last village was ransacked by barbarians.", game.Description);
            Assert.Equal(2015, game.YearPublished);
            Assert.Equal(2, game.MinPlayers);
            Assert.Equal(4, game.MaxPlayers);
            Assert.Equal(90, game.PlayingTime);
            Assert.Equal(90, game.MinPlayingTime);
            Assert.Equal(90, game.MaxPlayingTime);
            Assert.Equal(13, game.MinAge);
            Assert.Equal(3, game.Polls.Count);
            Assert.Equal(43, game.Links.Count);
            Assert.Equal(9, game.Versions.Count);
        }

        [Fact]
        public async Task Should_retrieve_videos()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, videos: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Result.First();

            Assert.Equal(15, game.Videos.Count);
            Assert.Equal(103, game.Videos.Total);

            ThingResponse.Video video = game.Videos[5];

            //Assert.Equal("How to Play Above and Below", video.Title);
        }

        [Fact]
        public async Task Should_retrieve_boardgame_statistics()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, stats: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Result.First();

            Assert.NotNull(game.Statistics);

            // values are subject to change - manually test this
        }

        [Fact]
        public async Task Should_retrieve_boardgame_comments()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, comments: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Result.First();

            Assert.NotNull(game.Comments);

            Assert.Equal(1, game.Comments.Page);
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

            ThingResponse.Item game = response.Result.First();

            Assert.NotNull(game.RatingComments);

            Assert.Equal(1, game.RatingComments.Page);
            Assert.Equal(100, game.RatingComments.Count);

            // Not 100% sure of the sort order on comments. 
            // They seem to be oldest to newest, and if so the following assertions shouldn't change over time.

            ThingResponse.RatingComment comment = game.RatingComments[0];
            Assert.Equal("artfuldodgr42", comment.Username);
            Assert.Equal(10, comment.Rating);
            Assert.Equal("", comment.Value);
        }

        [Fact]
        public async Task Should_retrieve_boardgame_marketplace_listings()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 172818 }, marketplace: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Result.First();

            Assert.NotNull(game.Marketplace);

            ThingResponse.MarketplaceListing listing = game.Marketplace[0];

            Assert.Equal(new DateTimeOffset(2016, 1, 16, 20, 08, 34, 0, TimeSpan.FromHours(0)), listing.ListDate);
            Assert.Equal("EUR", listing.Currency);
            Assert.Equal(51.95, listing.Price);
            Assert.Equal("new", listing.Condition);
            Assert.Equal("weight: 1760 grams + packaging", listing.Notes);
            Assert.Equal("https://boardgamegeek.com/geekmarket/product/869188", listing.Link);

        }


        [Fact]
        public async Task Should_retrieve_a_videogame_by_id()
        {
            ThingResponse response = await bgg.GetThingAsync(new ThingRequest(new int[] { 69327 }, versions: true));
            Assert.True(response.Succeeded);

            ThingResponse.Item game = response.Result.FirstOrDefault();
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

            ThingResponse.Item game = response.Result.FirstOrDefault();
            Assert.NotNull(game);

            Assert.Equal(234669, game.Id);
            Assert.Equal("Legacy of Dragonholt", game.Name);
            Assert.Equal(2, game.Versions.Count);

            // could do more asserts, but i'm not all the interested in the rpg items.
        }

        [Fact]
        public async Task Should_retrieve_logged_plays_for_user()
        {
            PlaysResponse response = await bgg.GetPlaysAsync(new PlaysRequest(USERNAME));
            Assert.True(response.Succeeded);

            PlaysResponse.PlaysCollection collection = response.Result;

            Assert.NotNull(collection);
            Assert.Equal(USERNAME, collection.Username);
            Assert.Equal(1266617, collection.UserId);
            Assert.Equal(1, collection.Total);
            Assert.Equal(1, collection.Page);
            Assert.Equal("https://boardgamegeek.com/xmlapi/termsofuse", collection.TermsOfUse);
            Assert.Single(collection.Plays);

            PlaysResponse.Play play = collection.Plays[0];
            Assert.Equal(34275125, play.Id);
            Assert.Equal(new DateTime(2019, 3, 7), play.Date);
            Assert.Equal(1, play.Quantity);
            Assert.Equal(35, play.Length);
            Assert.False(play.Incomplete);
            Assert.False(play.NowInStats);
            Assert.Equal("hillsboro", play.Location);
            Assert.Equal("this is just comments field", play.Comments);
            Assert.Equal(2, play.Players.Count);

            PlaysResponse.Item item = play.Item;
            Assert.Equal("Isle of Skye: From Chieftain to King", item.Name);
            Assert.Equal("thing", item.ObjectType);
            Assert.Equal(176494, item.ObjectId);
            Assert.Contains("boardgame", item.SubTypes);

            PlaysResponse.Player player1 = play.Players[0];
            Assert.Equal(USERNAME, player1.Username);
            Assert.Equal(1266617, player1.UserId);
            Assert.Equal("Jake Bruun", player1.Name);
            Assert.Equal("0", player1.StartPosition);
            Assert.Equal("green", player1.Color);
            Assert.Equal("42", player1.Score);
            Assert.False(player1.New);
            Assert.Equal(0, player1.Rating);
            Assert.True(player1.Win);

            PlaysResponse.Player player2 = play.Players[1];
            Assert.Equal("", player2.Username);
            Assert.Equal(0, player2.UserId);
            Assert.Equal("Robyn", player2.Name);
            Assert.Equal("0", player2.StartPosition);
            Assert.Equal("red", player2.Color);
            Assert.Equal("34", player2.Score);
            Assert.False(player2.New);
            Assert.Equal(0, player2.Rating);
            Assert.False(player2.Win);
        }

        [Fact]
        public async Task Should_search_for_item()
        {
            SearchRequest request = new SearchRequest("brass", exact: true);
            SearchResponse response = await bgg.SearchAsync(request);
            Assert.True(response.Succeeded);

            SearchResponse.ItemCollection collection = response.Result;

            Assert.Equal(1, collection.Total);
            Assert.Equal("https://boardgamegeek.com/xmlapi/termsofuse", collection.TermsOfUse);
            Assert.Single(collection.Items);

            SearchResponse.Item item = collection.Items[0];

            Assert.Equal("videogame", item.Type);
            Assert.Equal(188167, item.Id);
            Assert.Equal("Brass", item.Name);
            Assert.Null(item.YearPublished);
        }

        [Fact]
        public async Task Should_get_forum_list()
        {
            ForumListRequest request = new ForumListRequest(172818, "thing");
            ForumListResponse response = await bgg.GetForumListAsync(request);
            Assert.NotNull(response.Result);
        }

        [Fact]
        public async Task Should_get_a_forum_by_id()
        {
            ForumsRequest request = new ForumsRequest(1565736);
            ForumsResponse response = await bgg.GetForumsAsync(request);
            Assert.NotNull(response.Result);
        }

        [Fact]
        public async Task Should_get_a_thread_by_id()
        {
            ThreadsRequest request = new ThreadsRequest(2155876);
            ThreadsResponse response = await bgg.GetThreadsAsync(request);
            Assert.NotNull(response.Result);
        }

        [Fact]
        public async Task Should_retrieve_guild_information()
        {
            GuildRequest request = new GuildRequest(1805, members: true);
            GuildResponse response = await bgg.GetGuildAsync(request);
            Assert.True(response.Succeeded);
        }

        [Fact]
        public async Task Should_retrieve_hot_items()
        {
            HotItemsRequest request = new HotItemsRequest("boardgame");
            HotItemsResponse response = await bgg.GetHotItemsAsync(request);
            Assert.NotNull(response.Result);
        }

        [Fact]
        public async Task Should_retrieve_a_family_item()
        {
            FamilyRequest request = new FamilyRequest(86, "boardgamefamily");
            FamilyResponse response = await bgg.GetFamilyAsync(request);
            Assert.NotNull(response.Result);
        }

        [Fact]
        public async Task Should_get_game_by_id()
        {
            var game = await bgg.GetBoardGameAsync(43015);
            Assert.Equal("Hansa Teutonica", game.Name);
            Assert.Equal(2009, game.YearPublished);
        }

        [Fact]
        public async Task Should_get_game_collection_by_username()
        {
            var collection = await bgg.GetCollectionAsync(USERNAME);
            Assert.Equal(55, collection.Count);
        }

        [Fact]
        public async Task Should_get_user_profile_by_username()
        {
            var profile = await bgg.GetUserAsync(USERNAME);
            Assert.Equal(USERNAME, profile.Name);
            Assert.Equal(1266617, profile.Id);
        }
    }
}
