using BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2;
using System;
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
            Assert.Single(user.Top);
            Assert.Single(user.Hot);

        }
    }
}
